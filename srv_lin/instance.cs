class CInstance
{
    private static CInstance? m_instance;
    private static object m_obj_sync = new Object();
  
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
}