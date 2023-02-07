using Serilog;
using System.Diagnostics;
using System.Reflection;
using FluentFTP;

namespace srv_lin;
public class Worker : BackgroundService
{
    private readonly ILogger<Worker> m_logger;
    private readonly IConfiguration m_configuration;
    public Task<int>? m_tskThreadGram = null;
    private CancellationTokenSource m_cnc_tkn_src = new CancellationTokenSource();
    public string? m_str_dir_wrk;
    private object _obj_sync_command = new Object();
    public Ccommunicator? m_communicator;
    public const string m_str_error   = "error";
    public const string m_str_success = "success";

    public Worker( ILogger<Worker> logger, IConfiguration configuration )
    {
        m_logger = logger;
        m_configuration = configuration;
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

 
    private void Tst_DownloadFileFTP()
    {
        try
        {
            // create an FTP client and specify the host, username and password
            // (delete the credentials to use the "anonymous" account)
            //var client = new FtpClient("123.123.123.123", "david", "pass123");
            //var client = new FtpClient("192.168.1.59", "anon", "anon");
            FtpClient ftp_client = new FtpClient("192.168.1.59", "anon", "anon",21);
            //          ftp_client.Config.DataConnectionEncryption = false;
            //ftp_client.Config.EncryptionMode = FtpEncryptionMode.Implicit;
            //ftp_client.Config.EncryptionMode = FtpEncryptionMode.None;
            //            ftp_client.Config.EncryptionMode = FtpEncryptionMode.None;
            ftp_client.Config.FXPDataType = FtpDataType.Binary; 
            //       ftp_client.Config.SslProtocols = System.Security.Authentication.SslProtocols.None;
            ftp_client.Config.EncryptionMode = FtpEncryptionMode.None;
            //ftp_client.Config.EncryptionMode = FtpEncryptionMode.Explicit;
            ftp_client.Config.EncryptionMode = FtpEncryptionMode.None;
            //        ftp_client.Config.DataConnectionEncryption = false;
            ftp_client.Config.DownloadDataType = FtpDataType.Binary;
            //      ftp_client.Config.SslProtocols = System.Security.Authentication.SslProtocols.None;
            ftp_client.Config.ValidateCertificateRevocation = false;
            //ftp_client.SslProtocolActive
            //ftp_client.Config.ValidateAnyCertificate = false;
            System.Security.Cryptography.X509Certificates.X509CertificateCollection x = ftp_client.Config.ClientCertificates;
            //ftp_client.AutoDetect
            //ftp_client.Connect()
            List<FtpProfile> lfp = ftp_client.AutoDetect(false);
            //ftp_client.Config.DataConnectionType = FtpDataConnectionType.PASV;
            ftp_client.Config.DataConnectionType = FtpDataConnectionType.AutoPassive;
            ftp_client.Config.LogToConsole = true;
            ftp_client.ValidateCertificate += (FluentFTP.Client.BaseClient.BaseFtpClient control, FtpSslValidationEventArgs e)=>{ 
                e.Accept = true;
            };
            //ftp_client.ValidateCertificate 
            //ftp_client.Config.DataConnectionEncryption = true;
            //ftp_client.Config.EnableThreadSafeDataConnections = false;
            //FtpConfig ftp_conf = new FtpConfig();
            //ftp_conf.DataConnectionType = FtpDataConnectionType.AutoPassive;
            // connect to the server and automatically detect working FTP settings
            //FtpProfile ftp_profile = ftp_client.AutoConnect();// вот это к херам переопределяет по новой все настройки в соотвествии с её приоритетами!!! от SFTP -> plain FTP
            FtpProfile ftp_profile = new FtpProfile();
            ftp_profile.Encryption = FtpEncryptionMode.None;
            ftp_client.Connect();
            // get a list of files and directories in the "/htdocs" folder
            foreach (FtpListItem item in ftp_client.GetListing("/")) {
                // if this is a file
                if (item.Type == FtpObjectType.File) {
                    // get the file size
                    long size = ftp_client.GetFileSize(item.FullName);
                    Log.Information($"{item.FullName} : size:{size}");
                    // calculate a hash for the file on the server side (default algorithm)
                    //FtpHash hash = ftp_client.GetChecksum(item.FullName); // FILEZILLA так не умеет
                }
                // get modified date/time of the file or folder
                DateTime time = ftp_client.GetModifiedTime(item.FullName);
            }
            // download the file again
            ftp_client.DownloadFile(@"C:/rs_wrk/compile.tar_1", "/compile.tar_1");
            ftp_client.UploadFile(@"C:/rs_wrk/compile.tar_1", "/compile.tar_2");
            // disconnect! good bye!
            ftp_client.Disconnect();
        }
        catch( Exception ex)
        {
            Log.Error($"ftp: {ex.Message}");
        }

        return;
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

    public List<string> on_STATE(string[] str_params)
    {
        return new List<string>{ m_str_success };
    }
    public List<string> on_PROC_RUN(string[] str_params)
    {
        List<string> ls_ress = new List<string>();
        try
        {
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = str_params[0];
            string strArgs = "";
            if(str_params.Length > 1)
            {
                for(int i = 1 ; i<str_params.Length ; i++ )
                {
                    strArgs += str_params[i];
                    strArgs += " ";
                }
            }
            psi.Arguments = strArgs;
            Log.Information($"Start [{psi.FileName}] with arguments [{psi.Arguments}]");
            //Environment.ExpandEnvironmentVariables(@"%SystemRoot%\system32\cmdkey.exe");
            Process? process = Process.Start(psi);
            if( process == null )
            {
                ls_ress.Add($"{m_str_error}:can't launch [{psi.FileName} {psi.Arguments}]");
                Log.Error(ls_ress[0]);
            }
            else
            {
                ls_ress.Add($"{m_str_success}: [{process.Id}] launched [{psi.FileName} {psi.Arguments}]");
                ls_ress.Add($"{process.Id}");
            }
        }
        catch(Exception ex)
        {
            ls_ress.Add($"{m_str_error}:excption { ex.Message }");
            Log.Error(ls_ress[ls_ress.Count-1]);
        }
        return ls_ress;
    }

    public Process? GetProcByID(int id)
    {
        Process[] processlist = Process.GetProcesses();
        //foreach(var proc in processlist ){ Log.Information($"{proc.Id}  :  {proc.ProcessName}");
        return processlist.FirstOrDefault(pr => pr.Id == id);
    }

    public List<string> on_PROC_EXTERMINATE(string[] str_params)
    {
        List<string> ls_ress = new List<string>();
        try
        {
            bool isNumeric = int.TryParse( str_params[0], out int n_pid );
            if(isNumeric == false)
                throw new Exception($"can't parse {str_params[0]}");
            //!!! theris en error in this function! Process p = Process.GetProcessById(n_pid);
            Process? p2 = GetProcByID(n_pid);
            if(p2 == null)
                throw new Exception($"no such process {str_params[0]}");
            p2.Kill(true);
            bool bl_exit = p2.WaitForExit(1000);
            if(bl_exit == true)
            {
                ls_ress.Add($"{m_str_success}: proc_kill: {n_pid}");
                Log.Information(ls_ress[ls_ress.Count-1]);
            }
            else
            {
                ls_ress.Add($"{m_str_error}: proc_kill: {n_pid}");
                Log.Error(ls_ress[ls_ress.Count-1]);
            }
        }
        catch(Exception ex)
        {
            ls_ress.Add($"{m_str_error}: exception: {ex.Message}");
            Log.Error(ls_ress[ls_ress.Count-1]);
        }
        return ls_ress;
    }
    
    public List<string> on_DIR_MAKE(string[] str_params)
    {
        List<string> ls_ress = new List<string>();
        foreach(var str_param in str_params)
        {
            try
            {
                string str_path_to_new_dir = m_str_dir_wrk +"/" +str_param;
                System.IO.Directory.CreateDirectory(str_path_to_new_dir);
                Log.Information($"on_DIR_MAKE: make:{str_path_to_new_dir}");
                ls_ress.Add($"{m_str_success}: create dir {str_path_to_new_dir}");
            }
            catch(Exception ex ) 
            {
                Log.Error($"on_DIR_MAKE: Exception: {ex.Message}");
                ls_ress.Add($"{m_str_error}:{ex.Message}");
            }
        }
        return ls_ress;
    }
    private int on_GRAM_START(string[] str_params)
    {
        if(m_tskThreadGram!=null)
        {
            Log.Error("Gramaphone already runing, will be relaunched");
            on_GRAM_STOP(new string[]{""});
        }
        m_cnc_tkn_src.Dispose();
        m_cnc_tkn_src = new CancellationTokenSource(); // "Reset" the cancellation token source...
        m_tskThreadGram = Task.Run(()=>
        {
            return CGramophone.ThreadPlay( m_cnc_tkn_src.Token, m_str_dir_wrk+"/gram.json", m_communicator );
        });
        return 1;
    }
    private int on_GRAM_STOP(string[] str_params)
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
        }
        else
        {
            Log.Warning("No gramaphone launched");
        }
        return 1;
    }
    private int on_GRAM_STATE(string[] str_params)
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
    public int on_GRAM_KIT(string[] str_params)
    {
        return -100500;
    }
    private List<string>  on_FILE_UPLOAD(string[] str_params)
    {
        List<string> ls_ress = new List<string>();
        try
        {
            string str_from  = str_params[0];
            string str_namef = Path.GetFileName(str_from);
            string str_to    = m_str_dir_wrk+"/"+str_params[1]+ "/"+str_namef;
            FileInfo fi1 = new FileInfo(str_from); 
            File.Copy( str_from, str_to, true );
            FileInfo fi2 = new FileInfo(str_to); 
            if(fi1.Length != fi2.Length)
            {
                throw new Exception($"wrong file size!");
            }
            Log.Information($"copyed {str_from} to {str_to}");
            ls_ress.Add($"{m_str_success}: copyed {str_from} -> {str_to}");
        }
        catch(Exception ex)
        {
            Log.Error($"on_FILE_UPLOAD: Exception: {ex.Message}");
            ls_ress.Add($"{m_str_error}:{ex.Message}");
        }
        return ls_ress;
    }

