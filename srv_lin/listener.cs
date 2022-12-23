using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Serilog;

namespace srv_lin;
public class CListener
{
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