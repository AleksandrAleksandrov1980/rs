namespace shared;
public class CHlpLog
{
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