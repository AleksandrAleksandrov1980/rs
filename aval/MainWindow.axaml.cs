using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Microsoft.Extensions.Configuration;
using System;
using Microsoft.Extensions.Configuration;
using RastrSrvShare;
using Serilog;
using static RastrSrvShare.Ccommunicator;
using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Threading;
using System.Linq;
using System.Collections;

namespace aval;
public partial class MainWindow : Window
{
    private List<CmndStr> m_lstCmndStr = new List<CmndStr>();
    private Dictionary<string, ServerData> m_dct_Name_ServerData = new Dictionary<string, ServerData>();
    ComboBox m_cb_Cmnds;
    TextBox m_tb_Pars;
    TextBox m_tb_From;
    TextBox m_tb_To;
    TextBox m_tb_Role;
    TextBox m_tb_Log;
    TextBox m_tbSrvs;
    RastrSrvShare.Ccommunicator m_communicator = new RastrSrvShare.Ccommunicator();

    public class CmndStr
    {
        public string Name { get; set; } = "";
        public enCommands Cmnd{ get; set; } = enCommands.ERROR;
    }

    public class ServerData
    { 
        public string Name { get; set; } = "";
        public string Role { get; set; } = "";
        public override string ToString()
        {
            return $"{Name} : {Role}";
        }  
    }

    public int OnEvent( Ccommunicator.Evnt evnt )
    {
        try
        {
            Dispatcher.UIThread.InvokeAsync(new Action(() => { Log($"evnt:{evnt}"); }));
            if( m_dct_Name_ServerData.ContainsKey(evnt.from)== false)
            { 
                ServerData server_data = new ServerData();
                server_data.Name = evnt.from;
                server_data.Role = evnt.from_role;
                m_dct_Name_ServerData.Add(evnt.from, server_data);
                Dispatcher.UIThread.InvokeAsync(new Action(() => { PopulateServers(); }));
            }
        }   
        catch (Exception ex)
        { 
            Dispatcher.UIThread.InvokeAsync(new Action(() => { Log($"[exception {ex}]"); }));
        }
        return 1;
    }

    public MainWindow()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
        m_tb_Pars = this.FindControl<TextBox>("tbPars");
        m_tb_From = this.FindControl<TextBox>("tbFrom");
        m_tb_To = this.FindControl<TextBox>("tbTo");
        m_tb_Role = this.FindControl<TextBox>("tbRole");
        m_tb_Log = this.FindControl<TextBox>("Log1");
        m_cb_Cmnds = this.Find<ComboBox>("cbCmnds");
        m_tbSrvs = this.FindControl<TextBox>("tbSrvs");
        var commands = Enum.GetValues(typeof(enCommands));
        foreach(var x in commands )
        {
            CmndStr note = new CmndStr();
            note.Name = x.ToString();
            note.Cmnd = (enCommands)x;
            m_lstCmndStr.Add(note);
        }
        m_cb_Cmnds.Items = m_lstCmndStr;
        m_cb_Cmnds.SelectedIndex = 0;

        try
        { 
            RastrSrvShare.CRabbitParams par = new RastrSrvShare.CRabbitParams();
            try
            { 
                IConfiguration config = new ConfigurationBuilder()
                    .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();                    
                IConfigurationSection con_sec_params = config.GetSection("r_params");
                par.m_str_host          = con_sec_params.GetRequiredSection("q_host").Value;
                par.m_n_port            = int.Parse(con_sec_params.GetRequiredSection("q_port").Value);
                par.m_str_exch_cmnds    = con_sec_params.GetRequiredSection("q_exch_commands").Value;
                par.m_str_exch_evnts    = con_sec_params.GetRequiredSection("q_exch_events").Value;
                par.m_str_user          = con_sec_params.GetRequiredSection("q_user").Value;
                par.m_str_pass          = con_sec_params.GetRequiredSection("q_pass").Value;
                IConfigurationSection con_sec_ftp = con_sec_params.GetSection("ftp");
                /*ftp_.m_str_ftp_host     = con_sec_ftp.GetRequiredSection("host").Value;
                ftp_.m_str_ftp_user     = con_sec_ftp.GetRequiredSection("user").Value;
                ftp_.m_str_ftp_pass     = con_sec_ftp.GetRequiredSection("pass").Value;
                ftp_.m_n_ftp_port       = int.Parse(con_sec_ftp.GetRequiredSection("port").Value);*/
            }
            catch(Exception ex)
            { 
                Log($"Can't read 'appsettings.json' in current directory exception[{ex}]");
                return;
            }
            par.m_str_name = "sender"; //m_configuration.GetValue<string>("r_params:name","");
            RastrSrvShare.CSigner signer_prv = new RastrSrvShare.CSigner();
            string str_path_exe_dir = file_dir_hlp.GetPathExeDir();
            string str_path_prv_key = str_path_exe_dir+"/"+RastrSrvShare.CSigner.str_fname_prv_xml;
            //Log($"����� ��������� ���� ����������� [{str_path_prv_key}]");
            int nRes = signer_prv.ReadKey(str_path_prv_key);
            if(nRes<0)
            { 
                Log($"��������� ���� �� ��������. ����[{str_path_prv_key}]");
                return ;
            }
            m_communicator.Init(par, signer_prv); 
            Task taskConsumeEvents = Task.Run( ()=>{ m_communicator.ConsumeEvnts(OnEvent); });
        }
        catch(Exception ex)
        { 
            Log($"MainWindow() exception: {ex}");
        }
    }

    public void OnClickCommand1()
    {
	    Console.WriteLine(""); 
    }

    public void PopulateServers()
    { 
        var asString = string.Join(Environment.NewLine, m_dct_Name_ServerData.Values);
        m_tbSrvs.Text= asString;
    }

    public void Log(string str_msg)
    { 
        if(m_tb_Log!=null)
        { 
            m_tb_Log.Text += str_msg + "\r\n";
        }
    }

    public void on_btn_click_clear(object sender, RoutedEventArgs e)
    { 
        m_tb_Log.Text = "";
    }

    public void on_btn_click_send(object sender, RoutedEventArgs e)
    {
        try
        { 
            //string str_cmnd     = m_lstCmndStr[m_cb_Cmnds.SelectedIndex].Name;
            RastrSrvShare.Ccommunicator.enCommands en_command = m_lstCmndStr[m_cb_Cmnds.SelectedIndex].Cmnd;
            string str_to     = m_tb_To.Text;
            string str_from   = m_tb_From.Text;
            string str_role   = m_tb_Role.Text;
            string[] str_pars = m_tb_Pars.Text.Split(new string[] { Environment.NewLine },  StringSplitOptions.None);
            RastrSrvShare.Ccommunicator.Command cmnd_pub = m_communicator. PublishCmnd( en_command, str_to, str_role, str_pars );
            Log($"send_command : {cmnd_pub}\n");
        }
        catch (Exception ex) 
        {
            Log($"send exception: {ex}");
        }
    }

    public void on_btn_click_tst_astra(object sender, RoutedEventArgs e)
    {
         try
        { 
            //c++ /home/ustas/projects/c2
            Log($"tst_astra_beg\n");
            CRwrapper rw = new CRwrapper();
            int nRes = rw.call_test();
            Log($"tst_astra_end. get[{nRes}]\n");
        }
        catch (Exception ex) 
        {
            Log($"catch exception: {ex}");
        }

    }

}