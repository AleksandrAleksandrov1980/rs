using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Rabbit
{
    class CRecLog
    {
        public const string HOST_NAME = "192.168.1.59"; 
        const string USER = "rastr"; 
        const string USER_PASS = "rastr";
        const int PORT = 5672; // default 5672
        public void Test()
        {
            try
            {
                Console.WriteLine("test Receiver");

                //var factory = new ConnectionFactory() { HostName = "localhost" };
                var factory = new ConnectionFactory();
                factory.UserName = USER; // guest - resctricted to local only
                factory.Password = USER_PASS;
                factory.VirtualHost = "/";
                factory.HostName = HOST_NAME;
                factory.Port = PORT;
                Console.WriteLine( $" {factory.HostName}:{factory.Port} = {factory.UserName} => {factory.VirtualHost}");
                using(var connection = factory.CreateConnection())
                using(var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(exchange: "logs", type: ExchangeType.Fanout);

                    var queueName = channel.QueueDeclare().QueueName;
                    channel.QueueBind(queue: queueName,
                                    exchange: "logs",
                                    routingKey: "");

                    Console.WriteLine(" [*] Waiting for logs.");

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);
                        Console.WriteLine(" [x] {0}", message);
                    };
                    channel.BasicConsume(queue: queueName,
                                        autoAck: true,
                                        consumer: consumer);

                    Console.WriteLine(" Press [enter] to exit.");
                    Console.ReadLine();
                }

            }
            catch(Exception ex)
            {
                Console.WriteLine($"CRecLog.ERROR! {ex.Source} : {ex.Message}");
            }
        }
    }

}