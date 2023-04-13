using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Xml.Serialization;
using Microsoft.Win32;
using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;


namespace RastrSrvShare
{
    public class Consts
    { 
        public  const string m_str_error   = "error";
        public  const string m_str_success = "success";
    }

    public class CRabbitParams
    {
        public string m_str_name = "";
        public string m_str_host = "";
        public int    m_n_port = 0; // default 5672
        public string m_str_exch_cmnds = "";
        public string m_str_exch_evnts = "";
        public string m_str_user = "";
        public string m_str_pass = "";
        public CancellationToken m_cncl_tkn;
    }

    public class CSettings
    {
        //private RegistryKey m_rkHKLM_Software_RastrCalc = null;//
        public readonly static string m_strHKLM_Software_RastrCalc = "RastrCalc";
        public readonly static string m_strRastrCalc_PathWrkDir_DefVal = @"D:\RastrCalc\";
        public readonly static string m_SettingsXmlFile = "RastrCalc.xml";
        public readonly static string m_SettingsXmlFile_ROOT = "Settings";
        public readonly static string m_DirTmp = "BarsSmzuTmp";
        public readonly static string m_DirResults = "BarsSmzuResults";
        public readonly static string m_DirLogs = "Logs";
        public readonly static string m_DirMaster = "Master";
        public readonly static string m_WrkPrgName = "1master.exe";
        private CSettings.CModelProps m_ModelProps = new CSettings.CModelProps();

