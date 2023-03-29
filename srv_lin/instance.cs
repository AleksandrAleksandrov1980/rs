
namespace srv_lin;

class CInstance
{
    private static CInstance? m_instance;
    private static object m_obj_sync = new Object();
    private ILogger? m_logger;    
  
    public static CInstance GetCurrent()
    {
        if (m_instance == null)
        {
            lock (m_obj_sync)
            {
                if (m_instance == null)
                    m_instance = new CInstance();
            }
        }
        return m_instance;
    }

    public void SetMsLogger(ILogger logger_in)
    {
        m_logger = logger_in;
    }

    //public void Log(shared.CHlpLog.enErr err, string strMsg )
    public void Log(int n, string strMsg )
    {
        lock(m_obj_sync)
        {
            int n_mt_id = Thread.CurrentThread.ManagedThreadId;
            //logEntry.strTime = DateTime.Now.ToString("yyyy:MM:dd-HH:mm:ss");
//            string str_time = DateTime.Now.ToString( shared.CHlpLog.str_log_time_fmt );
            //m_logger.Log(LogLevel.Information
        }
    }
}