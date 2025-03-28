using Serilog;
using System.Diagnostics;
using System.Reflection;
using FluentFTP;
using System.Timers;
using System.Text.Json;
using System.Text.Json.Serialization;
using RastrSrvShare;
using static RastrSrvShare.Ccommunicator;
using System.Collections.Generic;
using System;
using static srv_lin.CGramophone;
using static System.Net.Mime.MediaTypeNames;

namespace srv_lin;
public class Worker : BackgroundService
{
    public string? m_str_dir_wrk;
    RastrSrvShare.ftp_hlp m_ftp_hlp = new RastrSrvShare.ftp_hlp();
    private CState m_state = new CState();
    private readonly ILogger<Worker> m_logger;
    private readonly IConfiguration m_configuration;
    public Task<int>? m_tskThreadGram = null;
    private CancellationTokenSource m_cnc_tkn_src = new CancellationTokenSource();
    private object _obj_sync_command = new Object();
    private object _obj_sync_event = new Object();
    public RastrSrvShare.Ccommunicator? m_communicator;
    private static System.Timers.Timer? m_timer_heart_beat;

    public Worker( ILogger<Worker> logger, IConfiguration configuration )
    {
        m_logger = logger;
        m_configuration = configuration;
    }
 
    public int OnEvent( RastrSrvShare.Ccommunicator.Evnt evnt )
    {
        try
        {
            lock(_obj_sync_event)
            {
/*
                if(evnt.en_event == RastrSrvShare.Ccommunicator.enEvents.HEART_BEAT)
                {
                    CState.CService? service = null;
                    service = m_state.m_services.GetValueOrDefault(evnt.from);
                    if(service != null)
                    {
                        service.m_dt_last_seen = DateTime.Now;
                    }
                    else
                    {
                        service =  new CState.CService();
                        service.m_dt_last_seen = DateTime.Now;
                        service.m_str_name = evnt.from;
                        service.m_str_role = evnt.from_role;
                        service.m_lst_errors.Add($"first time detected service at [{service.m_dt_last_seen}]");
                        m_state.m_services.Add(service.m_str_name, service);
                        Log.Information($"first time detected service [{service.m_str_name}] ");
                    }
                    m_state.CheckServicesState();
                }
*/
            }
        }
        catch(Exception ex)
        {
            Log.Error($"OnEvent  exception: [{ex}]");
        }
        return 1;
    }

    public List<string> on_STATE(string[] str_params)
    {
        List<string> ls_ress = new List<string>();
        try
        {
            m_state.CheckServicesState();
            JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize<object>(m_state,options);
            ls_ress.Add(json);
            ls_ress.Add(Consts.m_str_success);
        }
        catch(Exception ex)
        {
            ls_ress.Add($"{Consts.m_str_error}:exception [{ ex }]");
            Log.Error(ls_ress[ls_ress.Count-1]);
        }
        ///State
        return ls_ress; //new List<string>{ m_str_success };
    }

    private Dictionary<int,Task> m_dict_pid_task = new Dictionary<int,Task>();

    void on_PROC_EXIT(Process process, int nTimeOutMs, RastrSrvShare.Ccommunicator.Command command)
    { 
        List<string> ls_ress = new List<string>();
        try
        { 
            process.WaitForExit(nTimeOutMs);
            if(process.HasExited==true)
            { 
                ls_ress.Add($"{Consts.m_str_success}");
                ls_ress.Add($"{process.ExitCode}");
            }
            else
            {
                ls_ress.Add($"{Consts.m_str_error}: time_out!");
                Log.Error(ls_ress[ls_ress.Count-1]);
                process.Kill();
            }
            m_dict_pid_task.Remove(process.Id);
            m_state.m_n_slots_busy--;
        }
        catch(Exception ex) 
        {
            ls_ress.Add($"{Consts.m_str_error}: process.Id={process.Id} on_PROC_EXIT() exception [{ex}]");
            Log.Error(ls_ress[ls_ress.Count-1]);
        }
        PublishEvnt( RastrSrvShare.Ccommunicator.enEvents.FINISH, command.ToString() + "  " + process.HasExited.ToString(), command.en_command, command.tm_mark, command.guid, ls_ress.ToArray() );
    }

