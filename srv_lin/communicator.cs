using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Serilog;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Runtime.Serialization;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;

namespace srv_lin;
/*
public class Ccommunicator: IDisposable
{
    private string m_str_name     = "noname_yet";
    public string m_str_host_name = "get_host_name_err";   
    public string m_str_host_ip   = "get_host_ip_err";   
    public int    m_n_pid = -1;
    private ConnectionFactory? m_factory;
    private IConnection? m_connection;
    private IModel? m_channel_events;
    private string? m_str_exch_events;
    public delegate int OnCommand( Command command );
    public delegate int OnEvent( Event evnt );

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

    public void Dispose() 
    {
        Log.Information("Writer.Dispose!");
        m_channel_events?.Abort();
        m_channel_events?.Dispose();
        m_channel_events = null;
        m_connection?.Abort();
        m_connection?.Dispose();
        m_connection = null;
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
        public enCommands en_command{ get; set; } = enCommands.ERROR;
        public string     to        { get; set; } = "";
        public string     from      { get; set; } = "";
        public string     tm_mark   { get; set; } = ""; //guid on from
        public string[]   pars      { get; set; } ={""};
        public string     sign      { get; set; } = "";

        public static enCommands StrToCommand(string? str_command)
        {
            if(str_command!=null)
            {
                foreach( var x in Enum.GetValues(typeof(enCommands)))
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

    public class Event
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

    private int m_n_connection_errors = 0;

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
            Log.Error($"when CreateConnection() : {ex.Message}");
        }
    }

    private int m_n_publish_errors = 0;

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
                    m_n_publish_errors++;
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
            m_n_publish_errors++;   
            Log.Error($"when declare exchange {str_exch_name} : {ex.Message}");
        }
    }

    object m_obj_sync_publish = new Object();

    public int Publish(Ccommunicator.enEvents en_evnt, string[] results)
    {
        Ccommunicator.Event evnt = new Ccommunicator.Event();
        evnt.en_event = en_evnt;
        evnt.command  = "";
        evnt.results  = results;
        return Publish(evnt);
    }

    public string GetLocalHostName()
    {
        string nameh = "get_name_error";
        try 
        {
            nameh = Dns.GetHostName();
        }
        catch(Exception e)
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
        catch(Exception ex )
        {
        }
        return "get_ip_error";
    }

    public int Publish(Event evnt)
    {
        lock(m_obj_sync_publish)
        {
            try
            {
                evnt.from = $"{m_str_host_name}({m_str_host_ip})={m_str_name}({m_n_pid})";
                evnt.tm_mark = DateTime.Now.ToString("yyyy_MM_dd___HH_mm_ss_fffff");
                var options = new JsonSerializerOptions { WriteIndented = true };
                string json_evnt = JsonSerializer.Serialize(evnt,options);
                //byte[] jsonUtf8Bytes =JsonSerializer.SerializeToUtf8Bytes(weatherForecast);
                Log.Information($"publish to [{m_str_exch_events}] : [{json_evnt}]");
                MakeExchange( ref m_channel_events, m_str_exch_events );
                byte[] body = Encoding.UTF8.GetBytes(json_evnt);
                m_channel_events.BasicPublish( exchange: m_str_exch_events, routingKey: "", basicProperties: null, body: body );
            }
            catch(Exception ex)
            {
                m_n_publish_errors++;   
                Log.Error($"exception [{ex.Message}]");
            }
        }
        return 1;
    }

    private int m_n_consume_errors = 0;

    public int ConsumeCommands( Worker.CParams par, Microsoft.Extensions.Logging.ILogger m_logger, OnCommand on_command )
    {
        int nRes = 0;
        Log.Information("start Listener!");
        if(m_connection != null)
        {
            Log.Error("m_connection != null, trying to Dispose first!");
            Dispose();
        }
        m_str_name      = (par.m_str_name!=null)?par.m_str_name:"name_invalid";
        m_str_host_name = GetLocalHostName();
        m_str_host_ip   = GetIpv4();
        m_n_consume_errors = 0;
        m_n_pid            = Process.GetCurrentProcess().Id;
        //Console.WriteLine($"THREAD_Listener1_: {Thread.CurrentThread.ManagedThreadId}");
        m_factory = new ConnectionFactory();
        m_factory.HostName    = par.m_str_host;
        m_factory.Port        = par.m_n_port;
        m_factory.VirtualHost = "/";
        m_factory.UserName    = par.m_str_user; // guest - resctricted to local only
        m_factory.Password    = par.m_str_pass;
        m_logger.LogWarning($"CONNECTING {m_factory.HostName}:{m_factory.Port} = {m_factory.UserName} ");
        Log.Warning($"CONNECTING {m_factory.HostName}:{m_factory.Port} = {m_factory.UserName}");
        MakeConnection( ref m_connection, m_factory );
        //m_connection = m_factory.CreateConnection();
        //m_channel_events = m_connection.CreateModel();
        m_str_exch_events = par.m_str_exch_events;
        //Log.Warning($"EXCHANGE_EVENTS-> [{m_str_exch_events}]");
        //m_channel_events.ExchangeDeclare( exchange: m_str_exch_events, type: ExchangeType.Fanout, durable: false, autoDelete:true );
        MakeExchange( ref m_channel_events, m_str_exch_events );
        using(IModel channel_commands = m_connection.CreateModel())
        {
            Log.Warning($"EXCHANGE_COMMANDS-> [{par.m_str_exch_commands}]");
            channel_commands.ExchangeDeclare( exchange: par.m_str_exch_commands, type: ExchangeType.Fanout, durable: false, autoDelete:false );
            var queue_name = channel_commands.QueueDeclare().QueueName;
            channel_commands.QueueBind( queue: queue_name, exchange: par.m_str_exch_commands, routingKey: "" );
            Log.Information($"Waiting for commands queue [{queue_name}]");
            var consumer = new EventingBasicConsumer(channel_commands);
            consumer.Received += ( model, ea ) =>
            {
                //seems like this work in different thread!!
                //Console.WriteLine($"THREADs: {Thread.CurrentThread.ManagedThreadId}"); 
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
                    Log.Error($"exception -> [{ex.Message}] when trying deserialize command [{message}]");
                    command_serialized = null;    
                }   
                try
                {
                    Command command = new Command(command_serialized);
                    //Console.WriteLine($"THREAD_Listener1_: {Thread.CurrentThread.ManagedThreadId}");
                    nRes = on_command(command);
                    //Console.WriteLine($"THREAD_Listener2_: {Thread.CurrentThread.ManagedThreadId}");
                    Log.Information($"command ret: {nRes}");
                }
                catch(Exception ex)
                {
                    m_n_consume_errors++;
                    Log.Error($"exception -> [{ex.Message}] when trying execute command [{message}] m_n_consume_errors= {m_n_consume_errors}");
                }
            };
            //confirmation https://www.rabbitmq.com/tutorials/tutorial-two-dotnet.html
            channel_commands.BasicConsume( queue: queue_name, autoAck: true, consumer: consumer );
            par.m_cncl_tkn.WaitHandle.WaitOne();
            Log.Warning($"listener get cancel signal.");
        }
        return 1;
    }

    public int ConsumeEvents( Worker.CParams par, Microsoft.Extensions.Logging.ILogger m_logger, OnEvent on_event )
    {
        int nRes = 0;
        Log.Information("start Listener!");
        if(m_connection != null)
        {
            Log.Error("m_connection != null, trying to Dispose first!");
            Dispose();
        }
        ConnectionFactory factory =  new ConnectionFactory();
        factory.HostName    = par.m_str_host;
        factory.Port        = par.m_n_port;
        factory.VirtualHost = "/";
        factory.UserName    = par.m_str_user; // guest - resctricted to local only
        factory.Password    = par.m_str_pass;
        m_logger.LogWarning($"CONNECTING {factory.HostName}:{factory.Port} = {factory.UserName} ");
        Log.Warning($"CONNECTING {factory.HostName}:{factory.Port} = {factory.UserName}");
        IConnection? connection = null;
        MakeConnection( ref connection, factory );
        
        using(IModel channel_events = connection.CreateModel())
        {
            Log.Warning($"EXCHANGE_EVENTS-> [{par.m_str_exch_events}]");
            channel_events.ExchangeDeclare( exchange: par.m_str_exch_events, type: ExchangeType.Fanout, durable: false, autoDelete:false );
            var queue_name = channel_events.QueueDeclare().QueueName;
            channel_events.QueueBind( queue: queue_name, exchange: par.m_str_exch_events, routingKey: "" );
            Log.Information($"Waiting for commands queue [{queue_name}]");
            var consumer = new EventingBasicConsumer(channel_events);
            consumer.Received += ( model, ea ) =>
            {
                //seems like this work in different thread!!
                //Console.WriteLine($"THREADs: {Thread.CurrentThread.ManagedThreadId}"); 
                byte[] body = ea.Body.ToArray();
                string message = Encoding.UTF8.GetString(body);
                Log.Information($"EVENTS get message: {message}");
                Event? evnt = null;
                try
                {
                    evnt = JsonSerializer.Deserialize<Event>(message);
                }
                catch(Exception ex)
                {
                    Log.Error($"exception -> [{ex.Message}] when trying deserialize event [{message}]");
                    evnt = null;    
                }   
                try
                {
                    nRes = on_event(evnt??new Event());
                }
                catch(Exception ex)
                {
                    m_n_consume_errors++;
                    Log.Error($"exception -> [{ex.Message}] when trying proccess event [{message}] m_n_consume_errors= {m_n_consume_errors}");
                }
            };
            //confirmation https://www.rabbitmq.com/tutorials/tutorial-two-dotnet.html
            channel_events.BasicConsume( queue: queue_name, autoAck: true, consumer: consumer );
            par.m_cncl_tkn.WaitHandle.WaitOne();
            Log.Warning($"listener get cancel signal.");
        }
        return 1;
    }

}
*/