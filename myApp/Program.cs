// See https://aka.ms/new-console-template for more information

using Serilog;

using var log = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

Console.WriteLine("Hello, World!");


int i;
for(i=0;i<100;i++)
{
    Console.WriteLine($"Hello, World!{i}");
    int iIwwe;

    log.Information($"Hello, Serilog!{i}");

}
Console.WriteLine($"VSSS!{i}");
Console.WriteLine($"VSSS!{i}");
