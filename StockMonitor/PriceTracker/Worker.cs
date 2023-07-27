namespace PriceTracker.Services;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly ApiService _apiService;

    public Worker(ILogger<Worker> logger, ApiService apiService)
    {
        _logger = logger;

        _apiService = apiService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Starting service...");

        while (!stoppingToken.IsCancellationRequested)
        {
            await _apiService.GetStockData("");

            await Task.Delay(900000, stoppingToken);
        }
    }
}
