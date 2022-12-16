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
        /*
        // See: https://github.com/dotnet/runtime/issues/47303
        IConfigurationSection ii= context.Configuration.GetSection("Logging");
        IConfigurationSection i2= context.Configuration.GetSection("Loggingeewdfe");
        logging.AddConfiguration( context.Configuration.GetSection("Logging"));
        logging.AddConfiguration( context.Configuration.GetSection("EventSourceg"));
        logging.AddConfiguration( context.Configuration.GetSection("EventLog"));
       */ 
    })
    .ConfigureAppConfiguration(ddd=>{
        Console.Write("sdf");
    })
    .Build();

await host.RunAsync();
