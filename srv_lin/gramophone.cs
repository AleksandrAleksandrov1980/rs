using Serilog;
using System.Text.Json;
using System.Diagnostics;

public class CGramophone
{
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
            public string Name { get; set; }
            public string FileName { get; set; }
            public string Arguments { get; set; }
            public List<int> lstOk { get; set; }
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
            Log.Error($"Exception : {ex.Message}");
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
                return 1;
            }
            ProcessStartInfo psi = new ProcessStartInfo();
            //bool lblEx = System.IO.File.Exists("C:\\Program Files (x86)\\RastrWin3\\master.exe");
            //psi.FileName = System.IO.Path.GetFileName(fullPath);
            psi.FileName = task.FileName;
            psi.Arguments = task.Arguments;
            Console.Write($"Start [{psi.FileName}] with arguments [{psi.Arguments}]\n");
            //LogAdd(dllcom.CHlpLog.enErr.INF, $"Start [{psi.FileName}] with arguments [{psi.Arguments}]");

            Process? process = Process.Start(psi);
            if(process == null)
            {
                return -2;
            }
            //LogSendStartedProcccessId(process.Id);
            //process.Id;
            bool blRes = process.WaitForExit(task.TimeOutSecs * 1000);
            //LogSendFinishedProcccessId(process.Id);
            if (blRes != true)
            {
                //LogAdd(dllcom.CHlpLog.enErr.ERR, $"TimeOut Pid {process.Id}");
                return -3;// timeout
            }
            int nExitCode = process.ExitCode;
            //LogAdd(dllcom.CHlpLog.enErr.INF, $"ExitCode Pid {process.Id} : nExitCode {nExitCode}");
            return nExitCode;
        }
        catch (Exception ex)
        {
            Console.Write($"Error {ex.Message}\n");
            //LogAdd(dllcom.CHlpLog.enErr.ERR, $"Start excp-> {ex.Message}");
            return -1;
        }
        return 1;
    }

    int Play(CancellationToken cncl_tkn, string str_path_record)
    {
         int nCounter = 0 ;
        for(;;nCounter++)
        {
            string json = System.IO.File.ReadAllText(str_path_record);
            CRecord? record = JsonSerializer.Deserialize<CRecord>(json);
            if(record != null)
            {
                foreach( CRecord.CTask task in record.lstJTasks )
                {
                    int nRes =0 ;
                    nRes = PlayTask(task);
                }
            }
        }
        return 1;
    }

    public static int ThreadPlay( CancellationToken cncl_tkn, string str_path_record )
    {
        CGramophone gramophone = new CGramophone();
        gramophone.Play( cncl_tkn,  str_path_record);
        

       
        return 1;
    }
}