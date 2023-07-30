namespace PriceTracker.Services;

public class Worker : BackgroundService
{
    private readonly IHostApplicationLifetime _hostApplicationLifetime;

    private readonly ILogger<Worker> _logger;

    private readonly ApiService _apiService;
    private readonly EmailService _emailService;

    public Worker(IHostApplicationLifetime hostApplicationLifetime, ILogger<Worker> logger, ApiService apiService, EmailService emailService)
    {
        _logger = logger;
        _apiService = apiService;
        _emailService = emailService;
        _hostApplicationLifetime = hostApplicationLifetime;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Starting service...");

        while (!stoppingToken.IsCancellationRequested)
        {
            _hostApplicationLifetime.StopApplication();
        }
    }
}
