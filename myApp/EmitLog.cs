using RabbitMQ.Client;

namespace Rabbit
{
    class CEmitLog
    {
        //public const string HOST_NAME = "localhost" ; 
        //public const string HOST_NAME = "192.168.1.59" ; 
        public const string HOST_NAME = "192.168.1.59"; 
        //public const string HOST_NAME = "10.31.232.14"; 
        const string USER = "rastr"; 
        const string USER_PASS = "rastr";
        const int PORT = 5672; // default 5672
        //https://swimburger.net/blog/dotnet/how-to-run-a-dotnet-core-console-app-as-a-service-using-systemd-on-linux
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
                    uint k = 0;
                    uint i = 0;
                    for(i = 0 ;  ; i++)
                    {
                        var message = $"TEST_MSG_{k}_{i}";
                        var body = System.Text.Encoding.UTF8.GetBytes(message);
                        channel.BasicPublish(exchange: "logs",
                                            routingKey: "",
                                            basicProperties: null,
                                            body: body);
                        Console.WriteLine(" [x] Sent {0}", message);
                        //System.Threading.Thread.Sleep(100);//
                        System.Threading.Thread.Sleep(1000);//
                        if(i>uint.MaxValue-1)
                        {
                            k++;
                        }
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