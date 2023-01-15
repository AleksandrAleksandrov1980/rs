using srv_lin;
using Serilog;
using System.Runtime.InteropServices;

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
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            //for Win.EventLog! https://learn.microsoft.com/ru-ru/dotnet/core/extensions/logging-providers#windows-eventlog
            logging.AddEventLog(configuration => configuration.SourceName = "srv_lin");
        }
    })
    .ConfigureAppConfiguration(ConfigurationBuilder=>{
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            ConfigurationBuilder.AddJsonFile($"appsettings.lin.json", optional: true, reloadOnChange: true);
        }
        else
        {
            ConfigurationBuilder.AddJsonFile($"appsettings.win.json", optional: true, reloadOnChange: true);
        }
    })
    .Build();
 
CInstance c=CInstance.GetCurrent();
    
Log.Information("Hello, Serilog!");
await host.RunAsync();
Log.CloseAndFlush();
