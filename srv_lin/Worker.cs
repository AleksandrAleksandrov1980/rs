namespace srv_lin;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly string m_strHostName;

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;

        IConfigurationRoot MyConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        var IntExample = MyConfig.GetValue<int>("AppSettings:SampleIntValue");
        var AppName = MyConfig.GetValue<string>("AppSettings:APP_Name");
        m_strHostName = MyConfig.GetValue<string>("RPARAMS:HOST_NAME");
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation($"{m_strHostName} : Information-> running at: {DateTimeOffset.Now}" );
                _logger.LogWarning($"{m_strHostName} : Warning-> running at: {DateTimeOffset.Now}" );
                _logger.LogError($"{m_strHostName} : Error-> running at: {DateTimeOffset.Now}" );
                await Task.Delay(1000, stoppingToken);
            }
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
