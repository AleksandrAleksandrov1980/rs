using srv_lin;
using Serilog;
using System.Runtime.InteropServices;

//serilog
//https://stackoverflow.com/questions/48251515/serilog-not-creating-log-file-when-running-on-linux
//https://onloupe.com/blog/can-i-log-to-file-mel/

//t grafana loki https://github.com/josephwoodward/Serilog-Sinks-Loki
// elastic https://www.humankode.com/asp-net-core/logging-with-elasticsearch-kibana-asp-net-core-and-docker/

// maui https://mauiman.dev/maui_cli_commandlineinterface.html
//https://egvijayanand.in/2021/04/11/net-maui-debug-with-comet-in-vs-code/
//https://learn.microsoft.com/en-us/dotnet/maui/get-started/first-app?view=net-maui-6.0&tabs=vswin&pivots=devices-android

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
            //for EventLog! https://learn.microsoft.com/ru-ru/dotnet/core/extensions/logging-providers#windows-eventlog
            logging.AddEventLog(configuration => configuration.SourceName = "srv_lin");
        }
    })
    .ConfigureAppConfiguration(ddd=>{
        Console.Write("sdf");
    })
    .Build();
 
CInstance c=CInstance.GetCurrent();

if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
{
  // Do something
  Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    //.WriteTo.Console()
    .WriteTo.File(@"/var/rs_wrk/log.txt",
        rollingInterval: RollingInterval.Day,
        rollOnFileSizeLimit: true)
    .CreateLogger();
}
else
{
    Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Information()
        //.WriteTo.Console()
        .WriteTo.File(@"C:\rs_wrk\log.txt",
            rollingInterval: RollingInterval.Day,
            rollOnFileSizeLimit: true)
        .CreateLogger();
}
    
Log.Information("Hello, Serilog!");
await host.RunAsync();
Log.CloseAndFlush();