        public string RastrSrvPath
        {
            get
            {
                try
                {
                    RegistryKey m_rkHKLM_Software_RastrCalc = null;
                    using (var hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
                    {
                        m_rkHKLM_Software_RastrCalc = hklm.OpenSubKey("Software", false).OpenSubKey(m_strHKLM_Software_RastrCalc, false);
                        if (m_rkHKLM_Software_RastrCalc == null)
                        {
                            m_rkHKLM_Software_RastrCalc = hklm.OpenSubKey("Software", true).CreateSubKey(m_strHKLM_Software_RastrCalc);
                            m_rkHKLM_Software_RastrCalc.SetValue(m_strHKLM_Software_RastrCalc, m_strRastrCalc_PathWrkDir_DefVal);
                        }
                    }
                    return (string)m_rkHKLM_Software_RastrCalc.GetValue(m_strHKLM_Software_RastrCalc, m_strRastrCalc_PathWrkDir_DefVal);
                }
                catch (Exception ex)
                {
                    return m_strRastrCalc_PathWrkDir_DefVal;
                }
            }
        }

        public string PathPropsXml
        {
            get
            {
                return RastrSrvPath + "\\" + m_SettingsXmlFile;
            }
        }

        public string DirTmp
        {
            get
            {
                return RastrSrvPath + CSettings.m_DirTmp + "\\";
            }
        }

        public string DirBarsSmzuResults
        {
            get
            {
                return RastrSrvPath + m_DirResults + "\\";
            }
        }

        public string SechCalcXmlFName
        {
            get
            {
                return DirBarsSmzuResults + "\\" + DateTime.Now.ToString("yyyy_MM_dd") + ".xml";
            }
        }

        public string SechCalcXmlFName_RootElementName
        {
            get
            {
                return "List_SechCalc";
            }
        }

        public string DirLogs
        {
            get
            {
                return RastrSrvPath + CSettings.m_DirLogs + "\\";
            }
        }

        public class CModelProps //: INotifyPropertyChanged
        {
            //inherited prop - INotifyPropertyChanged
            public event PropertyChangedEventHandler PropertyChanged;
            private string m_strNewServerBind;
            ObservableCollection<CServerBind> m_ListServerBinds = new ObservableCollection<CServerBind>();
            private string m_strPathOnStartLoadMptFile = @"D:\BarsMDP\Results\smzu_mega.mptsmz";

            [XmlIgnore]
            public string strNewServerBind
            {
                get
                {
                    return m_strNewServerBind;
                }
                set
                {
                    m_strNewServerBind = value;
                    OnPropertyChanged("strNewServerBind");
                }
            }

            public string strPathOnStartLoadMptFile
            {
                set
                {
                    m_strPathOnStartLoadMptFile = value;
                }
                get
                {
                    return m_strPathOnStartLoadMptFile;
                }
            }

            public ObservableCollection<CServerBind> ListServerBinds
            {
                set
                {
                    m_ListServerBinds = value;
                }
                get
                {
                    return m_ListServerBinds;
                }
            }

            public class CServerBind : INotifyPropertyChanged
            {
                private bool m_blAct = true;
                private string m_strServerBind;
                private int m_nPower = 1;
                private int m_nMaxProc = 2;

                public event PropertyChangedEventHandler PropertyChanged;

                [XmlElement(Order = 1)]
                public bool blAct
                {
                    get
                    {
                        return m_blAct;
                    }
                    set
                    {
                        m_blAct = value;
                        if (this.PropertyChanged != null)
                        {
                            PropertyChanged(this, new PropertyChangedEventArgs("blAct"));
                        }
                    }
                }

                [XmlElement(Order = 2)]
                public int MaxProc
                {
                    get
                    {
                        return m_nMaxProc;
                    }
                    set
                    {
                        int nTmp = value;
                        if (nTmp < 0)
                        {
                            nTmp = 1;
                        }
                        m_nMaxProc = nTmp;
                        if (this.PropertyChanged != null)
                        {
                            PropertyChanged(this, new PropertyChangedEventArgs("MaxProc"));
                        }
                    }
                }

                [XmlElement(Order = 3)]
                public int Power
                {
                    get
                    {
                        return m_nPower;
                    }

                    set
                    {
                        int nTmp = value;
                        if (value < 0)
                        {
                            nTmp = 1;
                        }
                        m_nPower = nTmp;
                        if (this.PropertyChanged != null)
                        {
                            PropertyChanged(this, new PropertyChangedEventArgs("Power"));
                        }
                    }
                }

                [XmlIgnore]
                public string strServerBind
                {
                    get
                    {
                        return m_strServerBind;
                    }
                    set
                    {
                        m_strServerBind = value;
                        if (this.PropertyChanged != null)
                        {
                            PropertyChanged(this, new PropertyChangedEventArgs("strServerBind"));
                        }
                    }
                }

                [XmlIgnore]
                public Uri UriServerBind
                {
                    get
                    {
                        return new Uri(strServerBind);
                    }
                }

                //https://stackoverflow.com/questions/1379888/how-do-you-serialize-a-string-as-cdata-using-xmlserializer
                [XmlElement("strServerBind", Order = 4)]
                public System.Xml.XmlCDataSection MyStringCDATA
                {
                    get
                    {
                        return new System.Xml.XmlDocument().CreateCDataSection(strServerBind);
                    }
                    set
                    {
                        strServerBind = value.Value;
                    }
                }

                public CServerBind()
                {
                }

                public CServerBind(CServerBind cpy)
                {
                    m_blAct = cpy.m_blAct;
                    m_strServerBind = cpy.m_strServerBind;
                    Power = cpy.Power;
                    m_nMaxProc = cpy.m_nMaxProc;
                }

                public CServerBind(string strNewServerBind, int nPower)
                {
                    m_strServerBind = strNewServerBind;
                    Power = nPower;
                }

            }// public class CServerBind

            public CModelProps()
            {
                //strNewServerBind = "NewServerBind";
                //strNewServerBind = @"net.tcp://desktop-vm10x64.ems.int:1313/RastrSrv/";
                //m_ListServerBinds.Add(new CServerBind("sdfdsfsdfsdf"));
            }

            public CModelProps(CModelProps cpy)
            {
                m_strPathOnStartLoadMptFile = cpy.m_strPathOnStartLoadMptFile;
                m_ListServerBinds = new ObservableCollection<CServerBind>(cpy.m_ListServerBinds);
            }

            void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }

            public CServerBind GetServerBind(string strServerBind)
            {
                CServerBind serverBind = null;
                /*
                                try
                                {
                                    int nIndx = m_ListServerBinds.FindIndex(a => a.strServerBind.Equals(strServerBind));
                                    if (nIndx > -1)
                                    {
                                        serverBind = m_ListServerBinds[nIndx];
                                    }
                                }
                                catch (Exception ex)
                                {
                                    System.Windows.MessageBox.Show($"Перехвачена необработанная исключительная ситуация {ex.Message}", "Ошибка", System.Windows.MessageBoxButton.OK);
                                }
                */
                return serverBind;
            }

            public int CheckBindStr(string strBindStr)
            {
                try
                {/*
                    int nRes = 0;
                    CRastrServHlp RastrServHlp = new CRastrServHlp();
                    nRes = RastrServHlp.Init(new Uri(strBindStr));
                    if (nRes < 0)
                    {
                        //                            System.Windows.MessageBoxResult mbr =
                        //                            System.Windows.MessageBox.Show($"Сервер не отвечает [{strBindStr}] Ошибка [{RastrServHlp.m_strError}]", "Ошибка", System.Windows.MessageBoxButton.OK);
                        return -1;
                    }
                    */
                }
                catch (Exception ex)
                {
                    //System.Windows.MessageBoxResult mbr =
                    //System.Windows.MessageBox.Show($"Сервер вызвал [{strBindStr}] Исключение [{ex.Message}] ", "Ошибка", System.Windows.MessageBoxButton.OK);

                    return -2;
                }
                return 1;
            }

            public int AddNewBind(CServerBind ServerBindNew)
            {
                try
                {
                    if (ServerBindNew.strServerBind.Length < 3)
                    {
                        return -1;
                    }
                    //int h = m_ListServerBinds.Where(a => a.strServerBind.Equals(ServerBindNew.strServerBind));
                   // CServerBind serverBind = m_ListServerBinds.Where(a => a.strServerBind.Equals(ServerBindNew.strServerBind)).FirstOrDefault();
                    //if (serverBind == null)
                    if(false)
                    {
                        /*
                        int nRes = 0;
                        nRes = CheckBindStr(ServerBindNew.strServerBind);
                        if (nRes < 0)
                        {
                            System.Windows.MessageBoxResult mbr =
                            System.Windows.MessageBox.Show($"Сервер не отвечает [{ServerBindNew.strServerBind}] все равно добавить?", "Вопрос", System.Windows.MessageBoxButton.YesNo);
                            if (mbr == MessageBoxResult.No)
                            {
                                return -2;
                            }
                        }
                        */
                        m_ListServerBinds.Add(ServerBindNew);
                        return 1;
                    }
                    else
                    {
                        //MessageBox.Show($"Сервер уже есть [{ServerBindNew.strServerBind}] ", "Ошибка", MessageBoxButton.OK);
                        return -3; // exists
                    }

                    /*
                                              int nIndx = m_ListServerBinds.FindIndex(a => a.strServerBind.Equals(ServerBindNew.strServerBind));
                                              if (nIndx < 0)
                                              {
                                                  int nRes = 0;

                                                  nRes = CheckBindStr(ServerBindNew.strServerBind);
                                                  if (nRes < 0)
                                                  {
                                                      System.Windows.MessageBoxResult mbr =
                                                      System.Windows.MessageBox.Show($"Сервер не отвечает [{ServerBindNew.strServerBind}] все равно добавить?", "Вопрос", System.Windows.MessageBoxButton.YesNo);
                                                      if (mbr == MessageBoxResult.No)
                                                      {
                                                          return -2;
                                                      }
                                                  }
                                                  m_ListServerBinds.Add(ServerBindNew);
                                                  return 1;
                                              }
                                              else
                                              {
                                                  MessageBox.Show($"Сервер уже есть [{ServerBindNew.strServerBind}] ", "Ошибка", MessageBoxButton.OK);
                                                  return -3; // exists
                                              }
                      */
                }
                catch (Exception ex)
                {
                    //                       System.Windows.MessageBox.Show($"Перехвачена необработанная исключительная ситуация {ex.Message}", "Ошибка", System.Windows.MessageBoxButton.OK);
                }
                return -1;
            }

            public int ExterminateBind(CServerBind ServerBindKill)
            {
                /*
                                    int nIndx = m_ListServerBinds.FindIndex(a => a.strServerBind.Equals(ServerBindKill.strServerBind));
                                    if (nIndx < 0)
                                    {
                                        MessageBox.Show($"Привязка не найдена[{ServerBindKill.strServerBind}] ", "Ошибка", System.Windows.MessageBoxButton.OK);
                                        return -1;
                                    }
                                    MessageBoxResult mbr = MessageBox.Show($"Удалить привязку[{ServerBindKill.strServerBind}] ?", "Вопрос", MessageBoxButton.YesNo);
                                    if (mbr == MessageBoxResult.Yes)
                                    {
                                        m_ListServerBinds.RemoveAt(nIndx);
                                    }
                */
                return 1;
            }

        }// class CModelProps

