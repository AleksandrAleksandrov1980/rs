﻿using RabbitMQ.Client.Events;
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
using System.Linq;
using System.Security.Cryptography;
using System.ComponentModel.Design;
using System.ComponentModel;

namespace RastrSrvShare;    
public class Ccommunicator: IDisposable
{
    private string m_str_name = "";
    public  string m_str_host_name = "get_host_name_err";   
    public  string m_str_host_ip   = "get_host_ip_err";   
    public  int    m_n_pid = -1;
    public  CRabbitParams m_rabbitParams = new CRabbitParams();
    private int m_n_connection_errors = 0;
    private int m_n_publish_evnt_errors = 0;
    private int m_n_publish_cmnd_errors = 0;
    private int m_n_consume_command_errors = 0;
    private int m_n_consume_evnt_errors = 0;
    private IConnection? m_connection;
    private IModel? m_channel_evnts;
    private IModel? m_channel_cmnds;
    private object m_obj_sync_publish_evnt = new Object();
    private object m_obj_sync_publish_cmnd = new Object();
    public  delegate int OnCommand( Command command );
    public  delegate int OnEvent( Evnt evnt );
    private CSigner? m_signer = null;

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
        DIR_UPLOAD       =  12,
        DIR_DOWNLOAD     =  13,
    }
