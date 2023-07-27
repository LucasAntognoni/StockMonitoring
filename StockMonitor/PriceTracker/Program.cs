using PriceTracker;
using PriceTracker.Configuration;
using PriceTracker.Services;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureHostConfiguration(hostConfig =>
    {
        hostConfig.SetBasePath(Directory.GetCurrentDirectory());
        hostConfig.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        hostConfig.AddCommandLine(args);
    })
    .ConfigureServices((context, services) =>
    {
        services.Configure<Api>(context.Configuration.GetSection("Api"));
        services.AddSingleton<ApiService>();

        services.AddHostedService<Worker>();
    })
    .Build();

host.Run();