        public int LoadXml(string strPath2XmlFile)
        {
            try
            {
                //strPath2XmlFile = @"D:\RastrCalc22\RastrCalc.xml";
                string strRoot = $"{m_SettingsXmlFile_ROOT}_{Environment.MachineName}";
                XmlSerializer serializer = new XmlSerializer(typeof(CModelProps),
                    new XmlRootAttribute()
                    {
                        ElementName = strRoot
                    });
                using (Stream writer = new FileStream(strPath2XmlFile, FileMode.Open, FileAccess.Read)) // №856573
                {
                    m_ModelProps = (CModelProps)serializer.Deserialize(writer);
                    writer.Close();
                }
            }
            catch (Exception ex)
            {
                return -1;
            }
            return 1;
        }

        public int SaveXml(string strPath2XmlFile)
        {
            try
            {
                string strRoot = $"{m_SettingsXmlFile_ROOT}_{Environment.MachineName}";
                //ElementName = m_SettingsXmlFile_ROOT
                XmlSerializer serializer = new XmlSerializer(typeof(CModelProps),
                    new XmlRootAttribute()
                    {

                        ElementName = strRoot
                    });
                using (Stream writer = new FileStream(strPath2XmlFile, FileMode.Create))
                {
                    serializer.Serialize(writer, m_ModelProps);
                    writer.Close();
                }
            }
            catch (Exception ex)
            {
                return -1;
            }
            return 1;
        }

