using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Net;
using System.Text;
using Serilog;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using static RastrSrvShare.Ccommunicator;
using Microsoft.Extensions.Options;
using Serilog.Core;

namespace RastrSrvShare;    
public class Ccommunicator: IDisposable
{
    public string m_str_host_name = "get_host_name_err";   
    public string m_str_host_ip   = "get_host_ip_err";   
    public int    m_n_pid = -1;
    public  CRabbitParams m_rabbitParams = new CRabbitParams();

    private int m_n_connection_errors = 0;
    private int m_n_publish_evnt_errors = 0;
    private int m_n_publish_cmnd_errors = 0;
    private int m_n_consume_command_errors = 0;
    private int m_n_consume_evnt_errors = 0;

    private IConnection? m_connection;
    private IModel? m_channel_evnts;
    private IModel? m_channel_cmnds;


    object m_obj_sync_publish_evnt = new Object();
    object m_obj_sync_publish_cmnd = new Object();
    public delegate int OnCommand( Command command );
    public delegate int OnEvent( Evnt evnt );

    public enum enCommands 
    {
        ERROR            = -1,
        STATE            =  1,
        PROC_RUN         =  2,
        PROC_EXTERMINATE =  3,
        DIR_MAKE         =  4,
        GRAM_START       =  6,
        GRAM_STOP        =  7,
        GRAM_KIT         =  8,
        GRAM_STATE       =  9,
        FILE_UPLOAD      =  10,
        FILE_DOWNLOAD    =  11,
    }

    public class CommandSerialized
    {
        public string   command { get; set; } = "";
        public string   to      { get; set; } = "";
        public string   from    { get; set; } = "";
        public string   tm_mark { get; set; } = "";//guid on from
        public string[] pars    { get; set; } ={""};
        public string   sign    { get; set; } = "";
    }

    public class Command
    {
        [JsonIgnore]
        public enCommands en_command{ get; set; } = enCommands.ERROR;
        [JsonPropertyName("command")]
        public string str_event         { get{return en_command.ToString();} 
                                          set{en_command = StrToCommand(value);} } 
        public string     to        { get; set; } = "";
        public string     from      { get; set; } = "";
        public string     tm_mark   { get; set; } = ""; //guid on from
        public string[]   pars      { get; set; } ={""};
        public string     sign      { get; set; } = "";

        public Command()
        { 
        }

        public static enCommands StrToCommand(string? str_command)
        {
            if(str_command!=null)
            {
                foreach(var x in Enum.GetValues(typeof(enCommands)))
                {
                    if(str_command == x.ToString())
                    {
                        return (enCommands)x;
                    }
                }
            } 
            return enCommands.ERROR;
        }

        public Command(CommandSerialized? command_serialized)
        {
            if(command_serialized != null)
            {
                en_command = StrToCommand(command_serialized.command);
                if(en_command != enCommands.ERROR)
                {
                    to      = command_serialized.to;
                    from    = command_serialized.from;
                    tm_mark = command_serialized.tm_mark;
                    pars    = command_serialized.pars;
                }
            }
            else
            {
                en_command = enCommands.ERROR;
            }
        }
    }

    public enum enEvents 
    {
        ERROR            = -1,
        NOTIFY           =  1,
        START            =  2,
        FINISH           =  3,
        HEART_BEAT       =  4,
    }

    public class Evnt
    {
        [JsonIgnore]
        public enEvents en_event        { get; set; } = enEvents.ERROR;
        [JsonPropertyName("event")]
        public string str_event         { get{return en_event.ToString();} 
                                          set{en_event = StrToEvent(value);} } 
        public string   to              { get; set; } = "";
        public string   from            { get; set; } = "";
        public string   command         { get; set; } = "";
        public string   tm_mark_command { get; set; } = "";
        public string   tm_mark         { get; set; } = "";//guid on from
        public string[] results         { get; set; } ={""};
        public string   sign            { get; set; } = "";

        public static enEvents StrToEvent(string? str_event)
        {
            if(str_event!=null)
            {
                foreach( var x in Enum.GetValues(typeof(enEvents)))
                {
                    if(str_event == x.ToString())
                    {
                        return (enEvents)x;
                    }
                }
            } 
            return enEvents.ERROR;
        }
    }

    public void Dispose() 
    {
        Log.Information("Writer.Dispose!");
        m_channel_evnts?.Abort();
        m_channel_evnts?.Dispose();
        m_channel_evnts = null;
        m_connection?.Abort();
        m_connection?.Dispose();
        m_connection = null;
    }

    public string GetLocalHostName()
    {
        string nameh = "get_name_error";
        try 
        {
            nameh = Dns.GetHostName();
        }
        catch(Exception)
        {
        }
        return nameh;
    }

