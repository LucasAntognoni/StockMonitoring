using PriceTracker.Services;
using PriceTracker.Settings;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureHostConfiguration(hostConfig =>
    {
        hostConfig.SetBasePath(Directory.GetCurrentDirectory());
        hostConfig.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        hostConfig.AddCommandLine(args);
    })
    .ConfigureServices((context, services) =>
    {
        services.Configure<ApiSettings>(context.Configuration.GetSection("API"));
        services.Configure<SmtpSettings>(context.Configuration.GetSection("SMTP"));
        services.Configure<EmailSettings>(context.Configuration.GetSection("Sender"));
        services.Configure<List<EmailSettings>>(context.Configuration.GetSection("Recipients"));
        
        services.AddTransient<ApiService>();
        services.AddTransient<EmailService>();

        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();