
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
    if(args_exe.Count()>4)
    { 
        CSend send_se = new CSend();
        send_se.m_path_to_file = args_exe[1];
        send_se.m_ftp_dir      = args_exe[2];
        send_se.m_str_cmnd     = args_exe[3];
        send_se.m_str_role     = args_exe[4];
        send_se.Run();
    }
    else
    { 
        Console.WriteLine($"not enough params. must be 4 get {args_exe.Count()}");
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


