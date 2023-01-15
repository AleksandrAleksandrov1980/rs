using Serilog;
using System.Diagnostics;
using System.Reflection;

namespace srv_lin;
public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IConfiguration _configuration;
    public Task<int>? m_tskThreadGram = null;
    private CancellationTokenSource m_cnc_tkn_src = new CancellationTokenSource();
    public string? m_str_dir_wrk;
    private object _obj_sync_command = new Object();
    public Ccommunicator? m_communicator;

    public Worker( ILogger<Worker> logger, IConfiguration configuration )
    {
        _logger = logger;
        _configuration = configuration;
    }
  
    public class CParams
    {
        public string? m_str_name = "";
        public string? m_str_host = "";
        public int     m_n_port   = 0 ; // default 5672
        public string? m_str_exch_commands = ""; 
        public string? m_str_exch_events   = ""; 
        public string? m_str_user = ""; 
        public string? m_str_pass = "";
        public CancellationToken m_cncl_tkn;
    }

    private int on_GRAM_START()
    {
        //=  Task.Run(() => {  CListener.ThreadListen(par, _logger,  OnCommand) ; } );

        if(m_tskThreadGram!=null)
        {
            Log.Error("Gramaphone already runing, will be relaunched");
            /*m_tskThreadGram.Dispose();
            m_tskThreadGram = null;*/
            //return -100;
            on_GRAM_STOP();
        }
        m_cnc_tkn_src.Dispose();
        m_cnc_tkn_src = new CancellationTokenSource(); // "Reset" the cancellation token source...
        m_tskThreadGram = Task.Run(()=>
        {
            return CGramophone.ThreadPlay( m_cnc_tkn_src.Token, m_str_dir_wrk+"/gram.json" );
        });
        return 1;
    }

    private int on_GRAM_STOP()
    {
        if(m_tskThreadGram!=null)
        {
            m_cnc_tkn_src.Cancel();
            bool blRes = false;
            blRes = m_tskThreadGram.Wait(3000);
            if(blRes==true)
            {
                Log.Warning("Task finished!");
                m_cnc_tkn_src.Dispose();
                m_cnc_tkn_src = new CancellationTokenSource(); // "Reset" the cancellation token source...
                m_tskThreadGram = null;
            }
            else
            {
                Log.Error("Task not finished in time!");
            }
            //m_tskThreadGram;
        }
        else
        {
            Log.Warning("No gramaphone launched");
        }
        
        return 1;
    }

    private int on_GRAM_STATE()
    {
       int nRes = 0;
       if(m_tskThreadGram!=null)
       {
            string strState = ($"OnGramState() : Status-> {m_tskThreadGram.Status}  Excp->{m_tskThreadGram.Exception }");
            switch (m_tskThreadGram.Status)
            {
                case TaskStatus.RanToCompletion:
                    Log.Information($"OnGramState() : RanToCompletion.Result {m_tskThreadGram.Result}");
                    strState += $": RanToCompletion.Result {m_tskThreadGram.Result}";
                    nRes = 1;
                break;

                case TaskStatus.WaitingForActivation:
                    Log.Information($"OnGramState() : WaitingForActivations -> busy ");           
                    nRes = 2;
                break;

                case TaskStatus.Faulted:
                    Log.Information($"OnGramState() : Faulted ");
                    nRes = -13;
                break;
            }
       }
       else
       {
            Log.Warning("NO GRAMAPHONE");
       }
       return 1;
    }

    private void Tst_DownloadFileFTP()
    {
        //https://stackoverflow.com/questions/860638/how-do-i-create-a-directory-on-ftp-server-using-c
        System.Net.WebRequest frequest = System.Net.WebRequest.Create("ftp://192.168.1.59/");
        //frequest.Method = System.Net.WebRequestMethods.Ftp.ListDirectory;
        frequest.Method = System.Net.WebRequestMethods.Ftp.ListDirectoryDetails;
        frequest.Credentials = new System.Net.NetworkCredential("anon", "");
        using (var resp = (System.Net.FtpWebResponse) frequest.GetResponse())
        {
            Console.WriteLine(resp.StatusCode);
            Stream responseStream = resp.GetResponseStream();
            StreamReader reader = new StreamReader(responseStream);
            string str = reader.ReadToEnd();
            //Console.WriteLine(reader.ReadToEnd());
            Log.Information(str);
        }

        //https://stackoverflow.com/questions/2781654/ftpwebrequest-download-file
        //string inputfilepath = @"C:\Temp\FileName.exe";
        string fileDownload = "compile.tar";
        string inputfilepath = @"C:\rs_wrk\"+fileDownload;
        string ftphost = "192.168.1.59";
        string ftpfilepath = "/"+fileDownload;
        string ftpfullpath = "ftp://" + ftphost + ftpfilepath;
        using (System.Net.WebClient request = new System.Net.WebClient())
        {
            request.Credentials = new System.Net.NetworkCredential("anon", "");
            //request.UploadFile(ftpfullpath+"_1",inputfilepath);
            byte[] fileData = request.DownloadData(ftpfullpath);
            using (FileStream file = File.Create(inputfilepath))
            {
                file.Write(fileData, 0, fileData.Length);
                file.Close();
            }
            Log.Information("Download Complete");
         }
    }

    /*
        STATE            = 1,
        RUN_PROC         = 2,
        EXTERMINATE_PROC = 3,
        CREATE_DIR       = 4,
        CLEAR_DIR        = 5,
        GRAM_START       = 6,
        GRAM_STOP        = 7,
        GRAM_KIT         = 8,
        GRAM_STATE       = 9,
    */

    public int OnCommand( Ccommunicator.Command command )
    {
        int nRes = 0;
        Console.WriteLine($"THREAD_onComm_: {Thread.CurrentThread.ManagedThreadId}");
        lock(_obj_sync_command)
        {
            Log.Information($"comm : {command.command.ToString()} - pars : {command.pars}");
            //m_writer.Publish($"comm : {command.command.ToString()} - pars : {command.pars}");
            m_communicator.Publish($"comm : {command.command.ToString()} - pars : {command.pars}");
            switch(command.command)
            {
                case Ccommunicator.enCommands.GRAM_START:
                    nRes = on_GRAM_START();                      
                break;

                case Ccommunicator.enCommands.GRAM_STATE:
                    nRes = on_GRAM_STATE();
                break;

                case Ccommunicator.enCommands.GRAM_STOP:
                    nRes = on_GRAM_STOP();
                break;
                
                default:
                    nRes = -1;
                    Log.Error($"unknown command!");
                break;
            }
        }
        return 1;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            _logger.LogInformation("start main()");
            m_str_dir_wrk = _configuration.GetValue<string>("platform:dir_wrk");
            if(m_str_dir_wrk==null)
            {
                _logger.LogError("appsettings.[platform].json не задана рабочая директория 'platform:dir_wrk' сервис остановлен.");
               return;
            }
            _logger.LogInformation($"create dir:{m_str_dir_wrk} ");
            System.IO.Directory.CreateDirectory(m_str_dir_wrk);
            string str_dir_log = m_str_dir_wrk+"/logs/";
            _logger.LogInformation($"create dir:{str_dir_log} ");
            System.IO.Directory.CreateDirectory(str_dir_log);
            string str_path_log = str_dir_log+"rs.log";
            _logger.LogInformation($"path log:{str_path_log} ");
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .WriteTo.File(str_path_log,
                    rollingInterval: RollingInterval.Day,
                    rollOnFileSizeLimit: true)
                .CreateLogger();
            string str_cal_guid = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_fff");
            //Tst_DownloadFileFTP();
            
            CInstance c=CInstance.GetCurrent();
            c.SetMsLogger(_logger);
            c.Log(shared.CHlpLog.enErr.INF , "");

            CParams par = new CParams();
            par.m_str_name          = _configuration.GetValue<string>("r_params:name","");
            par.m_str_host          = _configuration.GetValue<string>("r_params:q_host","");
            par.m_n_port            = _configuration.GetValue<int>   ("r_params:q_port",0); // default 5672
            par.m_str_exch_commands = _configuration.GetValue<string>("r_params:q_exch_commands",""); 
            par.m_str_exch_events   = _configuration.GetValue<string>("r_params:q_exch_events",""); 
            par.m_str_user          = _configuration.GetValue<string>("r_params:q_user",""); 
            par.m_str_pass          = _configuration.GetValue<string>("r_params:q_pass","");
            par.m_cncl_tkn          = stoppingToken;

            var  proc = Process.GetCurrentProcess();
            
            string strTmp = "";
            strTmp += $"\n\n"; 
            strTmp += $"-----------------------------------------------------------------------------------------\n";
            strTmp += $"-----------------------------------------------------------------------------------------\n";
            strTmp += $"[START][{DateTime.Now.ToString("yyyy_MM_dd___HH_mm")}] Rastr[X]\n";
            strTmp += $"-----------------------------------------------------------------------------------------\n";
            strTmp += $"path to exe: {Process.GetCurrentProcess()?.MainModule?.FileName}\n";
            strTmp += $"--------------------------------------------------------------------\n" ; 
            strTmp += $"name        : {par.m_str_name}\n";
            strTmp += $"q_host      : {par.m_str_host}\n";
            strTmp += $"q_port      : {par.m_n_port}\n"; 
            strTmp += $"q_exch_cmds : {par.m_str_exch_commands}\n"; 
            strTmp += $"q_exch_evts : {par.m_str_exch_events}\n"; 
            strTmp += $"q_user      : {par.m_str_user}\n"; 
            //_logger.LogWarning($" : {str_q_log_pass}");
            strTmp += $"--------------------------------------------------------------------\n" ; 
            _logger.LogWarning(strTmp);
            Log.Warning(strTmp);

/*
            Log.Information($"-----------------------------------------------------------------------------------------");
            Log.Information($"---------------------------[START][{DateTime.Now.ToString("yyyy_MM_dd_HH_mm")}]---------------------------------------");
            Log.Information($"-----------------------------------------------------------------------------------------");

            _logger.LogWarning($"--------------------------------------------------------------------" ); 
            _logger.LogWarning($"name        : {par.m_str_name}");
            _logger.LogWarning($"q_host      : {par.m_str_host}");
            _logger.LogWarning($"q_port      : {par.m_n_port}"); 
            _logger.LogWarning($"q_exch_cmds : {par.m_str_exch_commands}"); 
            _logger.LogWarning($"q_exch_evts : {par.m_str_exch_events}"); 
            _logger.LogWarning($"q_user      : {par.m_str_user}"); 
            //_logger.LogWarning($" : {str_q_log_pass}");
            _logger.LogWarning($"--------------------------------------------------------------------" ); 
*/
            int i = 0 ;
            for(i= 0; i < 10 ; i++){
                try{
                    if(m_communicator!=null){
                        m_communicator.Dispose();
                        m_communicator = null;
                    }
                    m_communicator = new Ccommunicator();
                    Task taskCommuicator = Task.Run( ()=>{ m_communicator.Consume(par, _logger, OnCommand); });
                    await taskCommuicator;
                    //m_communicator.Consume(par, _logger, OnCommand);
                    if(m_communicator!=null){
                        m_communicator.Dispose();
                        m_communicator = null;
                    }
                }
                catch(Exception ex){
                    Log.Error($"[{i}] Communicator exception : {ex.Message}. Will be relaunched.");
                }
                if(stoppingToken.IsCancellationRequested==true)
                {
                    Log.Warning($"[{i}] Communicator canceled.");
                    break;
                }
            }
            /*
            CListener.ThreadListen( par, _logger, OnCommand );

            m_writer.Init( par, _logger );
            int i = 0;
            for(i=0;i<10;i++){
                m_writer.Publish($"{i} : hello world!");
                //Task.Run(()=>{m_writer.Publish($"{i} : hello world!");});
                //Thread.Sleep(10);
            }
            //https://github.com/MassTransit/MassTransit
            //https://github.com/EasyNetQ/EasyNetQ
            //#pragma warning disable CS4014
            //Task<int> t= Task.Factory.StartNew<int>(() => CListener.ThreadListen(par, _logger), TaskCreationOptions.LongRunning
            //                                        ).ConfigureAwait(true);// false //https://blog.stephencleary.com/2012/07/dont-block-on-async-code.html
            */

/*
            Console.WriteLine($"THREAD_1_: {Thread.CurrentThread.ManagedThreadId}");
            Task taskListener = Task.Run(()=>{  
                Console.WriteLine($"THREAD_2_: {Thread.CurrentThread.ManagedThreadId}");
                CListener.ThreadListen( par, _logger, OnCommand );
                 Console.WriteLine($"THREAD_3_: {Thread.CurrentThread.ManagedThreadId}");
                });
            Console.WriteLine($"THREAD_4_: {Thread.CurrentThread.ManagedThreadId}");
           */ 
           /*
           Console.WriteLine($"THREAD_1_: {Thread.CurrentThread.ManagedThreadId}");
           CListener.ThreadListen( par, _logger, OnCommand );
            // ttt.Wait(500,stoppingToken);
            //await taskListener;
            //Console.ReadLine();
            if( m_writer != null)
                m_writer.Dispose();
                */
        }
        catch( Exception ex)
        {
            _logger.LogError($"Program catch exeption: {ex.Message}");

            //https://learn.microsoft.com/en-us/dotnet/core/extensions/windows-service 
            // In order for the Windows Service Management system to leverage configured
            // recovery options, we need to terminate the process with a non-zero exit code.
            //Environment.Exit(1); 
        }
    }
}
