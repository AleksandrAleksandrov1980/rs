using ASTRALib;
using RabbitMQ.Client;
using Serilog;
using System.Text;
using System.Text.Json;

namespace send_to_web;

class _hlp{
    public string str_id  {get;set;}
    public string str_val {get;set;}
}

//dotnet publish "C:\projects\git_main\rs\frw\frw.csproj" -c Release -o C:\projects\git_main\rs\frw\publish -r win-x64 --self-contained -p:PublishTrimmed=true
class Program
{
    static void Main(string[] args) {
        try{
            string str_path_exe_dir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string str_exe_file_name = System.AppDomain.CurrentDomain.FriendlyName;
            string str_path_log = str_path_exe_dir+Path.DirectorySeparatorChar+System.IO.Path.GetFileNameWithoutExtension( str_exe_file_name )+".log";
            Console.WriteLine($"log:{str_path_log}"); 
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .WriteTo.File(str_path_log,
                    rollingInterval: RollingInterval.Day,
                    rollOnFileSizeLimit: true)
                .CreateLogger();
            Rastr rastr = new ASTRALib.Rastr();
            Log.Information("Create Rastr");
            string str_path_to_after_oc_file = args[0];
            Log.Information($"Rastr.load:{str_path_to_after_oc_file}");
            rastr.Load( RG_KOD.RG_REPL, str_path_to_after_oc_file, "" );
            ASTRALib.table table_node = rastr.Tables.Item("node");
            System.Array arr = (System.Array)table_node.WriteSafeArray("ny,vras","");
            int nNumRows = arr.GetUpperBound(0) + 1;
            int nNumCols = arr.GetUpperBound(1) + 1;
            List<_hlp> lstHlps = new List<_hlp>();
            for(int nRowNum = 0 ; nRowNum < nNumRows; nRowNum++){ 
                _hlp hlp = new _hlp();
                hlp.str_id  = $"node_vras_{arr.GetValue(nRowNum,0)}";
                hlp.str_val = $"{Math.Round((double)arr.GetValue(nRowNum,1),1)}";
                lstHlps.Add( hlp );
            }
            string str_json_hlps = JsonSerializer.Serialize(lstHlps);
            const string str_exch_name = "tele";
            ConnectionFactory factory = new ConnectionFactory();

            factory.HostName    = "192.168.1.59";
            factory.Port        = 5672;
            factory.VirtualHost = "/";
            factory.UserName    = "rastr"; // guest - resctricted to local only
            factory.Password    = "rastr";
            Log.Warning($"CONNECTING {factory.HostName}:{factory.Port} = {factory.UserName}");
            using(IConnection connection = factory.CreateConnection()){ 
                using(IModel exchange = connection.CreateModel()){ 
                    Log.Warning($"EXCHANGE_ declare-> [{str_exch_name}]");
                    exchange.ExchangeDeclare( exchange: str_exch_name, type: ExchangeType.Fanout, durable: false, autoDelete:false );
                    Log.Information($"EXCHANGE_ declared-> [{str_exch_name}]");
                    byte[] body = Encoding.UTF8.GetBytes(str_json_hlps);
                    exchange.BasicPublish( exchange: str_exch_name, routingKey: "", basicProperties: null, body: body );
                    Log.Information($"to: {str_exch_name} : count : {lstHlps.Count}");
                }
            }
            Log.Information("That's all folks!");
        }catch(Exception ex){ 
            Log.Error($"exception: {ex}");
        }
        Console.WriteLine("end.");
    }
}
