using PriceTracker;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureHostConfiguration(hostConfig =>
    {
        hostConfig.SetBasePath(Directory.GetCurrentDirectory());
        hostConfig.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        hostConfig.AddCommandLine(args);
    })
    .ConfigureServices((context, services) =>
    {
        services.AddHostedService<Worker>();
    })
    .Build();

host.Run();