using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using shared;
using Serilog;

namespace srv_lin;
public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IConfiguration _configuration;
    public Task<int>? m_tskWrkThreadGram = null;

    public Worker(ILogger<Worker> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration =configuration;
    }

    private object _obj_sync_command = new Object();

    public int OnCommand( CListener.Command command )
    {
        int nRes = 0;
        lock(_obj_sync_command)
        {
            Log.Information($"comm : {command.command.ToString()} - pars : {command.pars}");

            switch(command.command)
            {
                case CListener.enCommands.GRAM_START:
                    nRes = on_GRAM_START();
                break;
                
                default:
                    nRes = -1;
                    Log.Error($"unknown command!");
                break;
            }
        }
        return 1;
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

    private Cgramophone? m_gramaphone = null;

    private int on_GRAM_START()
    {
        //return 1;
        int nRes = 0;
        m_gramaphone = new Cgramophone();
        nRes = m_gramaphone.DeSerializeRecordFromJsonFile(@"C:\projects\gitmain\rs\wrk\gram.json");
        Cgramophone.CRecord.CTask task = new Cgramophone.CRecord.CTask();
        task.Act = true;
        task.FileName = @"C:\Program Files (x86)\RastrWin3\master.exe"; 
        task.Arguments = "-register";
        task.lstOk = new List<int>(1000){1};
        m_gramaphone.PlayTask(task);
        return nRes;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            Log.Information("Heldddlo, Serilog!");

            CInstance c=CInstance.GetCurrent();
            c.SetMsLogger(_logger);
            c.Log(shared.CHlpLog.enErr.INF , "");
            string? str_name   = _configuration.GetValue<string>("r_params:name");
            string? str_q_host = _configuration.GetValue<string>("r_params:q_host");
            int?    n_q_port   = _configuration.GetValue<int>   ("r_params:q_port"); // default 5672
            string? str_q_exch = _configuration.GetValue<string>("r_params:q_exch"); 
            string? str_q_user = _configuration.GetValue<string>("r_params:q_user"); 
            string? str_q_pass = _configuration.GetValue<string>("r_params:q_pass");
            
            _logger.LogWarning($"--------------------------------------------------------------------" ); 
            _logger.LogWarning($"name        : {str_name}");
            _logger.LogWarning($"q_host      : {str_q_host}");
            _logger.LogWarning($"q_port      : {n_q_port}"); 
            _logger.LogWarning($"q_exch      : {str_q_exch}"); 
            _logger.LogWarning($"q_user      : {str_q_user}"); 
            //_logger.LogWarning($" : {str_q_log_pass}");
            _logger.LogWarning($"--------------------------------------------------------------------" ); 

            //CListener.CParams1 
            CListener.CParams par = new CListener.CParams();
            
            par.m_str_host = _configuration.GetValue<string>("r_params:q_host","");
            par.m_n_port   = _configuration.GetValue<int>   ("r_params:q_port",0); // default 5672
            par.m_str_exch = _configuration.GetValue<string>("r_params:q_exch",""); 
            par.m_str_user = _configuration.GetValue<string>("r_params:q_user",""); 
            par.m_str_pass = _configuration.GetValue<string>("r_params:q_pass","");
            par.m_cncl_tkn = stoppingToken;
            
            CListener listener = new CListener();
            
            //CListener.ThreadListen(par, _logger);

            int nRes = 0;
            //nRes = await Task.Factory.StartNew<int>(
                /*
            Task<int> t= Task.Factory.StartNew<int>(
                                                         () => CListener.ThreadListen(par, _logger),
                                                         TaskCreationOptions.LongRunning
                                                        ).ConfigureAwait(true);// false //https://blog.stephencleary.com/2012/07/dont-block-on-async-code.html

*/
            
            Task ttt =  Task.Run(() => {  CListener.ThreadListen(par, _logger,  OnCommand) ; } );
            // ttt.Wait(500,stoppingToken);
            await ttt;
            //Task.WaitAll(ttt,5000,stoppingToken);
            
            //Console.ReadLine();
            //return ;

            
/*
            while (!stoppingToken.IsCancellationRequested)
            {
                string? m_strHostName=null;
                _logger.LogInformation($"{m_strHostName} : Information-> running at: {DateTimeOffset.Now}" );
                _logger.LogWarning($"{m_strHostName} : Warning-> running at: {DateTimeOffset.Now}" );
                _logger.LogError($"{m_strHostName} : Error-> running at: {DateTimeOffset.Now}" );
                await Task.Delay(1000, stoppingToken);
                //Task.WaitAny()
            }
           */ 
        }
        catch( Exception ex)
        {
            _logger.LogError($"catched exeption: {ex.Message}");

            //https://learn.microsoft.com/en-us/dotnet/core/extensions/windows-service 
            // In order for the Windows Service Management system to leverage configured
            // recovery options, we need to terminate the process with a non-zero exit code.
            //Environment.Exit(1); 
        }
    }
}
