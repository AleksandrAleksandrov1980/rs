using ASTRALib;
using RabbitMQ.Client;
using Serilog;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using static System.Net.WebRequestMethods;

namespace send_to_web;

class _hlp{
    public string str_id  {get;set;}
    public string str_val {get;set;}
}

//dotnet publish "C:\projects\git_main\rs\frw\frw.csproj" -c Release -o C:\projects\git_main\rs\frw\publish -r win-x64 --self-contained -p:PublishTrimmed=true
class Program{
    static void Main(string[] args) {
        try{
            string str_path_exe_dir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            IConfiguration config = new ConfigurationBuilder()
                    .SetBasePath(str_path_exe_dir)
                    .AddJsonFile("appsettings.json")
                    .Build();                    
            IConfigurationSection con_sec_params = config.GetSection("r_params");
            string str_q_host = con_sec_params.GetRequiredSection("q_host").Value ?? "";
            int    n_q_port   = int.Parse(con_sec_params.GetRequiredSection("q_port").Value ?? "5672");
            string str_q_user = con_sec_params.GetRequiredSection("q_user").Value ?? "";
            string str_q_pass = con_sec_params.GetRequiredSection("q_pass").Value ?? "";
            string str_q_exch = con_sec_params.GetRequiredSection("q_exch").Value ?? "";
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
            Log.Information($"q_host:{str_q_host}");
            Log.Information($"q_port:{n_q_port}");
            Log.Information($"q_user:{str_q_user}");
            Log.Information($"q_user:{str_q_exch}");
            Rastr rastr = new ASTRALib.Rastr();
            Log.Information("Create Rastr");
            string str_path_to_after_oc_file = args[0];
            Log.Information($"Rastr.load:{str_path_to_after_oc_file}");
            rastr.Load( RG_KOD.RG_REPL, str_path_to_after_oc_file, "" );
            List<_hlp> lstHlps = new List<_hlp>();
            Random rnd = new Random();
            ASTRALib.table table_node = rastr.Tables.Item("node");
            System.Array arr = (System.Array)table_node.WriteSafeArray("ny,vras,pn,qn,pg,qg,sta","");
            int nNumRows = arr.GetUpperBound(0) + 1;
            int nNumCols = arr.GetUpperBound(1) + 1;
            for(int nRowNum = 0 ; nRowNum < nNumRows; nRowNum++){ 
                _hlp hlp_vras = new _hlp();
                hlp_vras.str_id  = $"node_{arr.GetValue(nRowNum,0)}_vras";
                hlp_vras.str_val = $"{Math.Round((double)arr.GetValue(nRowNum,1)+rnd.NextDouble()*10,1)}";
                lstHlps.Add( hlp_vras );
                _hlp hlp_sn = new _hlp();
                hlp_sn.str_id  = $"node_{arr.GetValue(nRowNum,0)}_sn";
                hlp_sn.str_val = $"{Math.Round((double)arr.GetValue(nRowNum,2)+rnd.NextDouble()*10,1)}+j{Math.Round((double)arr.GetValue(nRowNum,3)+rnd.NextDouble()*10,1)}";
                lstHlps.Add( hlp_sn );
                _hlp hlp_sg = new _hlp();
                hlp_sg.str_id  = $"node_{arr.GetValue(nRowNum,0)}_sg";
                hlp_sg.str_val = $"{Math.Round((double)arr.GetValue(nRowNum,3)+rnd.NextDouble()*10,1)}+j{Math.Round((double)arr.GetValue(nRowNum,4)+rnd.NextDouble()*10,1)}";
                lstHlps.Add( hlp_sg );
                _hlp hlp_sta = new _hlp();
                hlp_sta.str_id  = $"node_{arr.GetValue(nRowNum,0)}_sta";
                //hlp_sta.str_val = $"{(double)arr.GetValue(nRowNum,5)-1}";
                if(rnd.Next()%2 == 0)
                    hlp_sta.str_val = "0";
                else
                    hlp_sta.str_val = "1";
                lstHlps.Add( hlp_sta );
            }
            ASTRALib.table table_vetv = rastr.Tables.Item("vetv");
            System.Array arr_vetv = (System.Array)table_vetv.WriteSafeArray("ip,iq,np,sta,slb,sle","");
            int nNumRows_vetv = arr_vetv.GetUpperBound(0) + 1;
            int nNumCols_vetv = arr_vetv.GetUpperBound(1) + 1;
            for(int nRowNum = 0 ; nRowNum < nNumRows_vetv; nRowNum++){ 
                int ip = (int)arr_vetv.GetValue(nRowNum,0);
                int iq = (int)arr_vetv.GetValue(nRowNum,1);
                int np = (int)arr_vetv.GetValue(nRowNum,2);
                string str_id = $"vetv_{arr_vetv.GetValue(nRowNum,0)}_{arr_vetv.GetValue(nRowNum,1)}_{arr_vetv.GetValue(nRowNum,2)}";
                _hlp hlp_sta = new _hlp();
                hlp_sta.str_id  = str_id+"_sta";
                if(rnd.Next()%2 == 0)
                    hlp_sta.str_val = "0";
                else
                    hlp_sta.str_val = "1";
               // hlp_sta.str_val = $"{arr_vetv.GetValue(nRowNum,3)}";
                lstHlps.Add( hlp_sta );
                _hlp hlp_slb = new _hlp();
                hlp_slb.str_id  = str_id+"_slb";
                hlp_slb.str_val = $"{(string)arr_vetv.GetValue(nRowNum,4)}";
                lstHlps.Add( hlp_slb );
                _hlp hlp_sle = new _hlp();
                hlp_sle.str_id  = str_id+"_sle";
                hlp_sle.str_val = $"{(string)arr_vetv.GetValue(nRowNum,5)}";
                lstHlps.Add( hlp_sle );
            }
            string str_json_hlps = JsonSerializer.Serialize(lstHlps);
            ConnectionFactory factory = new ConnectionFactory();
            factory.HostName    = str_q_host;
            factory.Port        = n_q_port;
            factory.VirtualHost = "/";
            factory.UserName    = str_q_user; // guest - resctricted to local only
            factory.Password    = str_q_pass;
            Log.Warning($"CONNECTING {factory.HostName}:{factory.Port} = {factory.UserName}");
            using(IConnection connection = factory.CreateConnection()){ 
                using(IModel exchange = connection.CreateModel()){ 
                    Log.Warning($"EXCHANGE_ declare-> [{str_q_exch}]");
                    exchange.ExchangeDeclare( exchange: str_q_exch, type: ExchangeType.Fanout, durable: false, autoDelete:false );
                    Log.Information($"EXCHANGE_ declared-> [{str_q_exch}]");
                    byte[] body = Encoding.UTF8.GetBytes(str_json_hlps);
                    exchange.BasicPublish( exchange: "", routingKey: str_q_exch, basicProperties: null, body: body );
                    Log.Information($"send to: {str_q_exch} : count : {lstHlps.Count}");
                }
            }
            Log.Information("That's all folks!");
        }catch(Exception ex){ 
            Log.Error($"exception: {ex}");
        }
        Console.WriteLine("end.");
    }//static void Main(...) 
}//class Program{
