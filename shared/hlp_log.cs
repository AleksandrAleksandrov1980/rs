namespace shared;
public class CHlpLog
{
    public static readonly string str_log_time_fmt = "yyyy:MM:dd-HH:mm:ss";
    
    //Microsoft.Extensions.Logging.LogLevel d;
    public enum enErr
    {
        NO = 0,
        INF = 1,
        WRN = 2,
        ERR = 3,
    }

    public class CLogEntry
    {
        enErr err = enErr.NO;
        string txt = "";
    }
}