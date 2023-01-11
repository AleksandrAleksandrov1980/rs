using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Serilog;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Runtime.Serialization;

namespace srv_lin;
public class Ccommunicator: IDisposable
{
    private IConnection? m_connection;
    private IModel? m_channel_events;
    private string? m_str_exch_events;
    public delegate int OnCommand( Command command );

    public void Dispose() 
    {
        Log.Information("Writer.Dispose22222!");
        m_channel_events?.Abort();
        m_channel_events?.Dispose();
        m_channel_events = null;
        m_connection?.Abort();
        m_connection?.Dispose();
        m_connection = null;
    }

    public enum enCommands 
    {
        ERROR            = -1,
        STATE            = 1,
        RUN_PROC         = 2,
        EXTERMINATE_PROC = 3,
        CREATE_DIR       = 4,
        CLEAR_DIR        = 5,
        GRAM_START       = 6,
        GRAM_STOP        = 7,
        GRAM_KIT         = 8,
        GRAM_STATE       = 9,
    }

    public class CommandSerialized
    {
        public string? str_command{ get; set; }
        public string? str_pars   { get; set; }
    }

    public static class enCommands1 
    {
        public static readonly string STATE   = enCommands.STATE.ToString();
    }

    public class Command
    {
        public enCommands command{ get; set; }
        public string?    pars   { get; set; }

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
                command = StrToCommand(command_serialized.str_command);
                if(command != enCommands.ERROR)
                {
                    pars = new string( command_serialized.str_pars);
                }
            }
            else
            {
                command = enCommands.ERROR;
                pars = null;
            }
        }
    }

    public int Publish(string strMsg)
    {
        byte[] body = Encoding.UTF8.GetBytes(strMsg);
        Log.Information($"publish to [{m_str_exch_events}] : [{strMsg}]");
        //Task ttt = Task.Run(()=>{m_channel_events.BasicPublish( exchange: m_str_exch_events, routingKey: "", basicProperties: null, body: body );});
        m_channel_events.BasicPublish( exchange: m_str_exch_events, routingKey: "", basicProperties: null, body: body );
        return 1;
    }

    private int m_n_consume_errors = 0;

    public int Consume( Worker.CParams par, Microsoft.Extensions.Logging.ILogger _logger, OnCommand on_command )
    {
        int nRes = 0;
        Log.Information("start Listener!");
        if(m_connection != null)
        {
            Log.Error("m_connection != null, trying to Dispose first!");
            Dispose();
        }
        m_n_consume_errors = 0;
        //Console.WriteLine($"THREAD_Listener1_: {Thread.CurrentThread.ManagedThreadId}");
        ConnectionFactory factory = new ConnectionFactory();
        factory.HostName    = par.m_str_host;
        factory.Port        = par.m_n_port;
        factory.VirtualHost = "/";
        factory.UserName    = par.m_str_user; // guest - resctricted to local only
        factory.Password    = par.m_str_pass;
        _logger.LogWarning($"CONNECTING {factory.HostName}:{factory.Port} = {factory.UserName} => {factory.VirtualHost}");
        Log.Warning($"CONNECTING {factory.HostName}:{factory.Port} = {factory.UserName} => {factory.VirtualHost}");
        //using(var connection = factory.CreateConnection())
        m_connection = factory.CreateConnection();
        m_channel_events = m_connection.CreateModel();
        m_str_exch_events = par.m_str_exch_events;
        Log.Warning($"EXCHANGE-> [{m_str_exch_events}]");
        m_channel_events.ExchangeDeclare( exchange: m_str_exch_events, type: ExchangeType.Fanout, durable: false, autoDelete:true );
        using(IModel channel_commands = m_connection.CreateModel())
        {
            channel_commands.ExchangeDeclare( exchange: par.m_str_exch_commands, type: ExchangeType.Fanout, durable: false, autoDelete:true );
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
                Log.Information($"get message: {message}");
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
            if(on_command == null)
                throw new Exception("blablabla");
            par.m_cncl_tkn.WaitHandle.WaitOne();
            Log.Warning($"listener get cancel signal.");
        }
        return 1;
    }

}