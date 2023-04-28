
using publish;
using System.Xml.Linq;

try
{
    Console.WriteLine("Hello, World!");
    string[] args_exe= Environment.GetCommandLineArgs();
    foreach (string arg in args_exe)
    { 
        Console.WriteLine($"Hello, {arg}!");
    }
    if(args_exe.Count()>2)
    { 
        CSendSe send_se = new CSendSe();
        send_se.cmnd            = args_exe[1];
        send_se.path_to_file_se = args_exe[2];
        send_se.Run();
    }
    else
    { 
        Console.WriteLine($"not enough params");
    }
}
catch(Exception ex)
{ 
    Console.WriteLine($"exception:{ex}");
}
finally
{ 
    Console.WriteLine("Bye!");
    Console.ReadLine();
}