        public CModelProps ModelProps
        {
            get
            {
                return new CModelProps(m_ModelProps);
                //return m_ModelProps;
            }
            set
            {
                m_ModelProps = new CModelProps(value);
            }
        }

        public CSettings()
        {
        }

        public CSettings(CSettings Settings)
        {
            m_ModelProps = new CModelProps(Settings.ModelProps);
        }

    }

    /*
                DataTable dataTable = new DataTable("ServerStates");
                dataTable.Columns.Add("Act");
                dataTable.Columns.Add("Bind");
                dataTable.Columns.Add("MaxProc");
                dataTable.Columns.Add("CurProc");
                dataTable.Columns.Add("NumFiles");
                dataTable.Columns.Add("NumJobChunkProcs");
                DataTable dataTableSrvErrs = new DataTable("ServerErrs");
                dataTableSrvErrs.Columns.Add("Bind");
                dataTableSrvErrs.Columns.Add("Err");
                foreach (var item in m_Param.Settings.ModelProps.ListServerBinds)
                {
                    bool blFind = false;
                    foreach (var calcNode in m_CalcNodes)
                    {
                        if (calcNode.ServerBind.Equals(item))
                        {
                            blFind = true;
                            dataTable.Rows.Add("", item.strServerBind, calcNode.MaxWrkProc, calcNode.NumWrkPrcs, calcNode.m_dctPntFNames.Count, calcNode.JobChunkProcs.Count);
                            foreach (var jc in calcNode.JobChunkProcs)
                            {
                                foreach (var FileFailedDownload in jc.FilesFailedDownload)
                                {
                                    dataTableSrvErrs.Rows.Add(item.strServerBind, $"ошибка чтения: {FileFailedDownload}");
                                }
                                foreach (var FileFailedCopy in jc.FilesFailedCopy)
                                {
                                    dataTableSrvErrs.Rows.Add(item.strServerBind, $"ошибка чтения: {FileFailedCopy}");
                                }
                            }
                            break;
                        }
                    }
                    if (blFind == false)
                    {
                        dataTable.Rows.Add("откл.", item.strServerBind, item.MaxProc, 0, 0, 0);
                    };
                }
                //JobChunkProcs
                DataSet dataSet = new DataSet("State");
                dataSet.Tables.Add(dataTable);
                dataSet.Tables.Add(dataTableSrvErrs);
     */

    public class CLogHlp
    {

        public enum _enType
        {
            NO = -1,
            INF = 1,
            WRN = 2,
            ERR = 3,
            VIP = 4,
            XML = 5
        };

        public class CLogEntry
        {
            public _enType m_enType = _enType.NO;
            public string m_strLog = "";

            public string StrLog { get { return m_strLog; } set { m_strLog = value; } }

            public CLogEntry()
            {
            }

            public CLogEntry(string strLog, _enType enType = _enType.INF)
            {
                m_enType = enType;
                m_strLog = strLog;
            }
        }

        public static System.Data.DataSet GetStructSrvInf()
        { 
            DataTable dataTable = new DataTable("ServerStates");
            dataTable.Columns.Add("Act");
            dataTable.Columns.Add("Bind");
            dataTable.Columns.Add("MaxProc");
            dataTable.Columns.Add("CurProc");
            dataTable.Columns.Add("NumFiles");
            dataTable.Columns.Add("NumJobChunkProcs");
            DataTable dataTableSrvErrs = new DataTable("ServerErrs");
            dataTableSrvErrs.Columns.Add("Bind");
            dataTableSrvErrs.Columns.Add("Err");
            DataSet dataSet = new DataSet("State");
            dataSet.Tables.Add(dataTable);
            dataSet.Tables.Add(dataTableSrvErrs);
            return dataSet;
        }


    }// class CLogHlp

    //----------------------------------------------------------------------------------------------------------------------------------------

    public class MySink : ILogEventSink
    {
        private readonly IFormatProvider m_formatProvider;
        public IProgress<CLogHlp.CLogEntry> m_progress_log = null;

