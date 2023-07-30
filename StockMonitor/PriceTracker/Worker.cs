using PriceTracker.Models;

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
        try
        {
            #region Arguments validation
            string [] arguments = Environment.GetCommandLineArgs();

            if(arguments.Length != 4){
                throw new Exception("Missing arguments!");
            }
            
            string  ticker       = arguments[1];
            decimal sellingPrice = Decimal.Parse(arguments[2]);
            decimal buyingPrice  = Decimal.Parse(arguments[3]);
            #endregion

            // Validate ticker
            bool valid = await _apiService.ValidateTicker(ticker);
            if(!valid)
            {
                throw new Exception("Invalid ticker!");
            }

            decimal currentPrice = 0.0m;
            int     currentDate  = 0;

            while (!stoppingToken.IsCancellationRequested)
            {
                // Get price
                HistoricalDataPrice? data = await _apiService.GetTickerPrice(ticker, currentDate);
                
                if(data != null)
                {  
                    // In some cases only the 'Date' field has a value
                    if (data.Close != null)
                    {
                        currentPrice = (decimal)data.Close;
                    }
                    
                    currentDate = data.Date;

                    // Check price
                    if (currentPrice > sellingPrice)
                    {
                        await _emailService.SendEmail("SELL", ticker, sellingPrice, currentPrice);
                    }

                    if (currentPrice < buyingPrice)
                    {
                        await _emailService.SendEmail("BUY", ticker, sellingPrice, currentPrice);    
                    }
                }

                // Run every minute
                await Task.Delay(60000, stoppingToken);
            }
        }
        catch (Exception e)
        {
            _logger.LogError("Exception while running application: {exception}", e);
        }
        finally 
        {
            _hostApplicationLifetime.StopApplication();
        }
    }
}