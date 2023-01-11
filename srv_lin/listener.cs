using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Serilog;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Runtime.Serialization;

namespace srv_lin;
public class CListener
{
    public delegate int OnCommand( Command command );

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

    public static int ThreadListen( Worker.CParams par, Microsoft.Extensions.Logging.ILogger _logger, OnCommand on_command )
    {
        int nRes = 0;
        Log.Information("start Listener!");
        ConnectionFactory factory = new ConnectionFactory();
        factory.HostName    = par.m_str_host;
        factory.Port        = par.m_n_port;
        factory.VirtualHost = "/";
        factory.UserName    = par.m_str_user; // guest - resctricted to local only
        factory.Password    = par.m_str_pass;
        _logger.LogWarning($"CONNECTING {factory.HostName}:{factory.Port} = {factory.UserName} => {factory.VirtualHost}");
        Log.Warning($"CONNECTING {factory.HostName}:{factory.Port} = {factory.UserName} => {factory.VirtualHost}");
        using(var connection = factory.CreateConnection())
        using(var channel = connection.CreateModel())
        {
            channel.ExchangeDeclare( exchange: par.m_str_exch_commands, type: ExchangeType.Fanout, durable: false, autoDelete:true );
            var queue_name = channel.QueueDeclare().QueueName;
            channel.QueueBind( queue: queue_name, exchange: par.m_str_exch_commands, routingKey: "" );
            Log.Information($"Waiting for commands queue [{queue_name}]");
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += ( model, ea ) =>
            {
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
                Command command = new Command(command_serialized);
                nRes = on_command(command);
                Log.Information($"command ret: {nRes}");
            };
            //confirmation https://www.rabbitmq.com/tutorials/tutorial-two-dotnet.html
            channel.BasicConsume( queue: queue_name, autoAck: true, consumer: consumer );
            par.m_cncl_tkn.WaitHandle.WaitOne();
            Log.Warning($"listener get cancel signal.");
        }
        return 1;
    }

}