        public MySink(IFormatProvider formatProvider, IProgress<CLogHlp.CLogEntry> progress_log )
        {
            m_formatProvider = formatProvider;
            m_progress_log   = progress_log;
        }

        CLogHlp._enType GetLogHlpType(LogEventLevel logEventLevel )
        { 
            switch (logEventLevel) 
            {
                case LogEventLevel.Information: return CLogHlp._enType.INF;
                case LogEventLevel.Warning:     return CLogHlp._enType.WRN;
                case LogEventLevel.Error:       return CLogHlp._enType.ERR;
                case LogEventLevel.Fatal:       return CLogHlp._enType.VIP;
                case LogEventLevel.Debug:       return CLogHlp._enType.NO;
                case LogEventLevel.Verbose:     return CLogHlp._enType.NO;
                default:                        return CLogHlp._enType.ERR;
            }
        }

        public void Emit(LogEvent logEvent)
        {
            if(m_progress_log!=null)
            { 
                var message = logEvent.RenderMessage(m_formatProvider);
                CLogHlp.CLogEntry logEntry = new CLogHlp.CLogEntry();
                logEntry.m_enType = GetLogHlpType(logEvent.Level);
                logEntry.m_strLog = message;
                m_progress_log.Report(logEntry);
            }
        }

    }//class MySink 
    
    //----------------------------------------------------------------------------------------------------------------------------------------

    public static class MySinkExtensions
    {
        public static LoggerConfiguration MySink(
                  this LoggerSinkConfiguration loggerConfiguration, 
                  IFormatProvider formatProvider, 
                  IProgress<CLogHlp.CLogEntry> progress_log )
        {
            return loggerConfiguration.Sink(new MySink(formatProvider, progress_log));
        }

    }//static class MySinkExtensions

    //----------------------------------------------------------------------------------------------------------------------------------------

    public static class Helpers
    {
        public static double ParseToDouble(string value)
        {
            double result = Double.NaN;
            value = value.Trim();
            if (!double.TryParse(value, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.GetCultureInfo("ru-RU"), out result))
            {
                if (!double.TryParse(value, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.GetCultureInfo("en-US"), out result))
                {
                    return Double.NaN;
                }
            }
            return result;
        }
    }

    //----------------------------------------------------------------------------------------------------------------------------------------

    public class CParam
    {
        [XmlIgnore]
        public IProgress<CLogHlp.CLogEntry> m_IProgressLog = null;

        [XmlIgnore]
        public CancellationTokenSource m_CancellationTokeSource = null;

        //----------------------------------------------------------------------------------------------------------------------------------------

        public class CJobChunk
        {
            public int SechNum { get; set; }
            [XmlIgnore]
            public string SechName { get; set; }
            public int PntNum { get; set; }
            public bool Calc { get; set; }
            [XmlIgnore]
            public bool DisabledInMpt { get; set; }
            public string strPntFName { get; set; }
            public double MdpValWithPa { get; set; }
            public int MdpKod { get; set; } = -1;
            public string MdpDescr { get; set; } = "";

            public CJobChunk()
            {
                Calc = true;
                DisabledInMpt = false;
            }

            public CJobChunk(CJobChunk cpy)
            {
                SechNum = cpy.SechNum;
                SechName = cpy.SechName;
                PntNum = cpy.PntNum;
                Calc = cpy.Calc;
                DisabledInMpt = cpy.DisabledInMpt;
                strPntFName = cpy.strPntFName;
                MdpValWithPa = MdpValWithPa;
                MdpKod = MdpKod;
                MdpDescr = MdpDescr;
            }

            public string GetDataFName()
            {
                string strFName = "";

                strFName = System.IO.Path.GetFileName(strPntFName);
                return strFName;
            }

            public string Path2ResultsXml(string strPath2TmpDir)
            {
                string strPath2ResultsXml = "";
                string FName = "";

                FName = GetDataFName();
                strPath2ResultsXml = $"{strPath2TmpDir}\\{FName}_[{PntNum}]_[{SechNum}]_calc_log.xml";

                return strPath2ResultsXml;
            }

        }

        //----------------------------------------------------------------------------------------------------------------------------------------

        public class CJobChunks
        {
            public List<CJobChunk> m_lstJobChunks = new List<CJobChunk>();
            private Dictionary<int, string> m_dctSechNum2Name = new Dictionary<int, string>();

            public CJobChunks()
            {
            }

