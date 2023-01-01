using System.Text.Json;
using Serilog;

public class Cgramophone
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
}