    public string GetIpv4()
    {
        try
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
        }
        catch(Exception)
        {
        }
        return "get_ip_error";
    }

    

    private void MakeConnection( ref IConnection? connection, ConnectionFactory? factory )
    {
        try
        {
            if(factory==null)
                throw new Exception("factory==null");
            if(connection!=null)
            {
                if(connection.IsOpen)
                {
                    return;
                }
                else
                {
                    m_n_connection_errors++;
                    Log.Error($"___CONNECTION___ already closed? try dispose and recreate");
                    connection.Dispose();
                }
            }
            connection = factory.CreateConnection();
        }
        catch(Exception ex)
        {
            m_n_connection_errors++;
            Log.Error($"when CreateConnection() exception: {ex}");
        }
    }

    public void Init( CRabbitParams rabbitParams_in )
    { 
        try
        { 
            m_str_host_name     = GetLocalHostName();
            m_str_host_ip       = GetIpv4();
            m_n_pid             = Process.GetCurrentProcess().Id;
            ConnectionFactory factory = new ConnectionFactory();
            m_rabbitParams      = rabbitParams_in;
            factory.HostName    = m_rabbitParams.m_str_host;
            factory.Port        = m_rabbitParams.m_n_port;
            factory.VirtualHost = "/";
            factory.UserName    = m_rabbitParams.m_str_user; // guest - resctricted to local only
            factory.Password    = m_rabbitParams.m_str_pass;
            Log.Warning($"CONNECTING {factory.HostName}:{factory.Port} = {factory.UserName}");
            MakeConnection( ref m_connection, factory );
            if(m_connection == null)
            { 
                throw new Exception($"Init no connection.");
            }
        } 
        catch(Exception ex)
        {
            throw new Exception($"Init exception [{ex}] ");
        }
    }


    private void MakeExchange( ref IModel? exchange, string? str_exch_name )
    {
        try
        {
            if(str_exch_name==null)
                throw new Exception("str_exch_name==null");
            if(exchange!=null)
            {
                if(exchange.IsOpen)
                {
                    return;
                }
                else
                {
                    Log.Warning($"EXCHANGE_ already closed? try dispose and recreate -> [{str_exch_name}]");
                    exchange.Dispose();
                }
            }
            if(m_connection == null)
            {
                Log.Error($"no connection");    
                return;
            }
            exchange = m_connection.CreateModel();
            Log.Warning($"EXCHANGE_ declare-> [{str_exch_name}]");
            exchange.ExchangeDeclare( exchange: str_exch_name, type: ExchangeType.Fanout, durable: false, autoDelete:false );
            Log.Information($"EXCHANGE_ declared-> [{str_exch_name}]");
        }
        catch(Exception ex)
        {
            Log.Error($"when declare exchange {str_exch_name} : {ex}");
        }
    }

    public int PublishEvnt(Ccommunicator.enEvents en_evnt, string[] results)
    {
        Ccommunicator.Evnt evnt = new Ccommunicator.Evnt();
        evnt.en_event = en_evnt;
        evnt.command  = "";
        evnt.results  = results;
        return PublishEvnt(evnt);
    }

    

    public int PublishEvnt(Evnt evnt)
    {
        lock(m_obj_sync_publish_evnt)
        {
            try
            {
                evnt.from = $"{m_str_host_name}({m_str_host_ip})={m_rabbitParams.m_str_name}({m_n_pid})";
                evnt.tm_mark = DateTime.Now.ToString("yyyy_MM_dd___HH_mm_ss_fffff");
                var options = new JsonSerializerOptions { WriteIndented = true };
                string json_evnt = JsonSerializer.Serialize(evnt,options);
                //byte[] jsonUtf8Bytes =JsonSerializer.SerializeToUtf8Bytes(weatherForecast);
                Log.Information($"publish to [{m_rabbitParams.m_str_exch_evnts}] : [{json_evnt}]");
                MakeExchange( ref m_channel_evnts, m_rabbitParams.m_str_exch_evnts );
                byte[] body = Encoding.UTF8.GetBytes(json_evnt);
                m_channel_evnts.BasicPublish( exchange: m_rabbitParams.m_str_exch_evnts, routingKey: "", basicProperties: null, body: body );
            }
            catch(Exception ex)
            {
                m_n_publish_evnt_errors++;   
                Log.Error($"PublishEvnt() exception [{ex}]");
            }
        }
        return 1;
    }

    public int PublishCmnd(Ccommunicator.enCommands en_cmnd, string[] str_cmnd)
    {
        Ccommunicator.Command cmnd = new Ccommunicator.Command();
        cmnd.en_command = en_cmnd;
        cmnd.pars = str_cmnd;
        return PublishCmnd(cmnd);
    }

    

    public int PublishCmnd(Command cmnd)
    {
        lock(m_obj_sync_publish_cmnd)
        {
            try
            {
                cmnd.from = $"{m_str_host_name}({m_str_host_ip})={m_rabbitParams.m_str_name}({m_n_pid})";
                cmnd.tm_mark = DateTime.Now.ToString("yyyy_MM_dd___HH_mm_ss_fffff");
                string json_cmnd = JsonSerializer.Serialize(cmnd);
                Log.Information($"publish to [{m_rabbitParams.m_str_exch_cmnds}] : [{json_cmnd}]");
                MakeExchange( ref m_channel_cmnds, m_rabbitParams.m_str_exch_cmnds );
                byte[] body = Encoding.UTF8.GetBytes(json_cmnd);
                m_channel_cmnds.BasicPublish( exchange: m_rabbitParams.m_str_exch_cmnds, routingKey: "", basicProperties: null, body: body );
            }
            catch(Exception ex)
            {
                m_n_publish_cmnd_errors++;   
                Log.Error($"PublishCmnd() exception [{ex}]");
            }
        }
        return 1;
    }

    

    public int ConsumeCmnds( OnCommand on_command )
    {
        int nRes = 0;
        try
        {
            Log.Information("start ConsumeCmnds()!");
            using(IModel channel_commands = m_connection.CreateModel())
            {
                Log.Warning($"EXCHANGE_COMMANDS-> [{m_rabbitParams.m_str_exch_cmnds}]");
                channel_commands.ExchangeDeclare( exchange: m_rabbitParams.m_str_exch_cmnds, type: ExchangeType.Fanout, durable: false, autoDelete:false );
                MakeExchange( ref m_channel_evnts, m_rabbitParams.m_str_exch_cmnds );
                string queue_name = channel_commands.QueueDeclare().QueueName;
                channel_commands.QueueBind( queue: queue_name, exchange: m_rabbitParams.m_str_exch_cmnds, routingKey: "" );
                Log.Information($"Waiting for commands queue [{queue_name}]");
                EventingBasicConsumer consumer = new EventingBasicConsumer(channel_commands);
                consumer.Received += ( model, ea ) =>
                {
                    //Console.WriteLine($"THREADs: {Thread.CurrentThread.ManagedThreadId}"); //seems like this work in different thread!!
                    byte[] body = ea.Body.ToArray();
                    string message = Encoding.UTF8.GetString(body);
                    Log.Information($"COMMANDS get message: {message}");
                    CommandSerialized? command_serialized = null;
                    try
                    {
                        command_serialized = JsonSerializer.Deserialize<CommandSerialized>(message)!;
                    }
                    catch(Exception ex)
                    {
                        Log.Error($"exception -> [{ex.ToString()}] when trying deserialize command [{message}]");
                        command_serialized = null;    
                    }   
                    try
                    {
                        Command command = new Command(command_serialized);
                        nRes = on_command(command);
                        Log.Information($"command ret: {nRes}");
                    }
                    catch(Exception ex)
                    {
                        m_n_consume_command_errors++;
                        Log.Error($"exception -> [{ex}] when trying execute command [{message}] m_n_consume_errors= {m_n_consume_command_errors}");
                    }
                };
                //confirmation https://www.rabbitmq.com/tutorials/tutorial-two-dotnet.html
                channel_commands.BasicConsume( queue: queue_name, autoAck: true, consumer: consumer );
                m_rabbitParams.m_cncl_tkn.WaitHandle.WaitOne();
                Log.Warning($"listener get cancel signal.");
            }
        }
        catch(Exception ex)
        { 
            Log.Error($"ConsumeCmnds() exception -> [{ex}] ");
            return -1;
        }
        return 1;
    }

    public int ConsumeEvnts( OnEvent on_event )
    {
        int nRes = 0;
        Log.Information("start ConsumeEvnts()!");
        using(IModel channel_events = m_connection.CreateModel())
        {
            Log.Warning($"EXCHANGE_EVENTS-> [{m_rabbitParams.m_str_exch_evnts}]");
            channel_events.ExchangeDeclare( exchange: m_rabbitParams.m_str_exch_evnts, type: ExchangeType.Fanout, durable: false, autoDelete:false );
            string queue_name = channel_events.QueueDeclare().QueueName;
            channel_events.QueueBind( queue: queue_name, exchange: m_rabbitParams.m_str_exch_evnts, routingKey: "" );
            Log.Information($"Waiting for commands queue [{queue_name}]");
            EventingBasicConsumer consumer = new EventingBasicConsumer(channel_events);
            consumer.Received += ( model, ea ) =>
            {
                //seems like this work in different thread!!
                //Console.WriteLine($"THREADs: {Thread.CurrentThread.ManagedThreadId}"); 
                byte[] body = ea.Body.ToArray();
                string message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"EVENTS get message: {message}"); 
                Log.Information($"EVENTS get message: {message}");
                Evnt? evnt = null;
                try
                {
                    evnt = JsonSerializer.Deserialize<Evnt>(message);
                }
                catch(Exception ex)
                {
                    Log.Error($"exception -> [{ex.ToString()}] when trying deserialize event [{message}]");
                    evnt = null;    
                }   
                try
                {
                    nRes = on_event(evnt??new Evnt());
                }
                catch(Exception ex)
                {
                    m_n_consume_evnt_errors++;
                    Log.Error($"exception -> [{ex}] when trying proccess event [{message}] m_n_consume_errors= {m_n_consume_command_errors}");
                }
            };
            //confirmation https://www.rabbitmq.com/tutorials/tutorial-two-dotnet.html
            channel_events.BasicConsume( queue: queue_name, autoAck: true, consumer: consumer );
            m_rabbitParams.m_cncl_tkn.WaitHandle.WaitOne();
            Log.Warning($"listener get cancel signal.");
        }
        return 1;
    }
} // class Ccommunicator