    public List<string> on_FILE_DOWNLOAD(string[] str_params)
    {
        List<string> ls_ress = new List<string>();
        try
        {
            string str_namef = Path.GetFileName(str_params[0]);
            string str_from  = m_str_dir_wrk+"/"+str_params[0];
            string str_to    = str_params[1]+ "\\"+str_namef;
            FileInfo fi1 = new FileInfo(str_from); 
            File.Copy( str_from, str_to, true );
            FileInfo fi2 = new FileInfo(str_to); 
            if(fi1.Length != fi2.Length)
            {
                throw new Exception($"wrong file size!");
            }
            Log.Information($"copyed {str_from} to {str_to}");
            ls_ress.Add($"{m_str_success}: copyed {str_from} -> {str_to}");
        }
        catch(Exception ex)
        {
            Log.Error($"on_FILE_UPLOAD: Exception: {ex.Message}");
            ls_ress.Add($"{m_str_error}:{ex.Message}");
        }
        return ls_ress;
    }

    public int OnCommand( Ccommunicator.Command command )
    {
        int nRes = 0;
        Console.WriteLine($"THREAD_onComm_: {Thread.CurrentThread.ManagedThreadId}");
        lock(_obj_sync_command)
        {
            Log.Information($"comm : {command.en_command.ToString()} - pars : {String.Join(", ",command.pars)}");
            Ccommunicator.Event evnt_start = new Ccommunicator.Event();
            evnt_start.en_event        = Ccommunicator.enEvents.START;
            evnt_start.command         = command.en_command.ToString() + " : " + String.Join(", ",command.pars);
            evnt_start.tm_mark_command = command.tm_mark;
            m_communicator?.Publish( evnt_start );
            List<string> ls_ress = new List<string>();
            switch(command.en_command)
            {
                case Ccommunicator.enCommands.STATE:
                    ls_ress = on_STATE(command.pars);                      
                break;

                case Ccommunicator.enCommands.PROC_RUN:
                    ls_ress = on_PROC_RUN(command.pars);                      
                break;

                case Ccommunicator.enCommands.PROC_EXTERMINATE:
                    ls_ress = on_PROC_EXTERMINATE(command.pars);                      
                break;

                case Ccommunicator.enCommands.DIR_MAKE:
                    ls_ress = on_DIR_MAKE(command.pars);                      
                break;

                case Ccommunicator.enCommands.GRAM_START:
                    nRes = on_GRAM_START(command.pars);                      
                break;

                case Ccommunicator.enCommands.GRAM_STATE:
                    nRes = on_GRAM_STATE(command.pars);
                break;

                case Ccommunicator.enCommands.GRAM_KIT:
                    nRes = on_GRAM_KIT(command.pars);
                break;

                case Ccommunicator.enCommands.GRAM_STOP:
                    nRes = on_GRAM_STOP(command.pars);
                break;

                case Ccommunicator.enCommands.FILE_UPLOAD:
                    ls_ress = on_FILE_UPLOAD(command.pars);
                break;

                case Ccommunicator.enCommands.FILE_DOWNLOAD:
                    ls_ress = on_FILE_DOWNLOAD(command.pars);
                break;
                
                default:
                    nRes = -1;
                    Log.Error($"unhadled command : {command.en_command.ToString()}!");
                break;
            }
            Ccommunicator.Event evnt_finish = new Ccommunicator.Event();
            evnt_finish.en_event = Ccommunicator.enEvents.FINISH;
            evnt_finish.command = command.en_command.ToString();
            evnt_finish.tm_mark_command = command.tm_mark;
            evnt_finish.results = ls_ress.ToArray();
            m_communicator?.Publish( evnt_finish );
        }
        return 1;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            m_logger.LogInformation("start main()");
            m_str_dir_wrk = m_configuration.GetValue<string>("platform:dir_wrk");
            if(m_str_dir_wrk==null)
            {
                m_logger.LogError("appsettings.[platform].json не задана рабочая директория 'platform:dir_wrk' сервис остановлен.");
               return;
            }
            m_logger.LogInformation($"create dir:{m_str_dir_wrk} ");
            System.IO.Directory.CreateDirectory(m_str_dir_wrk);
            string str_dir_log = m_str_dir_wrk+"/logs/";
            m_logger.LogInformation($"create dir:{str_dir_log} ");
            System.IO.Directory.CreateDirectory(str_dir_log);
            string str_path_log = str_dir_log+"rs.log";
            m_logger.LogInformation($"path log:{str_path_log} ");
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
            c.SetMsLogger(m_logger);
            c.Log(shared.CHlpLog.enErr.INF , "");

            CParams par = new CParams();
            par.m_str_name          = m_configuration.GetValue<string>("r_params:name","");
            par.m_str_host          = m_configuration.GetValue<string>("r_params:q_host","");
            par.m_n_port            = m_configuration.GetValue<int>   ("r_params:q_port",0); // default 5672
            par.m_str_exch_commands = m_configuration.GetValue<string>("r_params:q_exch_commands",""); 
            par.m_str_exch_events   = m_configuration.GetValue<string>("r_params:q_exch_events",""); 
            par.m_str_user          = m_configuration.GetValue<string>("r_params:q_user",""); 
            par.m_str_pass          = m_configuration.GetValue<string>("r_params:q_pass","");
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
            //m_logger.LogWarning($" : {str_q_log_pass}");
            strTmp += $"--------------------------------------------------------------------\n" ; 
            m_logger.LogWarning(strTmp);
            Log.Warning(strTmp);

/*
            Log.Information($"-----------------------------------------------------------------------------------------");
            Log.Information($"---------------------------[START][{DateTime.Now.ToString("yyyy_MM_dd_HH_mm")}]---------------------------------------");
            Log.Information($"-----------------------------------------------------------------------------------------");

            m_logger.LogWarning($"--------------------------------------------------------------------" ); 
            m_logger.LogWarning($"name        : {par.m_str_name}");
            m_logger.LogWarning($"q_host      : {par.m_str_host}");
            m_logger.LogWarning($"q_port      : {par.m_n_port}"); 
            m_logger.LogWarning($"q_exch_cmds : {par.m_str_exch_commands}"); 
            m_logger.LogWarning($"q_exch_evts : {par.m_str_exch_events}"); 
            m_logger.LogWarning($"q_user      : {par.m_str_user}"); 
            //m_logger.LogWarning($" : {str_q_log_pass}");
            m_logger.LogWarning($"--------------------------------------------------------------------" ); 
*/
            int i = 0 ;
            for(i= 0; i < 10 ; i++){
                try{
                    if(m_communicator!=null){
                        m_communicator.Dispose();
                        m_communicator = null;
                    }
                    m_communicator = new Ccommunicator();
                    Task taskCommuicator = Task.Run( ()=>{ m_communicator.Consume(par, m_logger, OnCommand); });
                    await taskCommuicator;
                    //m_communicator.Consume(par, m_logger, OnCommand);
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
            CListener.ThreadListen( par, m_logger, OnCommand );

            m_writer.Init( par, m_logger );
            int i = 0;
            for(i=0;i<10;i++){
                m_writer.Publish($"{i} : hello world!");
                //Task.Run(()=>{m_writer.Publish($"{i} : hello world!");});
                //Thread.Sleep(10);
            }
            //https://github.com/MassTransit/MassTransit
            //https://github.com/EasyNetQ/EasyNetQ
            //#pragma warning disable CS4014
            //Task<int> t= Task.Factory.StartNew<int>(() => CListener.ThreadListen(par, m_logger), TaskCreationOptions.LongRunning
            //                                        ).ConfigureAwait(true);// false //https://blog.stephencleary.com/2012/07/dont-block-on-async-code.html
            */

/*
            Console.WriteLine($"THREAD_1_: {Thread.CurrentThread.ManagedThreadId}");
            Task taskListener = Task.Run(()=>{  
                Console.WriteLine($"THREAD_2_: {Thread.CurrentThread.ManagedThreadId}");
                CListener.ThreadListen( par, m_logger, OnCommand );
                 Console.WriteLine($"THREAD_3_: {Thread.CurrentThread.ManagedThreadId}");
                });
            Console.WriteLine($"THREAD_4_: {Thread.CurrentThread.ManagedThreadId}");
           */ 
           /*
           Console.WriteLine($"THREAD_1_: {Thread.CurrentThread.ManagedThreadId}");
           CListener.ThreadListen( par, m_logger, OnCommand );
            // ttt.Wait(500,stoppingToken);
            //await taskListener;
            //Console.ReadLine();
            if( m_writer != null)
                m_writer.Dispose();
                */
        }
        catch( Exception ex)
        {
            m_logger.LogError($"Program catch exeption: {ex.Message}");

            //https://learn.microsoft.com/en-us/dotnet/core/extensions/windows-service 
            // In order for the Windows Service Management system to leverage configured
            // recovery options, we need to terminate the process with a non-zero exit code.
            //Environment.Exit(1); 
        }
    }
}
