namespace PriceTracker.Services;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;

    private readonly ApiService _apiService;
    private readonly EmailService _emailService;

    public Worker(ILogger<Worker> logger, ApiService apiService, EmailService emailService)
    {
        _logger = logger;
        _apiService = apiService;
        _emailService = emailService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Starting service...");

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(60000, stoppingToken);
        }
    }
}
