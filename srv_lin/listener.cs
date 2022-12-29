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
    /*
    public static class enCommands 
    {
        public static readonly string STATE            = "STATE";
        public static readonly string RUN_PROC         = "RUN_PROC";
        public static readonly string EXTERMINATE_PROC = "EXTERMINATE_PROC";
        public static readonly string CREATE_DIR       = "CREATE_DIR";
        public static readonly string CLEAR_DIR        = "CLEAR_DIR";
        public static readonly string GRAM_START       = "GRAM_START";
        public static readonly string GRAM_STOP        = "GRAM_STOP";
        public static readonly string GRAM_KIT         = "GRAM_KIT";
        public static readonly string GRAM_STATE       = "STATE";
    }
*/
    public  enum enCommands 
    {
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

    //[JsonConverter(typeof(EmptyArrayToObjectConverter<Command>))]
    public class Command
    {
        public string? command{ get; set; }
        public string? pars   { get; set; }
    }

    public enum xz
    {
     d,
     sdf   
    }
/*
    public static bool TryParseJson<T>(this string @this, out T result)
    {
        bool success = true;
        var settings = new JsonSerializerSettings
        {
            Error = (sender, args) => { success = false; args.ErrorContext.Handled = true; },
            MissingMemberHandling = MissingMemberHandling.Error
        };
        result = JsonConvert.DeserializeObject<T>(@this, settings);
        return success;
    }
*/
    public class CParams
    {
        public string m_str_host = "";
        public int    m_n_port   = 0 ; // default 5672
        public string m_str_exch = ""; 
        public string m_str_user = ""; 
        public string m_str_pass = "";
        public CancellationToken m_cncl_tkn;
    }

   // public delegate int OnLog( shared.CHlpLog.CLogEntry log_entry );
    public delegate int OnCom();

    public CListener()
    {
        //Microsoft.Extensions.Logging.LoggerMessage();
        //

    }

    public static int ThreadListen( CParams par, Microsoft.Extensions.Logging.ILogger _logger)
    {
        Console.WriteLine($"hello world\n");
        Log.Information("Hccccccccccccccccccceldddlo, Serilog!");

        
        var x = Enum.GetValues(typeof(xz));
        //var x1 = .GetValues(typeof(Commands));

        ConnectionFactory factory = new ConnectionFactory();
        factory.HostName = par.m_str_host;
        factory.Port = par.m_n_port;
        factory.VirtualHost = "/";
        factory.UserName = par.m_str_user; // guest - resctricted to local only
        factory.Password = par.m_str_pass;
        
        _logger.LogWarning( $"CONNECTING {factory.HostName}:{factory.Port} = {factory.UserName} => {factory.VirtualHost}");
        using(var connection = factory.CreateConnection())
        using(var channel = connection.CreateModel())
        {
            channel.ExchangeDeclare(exchange: par.m_str_exch, type: ExchangeType.Fanout);

            var queueName = channel.QueueDeclare().QueueName;
            channel.QueueBind(queue: queueName,
                            exchange: par.m_str_exch,
                            routingKey: "");

            //Console.WriteLine(" [*] Waiting for logs.");
            _logger.LogInformation($" [*] Waiting for [{queueName}]");

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                Command? command;    
                try
                {
                    JsonSerializerOptions j = new JsonSerializerOptions();
                    
                     command = JsonSerializer.Deserialize<Command>((string)message)!;
                }
                catch
                {}

                //WeatherForecast? weatherForecast = 
                //JsonSerializer.Deserialize<WeatherForecast>(jsonString);

                _logger.LogInformation($" [x] {message}");
            };
            channel.BasicConsume(queue: queueName,
                                autoAck: true,
                                consumer: consumer);

            //Console.WriteLine(" Press [enter] to exit.");(
            par.m_cncl_tkn.WaitHandle.WaitOne();
            _logger.LogWarning("CANCELLED!!");
            //Console.ReadLine();
        }

        return 1;
    }

}