    public List<string> on_PROC_RUN(string[] str_params, RastrSrvShare.Ccommunicator.Command command)
    {
        List<string> ls_ress = new List<string>();
        try
        {
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.CreateNoWindow = true;
            int nTimeOutMs = int.Parse(str_params[0]);
            psi.FileName = m_str_dir_wrk+"/"+str_params[1];
            string strArgs = "";
            if(str_params.Length > 2)
            {
                for(int i = 2 ; i<str_params.Length ; i++ )
                {
                    strArgs += str_params[i];
                    strArgs += " ";
                }
            }
            psi.Arguments = strArgs;
            Log.Information($"Start_proc timeout:{nTimeOutMs} ms proc: [{psi.FileName}] [{psi.Arguments}]");
            //Environment.ExpandEnvironmentVariables(@"%SystemRoot%\system32\cmdkey.exe");
            Process? process = Process.Start(psi);
            if( process == null )
            {
                ls_ress.Add($"{Consts.m_str_error}:can't launch [{psi.FileName} {psi.Arguments}]");
                Log.Error(ls_ress[0]);
            }
            else
            {
                ls_ress.Add($"{Consts.m_str_success}: [{process.Id}] launched [{psi.FileName} {psi.Arguments}]");
                ls_ress.Add($"{process.Id}");
                m_state.m_n_slots_busy++;
                m_dict_pid_task.Add(process.Id, Task.Run( ()=>on_PROC_EXIT( process, nTimeOutMs, command ) ) );
            }
        }
        catch(Exception ex)
        {
            ls_ress.Add($"{Consts.m_str_error}:exception {ex}");
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
            //!!! there is en error in function! Process p = Process.GetProcessById(n_pid);
            Process? p2 = GetProcByID(n_pid);
            if(p2 == null)
                throw new Exception($"no such process {str_params[0]}");
            p2.Kill(true);
            bool bl_exit = p2.WaitForExit(1000);
            if(bl_exit == true)
            {
                ls_ress.Add($"{Consts.m_str_success}: proc_kill: {n_pid}");
                Log.Information(ls_ress[ls_ress.Count-1]);
            }
            else
            {
                ls_ress.Add($"{Consts.m_str_error}: proc_kill: {n_pid}");
                Log.Error(ls_ress[ls_ress.Count-1]);
            }
        }
        catch(Exception ex)
        {
            ls_ress.Add($"{Consts.m_str_error}: exception: {ex.ToString()}");
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
                ls_ress.Add($"{Consts.m_str_success}: create dir {str_path_to_new_dir}");
            }
            catch(Exception ex ) 
            {
                Log.Error($"on_DIR_MAKE: Exception: {ex}");
                ls_ress.Add($"{Consts.m_str_error}:{ex}");
            }
        }
        return ls_ress;
    }

    private CGramophone.CRecordParams m_recordParams = new CGramophone.CRecordParams();

    private int on_GRAM_START(string[] str_params)
    {
        if(m_tskThreadGram!=null)
        {
            if(m_tskThreadGram.Status==TaskStatus.Running)
            { 
                Log.Warning($"Gramaphone already runing, remember this params [{string.Join(" ", str_params)}]");
                CGramophone.CGramStartParams gram_start_params_previos = new CGramophone.CGramStartParams();
                gram_start_params_previos.str_args = (string[]) str_params.Clone();
                m_recordParams.m_cs_gram_start_params.Clear();
                m_recordParams.m_cs_gram_start_params.Push(gram_start_params_previos);
                return 2;
            }
            Log.Error("Gramaphone  not run, will be relaunched");
            on_GRAM_STOP(new string[]{""});
        }
        m_recordParams.str_path_srv_wrk_dir = m_str_dir_wrk;
        m_recordParams.m_str_pars.Clear();
        for(int i = 0 ; i < str_params.Length; i++)
        { 
            m_recordParams.m_str_pars.Add(str_params[i]);
        }
        m_cnc_tkn_src.Dispose();
        m_cnc_tkn_src = new CancellationTokenSource(); // "Reset" the cancellation token source...
        m_tskThreadGram = Task.Run(()=>
        {
              return CGramophone.ThreadPlay( m_cnc_tkn_src.Token, m_str_dir_wrk+"/gram.json", m_recordParams, m_communicator );
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
        if(m_tskThreadGram!=null)
        {
            Log.Warning("Gramaphone already runing, will be stoped by KIT!");
            on_GRAM_STOP(new string[]{""});
        }
        m_cnc_tkn_src.Dispose();
        m_cnc_tkn_src = new CancellationTokenSource(); // "Reset" the cancellation token source...

        return -100500;
    }

    static string GetChecksum(string str_hash, string filename)
    {
        using (var hasher = System.Security.Cryptography.HashAlgorithm.Create(str_hash))
        {
            if(hasher == null)
                throw new Exception("Can't create hasher!");
            using (var stream = System.IO.File.OpenRead(filename))
            {
                var hash = hasher.ComputeHash(stream);
                return BitConverter.ToString(hash).Replace("-", "");
            }
        }
    }
    /*
    private enum _enFtpDirection
    {
        UPLOAD,
        DOWNLOAD
    }

    private void ftp_hlp( _enFtpDirection en_ftp_dir, string str_path_from, string str_path_to )
    {
        //using(FtpClient ftp_client = new FtpClient( "192.168.1.59", "anon", "anon", 21 ) )
        using(FtpClient ftp_client = new FtpClient( m_str_ftp_host, m_str_ftp_user, m_str_ftp_pass, m_n_ftp_port ) )
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            ftp_client.Config.FXPDataType = FtpDataType.Binary; 
            ftp_client.Config.EncryptionMode = FtpEncryptionMode.None;
            ftp_client.Config.EncryptionMode = FtpEncryptionMode.None;
            ftp_client.Config.DownloadDataType = FtpDataType.Binary;
            ftp_client.Config.ValidateCertificateRevocation = false;
            System.Security.Cryptography.X509Certificates.X509CertificateCollection x = ftp_client.Config.ClientCertificates;
            ftp_client.Config.DataConnectionType = FtpDataConnectionType.AutoPassive;
            ftp_client.Config.LogToConsole = true;
            ftp_client.ValidateCertificate += (FluentFTP.Client.BaseClient.BaseFtpClient control, FtpSslValidationEventArgs e)=>{ e.Accept = true; };
            FtpProfile ftp_profile = new FtpProfile();
            ftp_profile.Encryption = FtpEncryptionMode.None;
            ftp_client.Connect();
            if(en_ftp_dir == _enFtpDirection.UPLOAD)
            {
                string str_namef    = Path.GetFileName(str_path_from);
                string str_ftp_file = str_path_to + "/"+str_namef;
                ftp_client.CreateDirectory(str_path_to);
                ftp_client.UploadFile(str_path_from,str_ftp_file);
                FileInfo fi = new FileInfo(str_path_from); 
                long size_ftp_file = ftp_client.GetFileSize(str_ftp_file);
                if(fi.Length != size_ftp_file)
                {
                    throw new Exception("size!!");
                }
            }
            else
            {
                string str_namef           = Path.GetFileName(str_path_from);
                string str_path_file_local = str_path_to+"/"+str_namef;
                //string str_ftp_file = str_path_ftp_dir+ "/"+str_namef;
                //ftp_client.CreateDirectory(str_path_ftp_dir);
                long size_ftp_file = ftp_client.GetFileSize(str_path_from);
                ftp_client.DownloadFile(str_path_file_local,str_path_from);
                FileInfo fi = new FileInfo(str_path_file_local); 
                if(fi.Length != size_ftp_file)
                {
                    throw new Exception("size!!");
                }
                //ftp_client.UploadFile(@"C:/rs_wrk/compile.tar_1", "/compile.tar_2");
                try
                {
                    Stopwatch sw2 = new Stopwatch();
                    sw2.Start();
                    string str_hash_fun = "MD5"; //"MD5" "SHA1" "SHA256" "SHA384" "SHA512"
                    string str_hash_val = GetChecksum( str_hash_fun, str_path_file_local );
                    sw2.Stop();
                    Log.Warning($"hash {str_hash_fun} [{str_hash_val}] Ok. time {sw2.Elapsed}");
                }
                catch(Exception)
                {
                }
            }
            ftp_client.Disconnect();
            sw.Stop();
            Log.Warning($"Ok. elapsed time {sw.Elapsed}");
        }
    }
    */


    private List<string>  on_FILE_UPLOAD(string[] str_params)
    {
        List<string> ls_ress = new List<string>();
        try
        {
            if(false) // file_copy from_share to dir
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
                ls_ress.Add($"{Consts.m_str_success}: copyed {str_from} to {str_to}");
            }
            else // file_copy from dir to ftp
            {
                string str_path_file_local = m_str_dir_wrk +"/"+str_params[0];
                m_ftp_hlp.file( ftp_hlp.enFtpDirection.UPLOAD, str_path_file_local, str_params[1] );
                Log.Information($"FILE_UPLOAD {str_path_file_local} to {str_params[1]}");
                ls_ress.Add($"{Consts.m_str_success}: copyed {str_path_file_local} to {str_params[1]}");
            }
        }
        catch(Exception ex)
        {
            Log.Error($"on_FILE_UPLOAD: Exception: {ex} ");
            ls_ress.Add($"{Consts.m_str_error}: {ex} ");
        }
        return ls_ress;
    }

    public List<string> on_FILE_DOWNLOAD(string[] str_params)
    {
        List<string> ls_ress = new List<string>();
        try
        {
            if(false)
            {
                string str_namef = Path.GetFileName(str_params[0])??"";
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
                ls_ress.Add($"{Consts.m_str_success}: copyed {str_from} to {str_to}");
            }
            else
            {
                string str_path_file_local = m_str_dir_wrk +"/"+str_params[1];
                m_ftp_hlp.file( ftp_hlp.enFtpDirection.DOWNLOAD, str_params[0], str_path_file_local );
                Log.Information($"FILE_DOWNLOAD {str_params[0]} to {str_path_file_local}");
                ls_ress.Add($"{Consts.m_str_success}: DOWNLOAD {str_params[0]} to {str_path_file_local}");
            }
        }
        catch(Exception ex)
        {
            Log.Error($"on_FILE_DOWNLOAD: Exception: {ex}");
            ls_ress.Add($"{Consts.m_str_error}:{ex}");
        }
        return ls_ress;
    }

    private List<string> on_DIR_UPLOAD(string[] str_params)
    {
        List<string> ls_ress = new List<string>();
        try
        {
            string str_path_dir_local = m_str_dir_wrk +"/"+str_params[0];
            m_ftp_hlp.dir(ftp_hlp.enFtpDirection.UPLOAD, str_path_dir_local, str_params[1] );
            Log.Information($"DIR_UPLOAD {str_path_dir_local} to {str_params[1]}");
            ls_ress.Add($"{Consts.m_str_success}: copyed {str_path_dir_local} to {str_params[1]}");
        }
        catch(Exception ex)
        {
            Log.Error($"on_DIR_UPLOAD: Exception: {ex} ");
            ls_ress.Add($"{Consts.m_str_error}: {ex} ");
        }
        return ls_ress;
    }

    public List<string> on_DIR_DOWNLOAD(string[] str_params)
    {
        List<string> ls_ress = new List<string>();
        try
        {
            string str_path_dir_local = m_str_dir_wrk +"/"+str_params[1];
            m_ftp_hlp.dir(ftp_hlp.enFtpDirection.DOWNLOAD, str_params[0], str_path_dir_local);
            Log.Information($"DIR_DOWNLOAD {str_params[0]} to {str_path_dir_local}");
            ls_ress.Add($"{Consts.m_str_success}: DOWNLOAD {str_params[0]} to {str_path_dir_local}");
        }
        catch(Exception ex)
        {
            Log.Error($"on_DIR_DOWNLOAD: Exception: {ex}");
            ls_ress.Add($"{Consts.m_str_error}:{ex}");
        }
        return ls_ress;
    }

    private void OnTimedEvent(Object source, ElapsedEventArgs e)
    {
        try
        {
            //Console.WriteLine("The Elapsed event was raised at {0:HH:mm:ss.fff}", e.SignalTime );
            RastrSrvShare.Ccommunicator.Evnt evnt_start = new RastrSrvShare.Ccommunicator.Evnt();
            evnt_start.en_event        = RastrSrvShare.Ccommunicator.enEvents.HEART_BEAT;
            m_communicator?.PublishEvnt( evnt_start );
        }
        catch(Exception ex)
        {
            Log.Error($"OntimerEvent : {ex}"); 
        }
    }

    public void PublishEvnt(RastrSrvShare.Ccommunicator.enEvents en_event, string str_command, RastrSrvShare.Ccommunicator.enCommands en_command , string str_tm_mark, string str_guid, string[] str_results)
    {
        RastrSrvShare.Ccommunicator.Evnt evnt = new RastrSrvShare.Ccommunicator.Evnt();
        //string s = evnt.ToString();
        evnt.en_event        = en_event;
        evnt.command         = str_command;
        evnt.command_en_command = en_command.ToString();
        evnt.command_tm_mark = str_tm_mark;
        evnt.command_guid    = str_guid;
        evnt.results         = str_results;
        m_communicator?.PublishEvnt( evnt );
    }

    public int OnCommand( RastrSrvShare.Ccommunicator.Command command )
    {
        int nRes = 0;
        //Console.WriteLine($"THREAD_onComm_: {Thread.CurrentThread.ManagedThreadId}");   
        lock(_obj_sync_command)
        {
            Log.Information(command.ToString());
            PublishEvnt( RastrSrvShare.Ccommunicator.enEvents.START, command.ToString(), command.en_command, command.tm_mark, command.guid, new string[]{""} );
            List<string> ls_ress = new List<string>();
            RastrSrvShare.Ccommunicator.enEvents en_event_on_cmnd_exit = enEvents.FINISH;
            switch(command.en_command)
            {
                case RastrSrvShare.Ccommunicator.enCommands.STATE:
                    ls_ress = on_STATE(command.pars);                      
                break;

                case RastrSrvShare.Ccommunicator.enCommands.PROC_RUN:
                    ls_ress = on_PROC_RUN(command.pars, command);
                    en_event_on_cmnd_exit = enEvents.NOTIFY;
                break;

                case RastrSrvShare.Ccommunicator.enCommands.PROC_EXTERMINATE:
                    ls_ress = on_PROC_EXTERMINATE(command.pars);                      
                break;

                case RastrSrvShare.Ccommunicator.enCommands.DIR_MAKE:
                    ls_ress = on_DIR_MAKE(command.pars);                      
                break;

                case RastrSrvShare.Ccommunicator.enCommands.GRAM_START:
                    nRes = on_GRAM_START(command.pars);                      
                break;

                case RastrSrvShare.Ccommunicator.enCommands.GRAM_STATE:
                    nRes = on_GRAM_STATE(command.pars);
                break;

                case RastrSrvShare.Ccommunicator.enCommands.GRAM_KIT:
                    nRes = on_GRAM_KIT(command.pars);
                break;

                case RastrSrvShare.Ccommunicator.enCommands.GRAM_STOP:
                    nRes = on_GRAM_STOP(command.pars);
                break;

                case RastrSrvShare.Ccommunicator.enCommands.FILE_UPLOAD:
                    ls_ress = on_FILE_UPLOAD(command.pars);
                break;

                case RastrSrvShare.Ccommunicator.enCommands.FILE_DOWNLOAD:
                    ls_ress = on_FILE_DOWNLOAD(command.pars);
                break;

                case RastrSrvShare.Ccommunicator.enCommands.DIR_UPLOAD:
                    ls_ress = on_DIR_UPLOAD(command.pars);
                break;

                case RastrSrvShare.Ccommunicator.enCommands.DIR_DOWNLOAD:
                    ls_ress = on_DIR_DOWNLOAD(command.pars);
                break;
               
                default:
                    nRes = -1;
                    Log.Error($"unhadled command : {command.en_command}!");
                break;
            }
            PublishEvnt( en_event_on_cmnd_exit, command.ToString(),command.en_command, command.tm_mark,  command.guid, ls_ress.ToArray() );
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
            m_ftp_hlp.m_str_ftp_host = m_configuration.GetValue<string>("r_params:ftp:host") ?? "error";
            m_ftp_hlp.m_str_ftp_user = m_configuration.GetValue<string>("r_params:ftp:user") ?? "error";
            m_ftp_hlp.m_str_ftp_pass = m_configuration.GetValue<string>("r_params:ftp:pass") ?? "error";
            m_ftp_hlp.m_n_ftp_port   = m_configuration.GetValue<int?>  ("r_params:ftp:port") ?? -1;
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

            CInstance c = CInstance.GetCurrent();
            c.SetMsLogger(m_logger);

            RastrSrvShare.CRabbitParams par = new RastrSrvShare.CRabbitParams();
            par.m_str_name          = m_configuration.GetValue<string>("r_params:name", "") ?? "";
            par.m_str_role          = m_configuration.GetValue<string>("r_params:role", "") ?? "";
            m_state.m_n_slots_max   = m_configuration.GetValue<int>   ("r_params:slots_max", 7);
            double d_timer_ms       = m_configuration.GetValue<double>("r_params:heart_beat_ms" ,2000);
            par.m_str_host          = m_configuration.GetValue<string>("r_params:q_host", "") ?? "";
            par.m_n_port            = m_configuration.GetValue<int>   ("r_params:q_port", 5672); // default 5672
            par.m_str_exch_cmnds    = m_configuration.GetValue<string>("r_params:q_exch_commands", "") ?? ""; 
            par.m_str_exch_evnts    = m_configuration.GetValue<string>("r_params:q_exch_events", "") ?? ""; 
            par.m_str_user          = m_configuration.GetValue<string>("r_params:q_user", "") ?? ""; 
            par.m_str_pass          = m_configuration.GetValue<string>("r_params:q_pass", "") ?? "";
            par.m_cncl_tkn          = stoppingToken;

            string strTmp = "";
            strTmp += $"\n\n"; 
            strTmp += $"-----------------------------------------------------------------------------------------\n";
            strTmp += $"-----------------------------------------------------------------------------------------\n";
            strTmp += $"[START][{DateTime.Now.ToString("yyyy_MM_dd___HH_mm")}] Rastr[X]\n";
            strTmp += $"-----------------------------------------------------------------------------------------\n";
            strTmp += $"path to exe: {Process.GetCurrentProcess()?.MainModule?.FileName}\n";
            strTmp += $"--------------------------------------------------------------------\n" ; 
            strTmp += $"name          : {par.m_str_name}\n";
            strTmp += $"role          : {par.m_str_role}\n";
            strTmp += $"heart_beat_ms : {d_timer_ms}\n";
            strTmp += $"--------------------------------------------------------------------\n" ; 
            strTmp += $"q_host        : {par.m_str_host}\n";
            strTmp += $"q_port        : {par.m_n_port}\n"; 
            strTmp += $"q_exch_cmds   : {par.m_str_exch_cmnds}\n"; 
            strTmp += $"q_exch_evts   : {par.m_str_exch_evnts}\n"; 
            strTmp += $"q_user        : {par.m_str_user}\n"; 
            //m_logger.LogWarning($" : {str_q_log_pass}");
            strTmp += $"--------------------------------------------------------------------\n" ; 
            strTmp += $"ftp.host      : {m_ftp_hlp.m_str_ftp_host}\n"; 
            strTmp += $"ftp.user      : {m_ftp_hlp.m_str_ftp_user}\n";
            //strTmp += $"ftp.pass      : {m_str_ftp_host}\n"; 
            strTmp += $"ftp.port      : {m_ftp_hlp.m_n_ftp_port}\n";  
            strTmp += $"--------------------------------------------------------------------\n" ; 
            m_logger.LogWarning(strTmp);
            Log.Warning(strTmp);

            m_timer_heart_beat = new System.Timers.Timer(d_timer_ms);
            m_timer_heart_beat.Elapsed += OnTimedEvent;
            m_timer_heart_beat.AutoReset = true;
            m_timer_heart_beat.Enabled = true;

            RastrSrvShare.CSigner signer_pub = new RastrSrvShare.CSigner();
            string str_path_exe_dir = file_dir_hlp.GetPathExeDir();
            string str_path_pub_key = str_path_exe_dir+"/"+RastrSrvShare.CSigner.str_fname_pub_xml;
            Log.Information($"читаю публичный ключ находящийся [{str_path_pub_key}]");
            int nRes = signer_pub.ReadKey(str_path_pub_key);
            if(nRes<0)
            { 
                Log.Error($"публичный ключ не прочитан.");
                return;
            }

            int i = 0 ;
            for(i= 0; i < 10 ; i++){
                try{
                    if(m_communicator!=null){
                        m_communicator.Dispose();
                        m_communicator = null;
                    }
                    m_communicator = new RastrSrvShare.Ccommunicator();
                    m_communicator.Init(par, signer_pub);
                    Task taskConsumeCommands = Task.Run( ()=>{ m_communicator.ConsumeCmnds(OnCommand, Ccommunicator.enConsumeCmndsMode.STRICT); });
                    Task taskConsumeEvents   = Task.Run( ()=>{ m_communicator.ConsumeEvnts(OnEvent); });
                    await taskConsumeCommands;
                    //m_communicator.Consume(par, m_logger, OnCommand);
                    if(m_communicator!=null){
                        m_communicator.Dispose();
                        m_communicator = null;
                    }
                }
                catch(Exception ex){
                    Log.Error($"[{i}] Communicator exception : {ex}. Will be relaunched.");
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
                m_writer.PublishEvnt($"{i} : hello world!");
                //Task.Run(()=>{m_writer.PublishEvnt($"{i} : hello world!");});
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
            m_logger.LogError($"Program catch exeption: {ex}");

            //https://learn.microsoft.com/en-us/dotnet/core/extensions/windows-service 
            // In order for the Windows Service Management system to leverage configured
            // recovery options, we need to terminate the process with a non-zero exit code.
            //Environment.Exit(1); 
        }
    }
}
