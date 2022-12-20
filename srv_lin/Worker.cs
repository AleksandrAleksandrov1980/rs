using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using shared;

namespace srv_lin;
public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IConfiguration _configuration;
    public Task<int>? m_tskWrkThreadGram = null;

    public Worker(ILogger<Worker> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration =configuration;

        /*
        IConfigurationRoot MyConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        var IntExample = MyConfig.GetValue<int>("AppSettings:SampleIntValue");
        var AppName = MyConfig.GetValue<string>("AppSettings:APP_Name");
        m_strHostName = MyConfig.GetValue<string>("RPARAMS:HOST_NAME");
        */
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            string? str_name   = _configuration.GetValue<string>("r_params:name");
            string? str_q_host = _configuration.GetValue<string>("r_params:q_host");
            int?    n_q_port   = _configuration.GetValue<int>   ("r_params:q_port"); // default 5672
            string? str_q_exch = _configuration.GetValue<string>("r_params:q_exch"); 
            string? str_q_user = _configuration.GetValue<string>("r_params:q_user"); 
            string? str_q_pass = _configuration.GetValue<string>("r_params:q_pass");
            
            _logger.LogWarning($"--------------------------------------------------------------------" ); 
            _logger.LogWarning($"name        : {str_name}");
            _logger.LogWarning($"q_host      : {str_q_host}");
            _logger.LogWarning($"q_port      : {n_q_port}"); 
            _logger.LogWarning($"q_exch      : {str_q_exch}"); 
            _logger.LogWarning($"q_user      : {str_q_user}"); 
            //_logger.LogWarning($" : {str_q_log_pass}");
            _logger.LogWarning($"--------------------------------------------------------------------" ); 

            //CListener.CParams1 
            CListener.CParams par = new CListener.CParams();
            
            par.m_str_host = _configuration.GetValue<string>("r_params:q_host");
            par.m_n_port   = _configuration.GetValue<int>   ("r_params:q_port"); // default 5672
            par.m_str_exch = _configuration.GetValue<string>("r_params:q_exch"); 
            par.m_str_user = _configuration.GetValue<string>("r_params:q_user"); 
            par.m_str_pass = _configuration.GetValue<string>("r_params:q_pass");
            par.m_cncl_tkn = stoppingToken;
            
            CListener listener = new CListener();
            
            //CListener.ThreadListen(par, _logger);

            int nRes = 0;
            //nRes = await Task.Factory.StartNew<int>(
                /*
            Task<int> t= Task.Factory.StartNew<int>(
                                                         () => CListener.ThreadListen(par, _logger),
                                                         TaskCreationOptions.LongRunning
                                                        ).ConfigureAwait(true);// false //https://blog.stephencleary.com/2012/07/dont-block-on-async-code.html

*/
            Task ttt =  Task.Run(() => {  CListener.ThreadListen(par, _logger) ; } );
            // ttt.Wait(500,stoppingToken);
            await ttt;
            //Task.WaitAll(ttt,5000,stoppingToken);
            
            //Console.ReadLine();
            return ;

            ConnectionFactory factory = new ConnectionFactory();
            factory.HostName = str_q_host;
            factory.Port = (int)n_q_port;
            factory.VirtualHost = "/";
            factory.UserName = str_q_user; // guest - resctricted to local only
            factory.Password = str_q_pass;
            
            _logger.LogWarning( $"CONNECTING {factory.HostName}:{factory.Port} = {factory.UserName} => {factory.VirtualHost}");
            using(var connection = factory.CreateConnection())
            using(var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: str_q_exch, type: ExchangeType.Fanout);

                var queueName = channel.QueueDeclare().QueueName;
                channel.QueueBind(queue: queueName,
                                exchange: str_q_exch,
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

                //Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }

/*
            while (!stoppingToken.IsCancellationRequested)
            {
                string? m_strHostName=null;
                _logger.LogInformation($"{m_strHostName} : Information-> running at: {DateTimeOffset.Now}" );
                _logger.LogWarning($"{m_strHostName} : Warning-> running at: {DateTimeOffset.Now}" );
                _logger.LogError($"{m_strHostName} : Error-> running at: {DateTimeOffset.Now}" );
                await Task.Delay(1000, stoppingToken);
                //Task.WaitAny()
            }
           */ 
        }
        catch( Exception ex)
        {
            _logger.LogError($"catched exeption: {ex.Message}");

            //https://learn.microsoft.com/en-us/dotnet/core/extensions/windows-service 
            // In order for the Windows Service Management system to leverage configured
            // recovery options, we need to terminate the process with a non-zero exit code.
            //Environment.Exit(1); 
        }
    }
}
