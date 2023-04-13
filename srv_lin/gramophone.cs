using Serilog;
using System.Text.Json;
using System.Diagnostics;
using srv_lin;

namespace srv_lin;

public class CGramophone
{
    //public  Ccommunicator Communicator { get; set; } 
    public RastrSrvShare.Ccommunicator? Communicator { get; set; } 

    public class CRecord
    {
        public bool Act { get; set; } = true;
        public string Name { get; set; } = "no";
        public string strPub { get; set; } = "no";
        public string strSub { get; set; } = "no";
        public List<CTask> lstJTasks { get; set; } = new List<CTask>();

        public CRecord()
        {
        }

        public CRecord(CRecord record)
        {
            Act = record.Act;
            Name = record.Name;
            lstJTasks = new List<CTask>(record.lstJTasks.Count);
            foreach (CTask task in record.lstJTasks)
            {
                lstJTasks.Add(new CTask(task));
            }
        }

        public class CTask
        {
            public bool Act { get; set; } = true;
            public string Name { get; set; } = "";
            public string FileName { get; set; } = "";
            public string Arguments { get; set; } = "";
            public List<int> lstOk { get; set; } = new List<int>();
            public int TimeOutSecs { get; set; } = 1000;

            public CTask()
            {
            }

            public CTask(CTask task)
            {
                Act = task.Act;
                Name = task.Name;
                FileName = task.FileName;
                Arguments = task.Arguments;
                this.lstOk = new List<int>(task.lstOk);
                TimeOutSecs = task.TimeOutSecs;
            }
        }
    }

    private CRecord? m_Record;

    public int DeSerializeRecordFromJsonFile(string strPathFile)
    {
        try
        {
            string json = System.IO.File.ReadAllText(strPathFile);
            m_Record = JsonSerializer.Deserialize<CRecord>(json);
        }
        catch (Exception ex)
        {
            Log.Error($"Exception : {ex}");
            return -1;
        }
        return 1;
    }

    public int PlayTask(CRecord.CTask task)
    {
        try
        {
            if (task.Act == false)
            {
                //LogAdd(dllcom.CHlpLog.enErr.INF, $"Task is off [{task.Name}]");
                Log.Information($"Task is off [{task.Name}]");
                return 1;
            }
            ProcessStartInfo psi = new ProcessStartInfo();
            //bool lblEx = System.IO.File.Exists("C:\\Program Files (x86)\\RastrWin3\\master.exe");
            //psi.FileName = System.IO.Path.GetFileName(fullPath);
            psi.FileName = task.FileName;
            psi.Arguments = task.Arguments;
            //Console.Write($"Start [{psi.FileName}] with arguments [{psi.Arguments}]\n");
            //LogAdd(dllcom.CHlpLog.enErr.INF, $"Start [{psi.FileName}] with arguments [{psi.Arguments}]");
            Log.Information($"Start [{psi.FileName}] with arguments [{psi.Arguments}]");
            //Log.Information($"Start [{psi.FileName}] with arguments [{psi.Arguments}]");
            Communicator.PublishEvnt( RastrSrvShare.Ccommunicator.enEvents.START, new string[]{psi.FileName,psi.Arguments} );
            Process? process = Process.Start(psi);
            if(process == null)
            {
                Communicator.PublishEvnt( RastrSrvShare.Ccommunicator.enEvents.ERROR, new string[]{"can't start process"} );
                Log.Error($"cant start [{psi.FileName}] with arguments [{psi.Arguments}]");
                return -2;
            }
            //LogSendStartedProcccessId(process.Id);
            //process.Id;
            bool blRes = process.WaitForExit(task.TimeOutSecs * 1000);
            //LogSendFinishedProcccessId(process.Id);
            if (blRes != true)
            {
                //LogAdd(dllcom.CHlpLog.enErr.ERR, $"TimeOut Pid {process.Id}");
                Communicator.PublishEvnt( RastrSrvShare.Ccommunicator.enEvents.ERROR, new string[]{ $"TimeOut Pid {process.Id}" } );
                Log.Error($"TimeOut Pid {process.Id}");
                return -3;// timeout
            }
            int nExitCode = process.ExitCode;
            //LogAdd(dllcom.CHlpLog.enErr.INF, $"ExitCode Pid {process.Id} : nExitCode {nExitCode}");
            Log.Information($"ExitCode Pid {process.Id} : nExitCode {nExitCode}");
            Communicator.PublishEvnt( RastrSrvShare.Ccommunicator.enEvents.FINISH, new string[]{ $"ExitCode Pid {process.Id} : nExitCode {nExitCode}" } );
            return nExitCode;
        }
        catch(Exception ex)
        {
            //Console.Write($"Error {ex.Message}\n");
            Log.Error($"\tPlay.Exception {ex}");
            Communicator.PublishEvnt( RastrSrvShare.Ccommunicator.enEvents.ERROR, new string[]{ $"Exception {ex.Message}" } );
            //LogAdd(dllcom.CHlpLog.enErr.ERR, $"Start excp-> {ex.Message}");
            return -1;
        }
    }

    int Play(CancellationToken cncl_tkn, string str_path_record)
    {
        int nCounter = 0 ;
        try
        {
            for(;;nCounter++)
            {
                Log.Information($"cycle [{nCounter}]----------------------------------------------");
                Log.Information($"read record -> {str_path_record}");
                CRecord? record = null;
                try
                {
                    string json = System.IO.File.ReadAllText(str_path_record);
                    record = JsonSerializer.Deserialize<CRecord>(json);
                }
                catch(Exception ex)
                {
                    Log.Error($"can't read, message-> {ex}");
                }               
                if(record != null)
                {
                    foreach( CRecord.CTask task in record.lstJTasks )
                    {
                        int nRes = 0;
                        if(cncl_tkn.IsCancellationRequested == true)
                        {
                            Log.Warning("Cancell requested!");
                            break;
                        }
                        nRes = PlayTask(task);
                        if(nRes < 0)
                        {
                            Log.Warning($"finished task [{task.Name}]  with error {nRes}");
                        }
                    }
                }
                //Thread.Sleep(1000);
                Task.Delay(1000, cncl_tkn).Wait();// throws System.AggregateException when canceled
            }
        }
        catch(System.AggregateException sae)
        {
            Log.Warning($"canceled in wait-> {sae}");
        }
        catch(System.OperationCanceledException oce)
        {
            Log.Warning($"canceled-> {oce}");
        }
        catch(Exception ex)
        {
            Log.Error($"Exception-> {ex}");
        }
        return 1;
    }

    public static int ThreadPlay( CancellationToken cncl_tkn, string str_path_record,  RastrSrvShare.Ccommunicator? communicator )
    {
        if(communicator==null)
        {
            Log.Error("no communcator suplied");
            return -1000;
        }
        CGramophone gramophone = new CGramophone();
        gramophone.Communicator = communicator;
        return gramophone.Play( cncl_tkn,  str_path_record );
    }
}