/*
    public class CommandSerialized
    {
        public string   command { get; set; } = "";
        public string   to      { get; set; } = "";
        public string   from    { get; set; } = "";
        public string   tm_mark { get; set; } = "";
        public string   guid    { get; set; } = "";
        public string[] pars    { get; set; } ={""};
        public string   sign    { get; set; } = "";
    }
*/
    public class Command
    {
        [JsonIgnore]
        public enCommands en_command{ get; set; } = enCommands.ERROR;
        [JsonPropertyName("command")]
        public string     str_event { get{return en_command.ToString();} set{en_command = StrToCommand(value);} } 
        public string     to        { get; set; } = "";
        public string     from      { get; set; } = "";
        public string     role      { get; set; } = ""; 
        public string     tm_mark   { get; set; } = ""; 
        public string     guid      { get; set; } = "";
        public string[]   pars      { get; set; } ={""};
        public string     sign      { get; set; } = "";

        public Command()
        { 
        }

        public override string ToString()
        {
            return $"\n\tfrom: {from} to: {to} \n\tcommand: {en_command.ToString()} pars: {String.Join(", ",pars)}";
        }  

        public byte[] GetBytesForSign()
        { 
            string str_for_sign = str_event+";"+to+";"+from+";"+role+";"+tm_mark+";"+guid+";";
            foreach (string par in pars)
            { 
                str_for_sign += par + ";";
            }
            byte[] bytes_for_sign = Encoding.UTF8.GetBytes(str_for_sign);
            return bytes_for_sign;
        }

        public string MakeSign(CSigner signer)
        { 
            this.sign             = "";
            byte[] bytes_for_sign = GetBytesForSign();
            byte[] bytes_sign     = signer.HashAndSignBytes(bytes_for_sign);
            this.sign             = Convert.ToBase64String(bytes_sign);
            return this.sign;
        }

        public bool IsSignValid(CSigner signer)
        { 
            try
            { 
                byte[] bytes_for_sign    = GetBytesForSign();
                byte[] bytes_signed_data = Convert.FromBase64String(this.sign);
                if(signer.VerifyData( bytes_for_sign, bytes_signed_data ) == true)
                {
                    return true;
                }
                return false;
            }
            catch
            { 
                return false;
            }
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
        /*
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
                    guid    = command_serialized.guid;
                    pars    = command_serialized.pars;
                    sign    = command_serialized.sign;
                }
            }
            else
            {
                en_command = enCommands.ERROR;
            }
        }
        */
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
        public enEvents en_event           { get; set; } = enEvents.ERROR;
        [JsonPropertyName("event")]
        public string str_event            { get{return en_event.ToString();} 
                                             set{en_event = StrToEvent(value);} } 
        public string   to                 { get; set; } = "";
        public string   from               { get; set; } = "";
        public string   from_role          { get; set; } = "";
        public string   command            { get; set; } = "";
        public string   command_en_command { get; set; } = "";
        public string   command_tm_mark    { get; set; } = "";
        public string   command_guid       { get; set; } = "";
        public string   tm_mark            { get; set; } = "";
        public string[] results            { get; set; } ={""};
        public string   sign               { get; set; } = "";

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

        public override string ToString()
        {
            string str_out = "\n";
            foreach(PropertyDescriptor descriptor in TypeDescriptor.GetProperties(this))
            {
                string name  = descriptor.Name;
                object value = descriptor.GetValue(this);
                string str = "";
                if(descriptor.PropertyType==typeof(string[])) 
                { 
                    string[] strings = (string[])value;
                    str = $"{String.Join(", ",strings)}";
                }
                else
                { 
                    str = value.ToString();                    
                }
                if(str.Length>0)
                { 
                    str_out += $"\t{name}:{str}\n";
                }
            }
            return str_out;
        }
    }

    public void Dispose() 
    {
        Log.Information("Writer.Dispose!");
        m_channel_evnts?.Abort();
        m_channel_evnts?.Dispose();
        m_channel_evnts = null;
        m_channel_cmnds?.Abort();
        m_channel_cmnds?.Dispose();
        m_channel_cmnds = null;
        m_connection?.Abort();
        m_connection?.Dispose();
        m_connection = null;
    }

    public static string GetTmMark()
    { 
        return DateTime.Now.ToString("yyyy_MM_dd___HH_mm_ss_fffff");
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

    private void MakeSrvName()
    { 
        m_str_name = $"{m_str_host_name}({m_str_host_ip})={m_rabbitParams?.m_str_name}({m_n_pid})";
        if(m_str_host_name.Length<1)
        { 
            m_str_name = "ERROR";
        }
        if(m_str_host_ip.Length<1)
        { 
            m_str_name = "ERROR";
        }
        if(m_rabbitParams == null)
        { 
            m_str_name = "ERROR";
        }
        if(m_rabbitParams?.m_str_name.Length<1)
        { 
            m_str_name = "ERROR";
        }
        if(m_n_pid<0)
        { 
            m_str_name = "ERROR";
        }
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

    public void Init( CRabbitParams rabbitParams_in, CSigner signer_in )
    { 
        try
        { 
            m_str_host_name     = GetLocalHostName();
            m_str_host_ip       = GetIpv4();
            m_n_pid             = Process.GetCurrentProcess().Id;
            m_rabbitParams      = rabbitParams_in;
            MakeSrvName();
            Log.Information($"My.name [{m_str_name}]");
            ConnectionFactory factory = new ConnectionFactory();
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
            m_signer = signer_in;
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
                evnt.from      = m_str_name;  // $"{m_str_host_name}({m_str_host_ip})={m_rabbitParams.m_str_name}({m_n_pid})";
                evnt.from_role = m_rabbitParams.m_str_role;
                evnt.tm_mark   = GetTmMark(); // DateTime.Now.ToString("yyyy_MM_dd___HH_mm_ss_fffff");
                JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };
                //string json_evnt = JsonSerializer.Serialize(evnt,options);
                string json_evnt = JsonSerializer.Serialize(evnt);
                //Log.Information($"publish to [{m_rabbitParams.m_str_exch_evnts}] : [{json_evnt}]");
                Log.Information($"publish to [{m_rabbitParams.m_str_exch_evnts}] : [{evnt.ToString()}]");
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

    public Command PublishCmnd(Ccommunicator.enCommands en_cmnd, string str_to, string str_role, string[] str_pars)
    {
        Ccommunicator.Command cmnd = new Ccommunicator.Command();
        cmnd.en_command = en_cmnd;
        cmnd.to         = str_to;
        cmnd.role       = str_role;
        cmnd.pars       = str_pars;
        return PublishCmnd(cmnd);
    }

    public Command PublishCmnd(Command cmnd)
    {
        lock(m_obj_sync_publish_cmnd)
        {
            try
            {
                cmnd.from    = m_str_name;  // $"{m_str_host_name}({m_str_host_ip})={m_rabbitParams.m_str_name}({m_n_pid})";
                cmnd.tm_mark = GetTmMark(); // DateTime.Now.ToString("yyyy_MM_dd___HH_mm_ss_fffff");
                cmnd.guid    = Guid.NewGuid().ToString();
                if(m_signer!=null)
                { 
                    cmnd.MakeSign(m_signer);
                    Debug.Assert(cmnd.sign.Length > 0);
                    Debug.Assert(cmnd.IsSignValid(m_signer) == true);
                    string json_cmnd = JsonSerializer.Serialize(cmnd);
                    //Log.Information($"publish to [{m_rabbitParams.m_str_exch_cmnds}] : [{json_cmnd}]");
                    Log.Information($"publish to [{m_rabbitParams.m_str_exch_cmnds}] : [{cmnd.ToString()}]");
                    MakeExchange( ref m_channel_cmnds, m_rabbitParams.m_str_exch_cmnds );
                    byte[] body = Encoding.UTF8.GetBytes(json_cmnd);
                    m_channel_cmnds.BasicPublish( exchange: m_rabbitParams.m_str_exch_cmnds, routingKey: "", basicProperties: null, body: body );
                    return cmnd;
                }
                else
                { 
                    Log.Error($"PublishCmnd() no private_key!");
                }
            }
            catch(Exception ex)
            {
                m_n_publish_cmnd_errors++;   
                Log.Error($"PublishCmnd() exception [{ex}]");
            }
        }
        return null;
    }

    public enum enConsumeCmndsMode
    { 
        STRICT,
        PROMISCUOUS
    };

    public int ConsumeCmnds( OnCommand on_command, enConsumeCmndsMode ConsumeCmndsMode )
    {
        int nRes = 0;
        try
        {
            Log.Information($"start ConsumeCmnds() ConsumeCmndsMode= {ConsumeCmndsMode}!");
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
//                    Log.Information($"{m_rabbitParams.m_str_exch_cmnds} : {message}");
                    Command? command = null;
                    try
                    {
                        command = JsonSerializer.Deserialize<Command>(message)!;
                    }
                    catch(Exception ex)
                    {
                        Log.Error($"exception -> [{ex}] when trying deserialize command [{message}]");
                        command = null;    
                    }   
                    try
                    {
                        if(command == null)
                        { 
                            throw new Exception("Can't deserialize command");
                        }
                        if(command.en_command == enCommands.ERROR)
                        { 
                            throw new Exception("Command with error");
                        }
                        nRes = -1000;
                        if(ConsumeCmndsMode == enConsumeCmndsMode.STRICT)
                        { 
                            if(command.en_command == enCommands.STATE) 
                            {
                                nRes = on_command(command); 
                            }
                            else 
                            {
                                if(  (command.to == m_str_name) ||
                                    ((command.to.Length == 0) && (m_rabbitParams.m_str_role.Equals(command.role)))
                                )
                                //if(m_rabbitParams.m_str_name.StartsWith(command.to))
                                {
                                    if(command.IsSignValid(m_signer) == true)
                                    {
                                        Log.Information($"signature valid.");
                                        nRes = on_command(command); 
                                    }
                                    else
                                    { 
                                        Log.Error($"got command with invalid signature! {message}");
                                    }
                                }
                                else
                                { 
                                    Log.Information($"command for different server my.name[{m_str_name}] != [{command.to }]");
                                }
                            }
                        }
                        else if(ConsumeCmndsMode == enConsumeCmndsMode.PROMISCUOUS)
                        {
                            Log.Information($"PROMISCUOUS mode.");
                            nRes = on_command(command); 
                        }
                        else
                        { 
                            Log.Error($"Unknown ConsumeCmndsMode= {ConsumeCmndsMode}");
                        }
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
        try
        {
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
                    //Console.WriteLine($"EVENTS get message: {message}"); 
                    //Log.Information($"EVENTS get message: {message}");
                    Evnt? evnt = null;
                    try
                    {
                        evnt = JsonSerializer.Deserialize<Evnt>(message);
                    }
                    catch(Exception ex)
                    {
                        Log.Error($"exception -> [{ex}] when trying deserialize event [{message}]");
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
        }
        catch(Exception ex)
        {
            Log.Error($"ConsumeEvnts() exception -> [{ex}] ");
            return -1;
        }
        return 1;
    }
} // class Ccommunicator


