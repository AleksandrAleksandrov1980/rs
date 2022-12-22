using srv_lin;

IHost host = Host.CreateDefaultBuilder(args)
    .UseWindowsService(options =>
    {
        options.ServiceName = "SRV_LIN";
    })
    .UseSystemd()
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
    })
    .ConfigureLogging((context, logging) =>
    {
        //for EventLog! https://learn.microsoft.com/ru-ru/dotnet/core/extensions/logging-providers#windows-eventlog
       logging.AddEventLog(configuration => configuration.SourceName = "srv_lin");
    })
    .ConfigureAppConfiguration(ddd=>{
        Console.Write("sdf");
    })
    .Build();
 
CInstance c=CInstance.GetCurrent();
await host.RunAsync();