            public CJobChunk? PopJobChunk()
            {
                if(m_lstJobChunks.Count==0)
                { 
                    return null;
                }
                CJobChunk job_chunk = m_lstJobChunks[0];
                m_lstJobChunks.Remove(job_chunk);
                return job_chunk;
            }

            public CJobChunks(CJobChunks cpy)
            {
                m_lstJobChunks = new List<CJobChunk>(cpy.m_lstJobChunks);
                m_dctSechNum2Name = new Dictionary<int, string>(cpy.m_dctSechNum2Name);
            }

            static public int SerializeListJobChunksToXmlFile(List<CJobChunk> lstCJobChunks, string strPathToXmlFile, string strRootElementName)
            {
                try
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<CJobChunk>),
                        new XmlRootAttribute()
                        {
                            ElementName = strRootElementName
                        });
                    using (Stream writer = new FileStream(strPathToXmlFile, FileMode.Create))
                    {
                        serializer.Serialize(writer, lstCJobChunks);
                        writer.Close();
                    }
                }
                catch (Exception)
                {
                    return -1;
                }
                //DeSerializefromXmlFile(strPathToXmlFile, strRootElementName);
                return 1;
            }

            static public int DeSerializefromXmlFile(ref List<CJobChunk> lstCJobChunks, string strPathToXmlFile, string strRootElementName)
            {
                try
                {
                    lstCJobChunks = new List<CJobChunk>();
                    XmlSerializer serializer = new XmlSerializer(typeof(List<CJobChunk>),
                        new XmlRootAttribute()
                        {
                            ElementName = strRootElementName
                        });
                    using (Stream writer = new FileStream(strPathToXmlFile, FileMode.Open))
                    {
                        lstCJobChunks = (List<CJobChunk>)serializer.Deserialize(writer);
                        writer.Close();
                    }
                }
                catch (Exception ex)
                {
                    return -1;
                }
                return 1;
            }

            public int UpdateState(List<CJobChunk> lstCJobChunk)
            {
                if (lstCJobChunk == null)
                {
                    return 1;
                }
                foreach (CJobChunk jobChunk in lstCJobChunk)
                {
                    CParam.CJobChunk jobChunk2 = null;
                    jobChunk2 = m_lstJobChunks.Find(JOBchunk => (JOBchunk.PntNum.Equals(jobChunk.PntNum) && JOBchunk.SechNum.Equals(jobChunk.SechNum)));
                    if (jobChunk2 != null)
                    {
                        if (jobChunk2.DisabledInMpt == false)
                        {
                            if (jobChunk2.Calc != jobChunk.Calc)
                            {
                                Debug.Print($"Pnt[{jobChunk.PntNum:00.}] Sech[{jobChunk.SechNum:000.}] new STATE[{jobChunk.Calc}]");
                                jobChunk2.Calc = jobChunk.Calc;
                            }
                        }
                    }
                    else
                    {
                        Debug.Print($"Not found list Pnt[{jobChunk.PntNum:00.}] Sech[{jobChunk.SechNum:000.}] to SET[{jobChunk.Calc}]");
                    }
                }
                return 1;
            }

            public List<int> PntNums2Calc
            {
                get
                {
                    List<int> vsOut = new List<int>();
                    if (m_lstJobChunks != null)
                    {
                        foreach (var JobChunk in m_lstJobChunks)
                        {
                            if (vsOut.FindIndex(PntNum => JobChunk.PntNum.Equals(PntNum)) < 0)
                            {
                                if (JobChunk.Calc == true)
                                {
                                    vsOut.Add(JobChunk.PntNum);
                                }
                            };
                        }
                    }
                    return vsOut;
                }
            }

            public List<CJobChunk> GetCJobChunksForPnt2Calc(int nPntNum)
            {
                List<CJobChunk> lstJC = new List<CJobChunk>();

                if (m_lstJobChunks != null)
                {
                    foreach (var JobChunk in m_lstJobChunks)
                    {
                        if (JobChunk.PntNum == nPntNum)
                        {
                            if (JobChunk.Calc == true)
                            {
                                lstJC.Add(new CJobChunk(JobChunk));
                            }
                        }
                    }
                }
                return lstJC;
            }

            public void Clear()
            {
                m_lstJobChunks.Clear();
            }

            public int SetSechName(int nSechNum, string strName)
            {
                if (m_dctSechNum2Name.ContainsKey(nSechNum) == true)
                {
                    return -1;
                }
                m_dctSechNum2Name.Add(nSechNum, strName);
                return 1;
            }

            public string GetSechName(int nSechNum)
            {
                if (m_dctSechNum2Name.ContainsKey(nSechNum) == false)
                {
                    return "<>";
                }
                return m_dctSechNum2Name[nSechNum];
            }

            public List<int> SechNums
            {
                get
                {
                    List<int> vsOut = new List<int>();
                    if (m_lstJobChunks != null)
                    {
                        foreach (var JobChunk in m_lstJobChunks)
                        {
                            if (vsOut.FindIndex(ns => JobChunk.SechNum.Equals(ns)) < 0)
                            {
                                vsOut.Add(JobChunk.SechNum);
                            };
                        }
                    }
                    return vsOut;
                }
            }

            public int NumChuncksToCalc
            {
                get
                {
                    int nNum = 0;

                    if (m_lstJobChunks != null)
                    {
                        foreach (var Jobchunk in m_lstJobChunks)
                        {
                            if (Jobchunk.Calc == true)
                            {
                                nNum++;
                            }
                        }
                    }
                    return nNum;
                }
            }

            public int MaxPntNum
            {
                get
                {
                    int nMaxPntNum = -100;
                    if (m_lstJobChunks != null)
                    {
                        foreach (var Jobchunk in m_lstJobChunks)
                        {
                            if (nMaxPntNum < Jobchunk.PntNum)
                            {
                                nMaxPntNum = Jobchunk.PntNum;
                            }
                        }
                    }
                    return nMaxPntNum;
                }
            }

            public int MinPntNum
            {
                get
                {
                    int nMinPntNum = -101;
                    if (m_lstJobChunks != null)
                    {
                        if (m_lstJobChunks.Count > 0)
                        {
                            nMinPntNum = m_lstJobChunks[0].PntNum;
                            foreach (var Jobchunk in m_lstJobChunks)
                            {
                                if (nMinPntNum > Jobchunk.PntNum)
                                {
                                    nMinPntNum = Jobchunk.PntNum;
                                }
                            }
                        }
                    }
                    return nMinPntNum;
                }
            }
        }//class CJobChunks

        //----------------------------------------------------------------------------------------------------------------------------------------

        private CSettings m_Settings = null;
        public string strPath2MptSmz = "";
        //private string strPath2Tmp = @"C:\TMP\RastrSrv\BarsSmzTmp\";
        private string strPath2Tmp = @"";
        public string strPntFName = @"Pnt";
        public int nPntBeg = -13;
        public int nPntEnd = -13;
        private string m_strCalcGuid;
        public static readonly string FtpDirCalcs = "CALCS";
        public static readonly string LocDirCalcs = "CALCS2";
        public int ProcTimeOutMs { get; set; } = 600_000;

        [XmlIgnore]
        private List<CCalcHost> m_CalcHosts = new List<CCalcHost>();

        CJobChunks m_JobChunks = null;

        public string GetMptSmzName { get{ return System.IO.Path.GetFileName(strPath2MptSmz); } }

        public CSettings Settings
        {
            get
            {
                return new CSettings(m_Settings);
            }
            set
            {
                m_Settings = new CSettings(value);
            }
        }

        public CJobChunks JobChunks
        {
            get
            {
                return m_JobChunks;
            }
            set
            {
                m_JobChunks = new CJobChunks(value);
            }
        }

        public CParam.CJobChunks GetJobChunksForProceed 
        { 
            get 
            { 
                CParam.CJobChunks JobChunksReadyForProceed = new CParam.CJobChunks();
                //List<CParam.CJobChunk> lstJobChunksForPnt = null;
                List<int> lstUnProccessedPnts = JobChunks.PntNums2Calc;

                // набивка всех задач по всем точкам в один расчет// внести в m_Param.JobChunks
                int i = 0;
                for (i = 0; i < 500; i++)
                {
                    int nCurrentPntNum = -1;
                    if (lstUnProccessedPnts.Count > 0)
                    {
                        nCurrentPntNum = lstUnProccessedPnts[0];
                        lstUnProccessedPnts.RemoveAt(0);
                        strPntFName = strPath2MptSmz;
                        List<CParam.CJobChunk> lstJobChunksForPnt = JobChunks.GetCJobChunksForPnt2Calc(nCurrentPntNum);
                        foreach (var JobChunkForPnt in lstJobChunksForPnt)
                        {
                            JobChunkForPnt.strPntFName = strPntFName;
                        }
                        JobChunksReadyForProceed.m_lstJobChunks.InsertRange(JobChunksReadyForProceed.m_lstJobChunks.Count, lstJobChunksForPnt);
                    }//if (lstUnProccessedPnts.Count > 0)
                    else 
                    {
                        break;
                    }
                }//for (i = 0; i < 500; i++)

                return JobChunksReadyForProceed; 
            } 
        }

        /*
CParam.CJobChunks JobChunksReadyForProceed = new CParam.CJobChunks();
                List<CParam.CJobChunk> lstJobChunksForPnt = null;

                try
                {
                    AddLog($"Загружаем без шаблона " + m_Param.strPath2MptSmz);
                    nRes = m_RastrHlp.Load(0, m_Param.strPath2MptSmz, "");
                    if (nRes < 0)
                    {
                        AddLog($"Ошибка загрузки", CLogHlp._enType.ERR);
                        return -1;
                    }
                    lstUnProccessedPnts = m_Param.JobChunks.PntNums2Calc;

                    // набивка всех задач по всем точкам в один расчет// внести в m_Param.JobChunks
                    for (i = 0; i < 500; i++)
                    {
                        nCurrentPntNum = -1;
                        if (lstUnProccessedPnts.Count > 0)
                        {
                            nCurrentPntNum = lstUnProccessedPnts[0];
                            lstUnProccessedPnts.RemoveAt(0);
                            strPntFName = m_Param.strPath2MptSmz;
                            lstJobChunksForPnt = m_Param.JobChunks.GetCJobChunksForPnt2Calc(nCurrentPntNum);
                            foreach (var JobChunkForPnt in lstJobChunksForPnt)
                            {
                                JobChunkForPnt.strPntFName = strPntFName;
                            }
                            JobChunksReadyForProceed.m_lstJobChunks.InsertRange(JobChunksReadyForProceed.m_lstJobChunks.Count, lstJobChunksForPnt);
                        }//if (lstUnProccessedPnts.Count > 0)
                        else 
                        {
                            break;
                        }
                    }//for (i = 0; i < 500; i++)
         */

        public string CalcGuid
        {
            get
            {
                return m_strCalcGuid;
            }
            set
            {
                m_strCalcGuid = value;
            }
        }

        public string DirSmzuTmp
        {
            get
            {
                return strPath2Tmp;
            }
            set
            {
                strPath2Tmp = value;
            }
        }

        public string DirBarsSmzuResults
        {
            get
            {
                return m_Settings.DirBarsSmzuResults + CalcGuid + "\\";
            }
        }

        public List<CCalcHost> CalcHosts
        {
            get
            {
                return m_CalcHosts;
            }
            set
            {
                m_CalcHosts = value;
            }
        }

        public class CCalcHost
        {
            Uri m_uri = null;

            [XmlIgnore]
            public Uri URI
            {
                get { return m_uri; }
                set { m_uri = value; }
            }
        };
    }; //class Param

    //----------------------------------------------------------------------------------------------------------------------------------------

    public class CState
    {
        public int    m_n_slots_max  { get; set;} = -1;
        public int    m_n_slots_busy { get; set;} =  0;

        public class CService
        {
            public string       m_str_name                { get; set;} = "NO_NAME";
            public DateTime     m_dt_last_seen            { get; set;} 
            public int          m_n_alarm_border_millisec { get; set;} = 15000;
            public bool         m_bl_alarm_fixed          { get; set;} = false;
            public List<string> m_lst_errors              { get; set;} = new List<string>();
        }

        public Dictionary<string, CService> m_services {get;set;} = new Dictionary<string, CService>();

        public void CheckServicesState()
        {
            /*
            DateTime dtNow = DateTime.Now;
            foreach(CService service in m_services.Values )
            {
                if( (dtNow - service.m_dt_last_seen).TotalMilliseconds > service.m_n_alarm_border_millisec )
                {
                    if(service.m_bl_alarm_fixed == false)
                    {
                        service.m_bl_alarm_fixed = true;
                        service.m_lst_errors.Add($"disappearence fixed at [{dtNow}]");
                        Log.Information($"disappeared service [{service.m_str_name}] for the [{(dtNow - service.m_dt_last_seen).TotalMilliseconds}] sec");
                    }
                }
                if( (dtNow - service.m_dt_last_seen).TotalMilliseconds < service.m_n_alarm_border_millisec )
                {
                    if(service.m_bl_alarm_fixed == true)
                    {
                        service.m_bl_alarm_fixed = false;
                        service.m_lst_errors.Add($"appearance fixed at [{dtNow}]");
                        Log.Information($"appeared service [{service.m_str_name}] after [{(dtNow - service.m_dt_last_seen).TotalMilliseconds}] sec");
                    }
                }
            }
            */
        }
    } //class CState

    //----------------------------------------------------------------------------------------------------------------------------------------

    public class CMain
    {
        public void tst()
        {
            Console.WriteLine("hello");
        }
    }

    //----------------------------------------------------------------------------------------------------------------------------------------
}
