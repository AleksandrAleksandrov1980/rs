using RabbitMQ.Client;

namespace Rabbit
{
    class CEmitLog
    {
        //public const string HOST_NAME = "localhost" ; 
        //public const string HOST_NAME = "192.168.1.59" ; 
        public const string HOST_NAME = "192.168.1.59"; 
        const string USER = "rastr"; 
        const string USER_PASS = "rastr";
        const int PORT = 5672; // default 5672
        
        public void test()
        {
            try
            {
                var factory = new ConnectionFactory();
                factory.UserName = USER; // guest - resctricted to local only
                factory.Password = USER_PASS;
                factory.VirtualHost = "/";
                factory.HostName = HOST_NAME;
                factory.Port = PORT;
                Console.WriteLine( $" {factory.HostName}:{factory.Port} = {factory.UserName} => {factory.VirtualHost}");
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(exchange: "logs", type: ExchangeType.Fanout);
                    int i = 0;
                    for(i = 0 ; i < 10000 ; i++)
                    {
                        var message = $"TEST_MSG_{i}";
                        var body = System.Text.Encoding.UTF8.GetBytes(message);
                        channel.BasicPublish(exchange: "logs",
                                            routingKey: "",
                                            basicProperties: null,
                                            body: body);
                        Console.WriteLine(" [x] Sent {0}", message);
                        System.Threading.Thread.Sleep(1000);//
                    }
                }
                Console.WriteLine(" Press [enter] to exit.");
            }
            catch(Exception ex)
            {
                Console.WriteLine($"ERROR! {ex.Source} : {ex.Message}");
            }
        }
    }

}