namespace srv_lin;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation($"Information-> running at: {DateTimeOffset.Now}" );
            _logger.LogWarning($"Warning-> running at: {DateTimeOffset.Now}" );
            _logger.LogError($"Error-> running at: {DateTimeOffset.Now}" );
            await Task.Delay(1000, stoppingToken);
        }
    }
}
