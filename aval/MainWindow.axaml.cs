using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Microsoft.Extensions.Configuration;
using System;

using Microsoft.Extensions.Configuration;
using RastrSrvShare;
using Serilog;
using static RastrSrvShare.Ccommunicator;

namespace aval;

public partial class MainWindow : Window
{
    TextBox m_tb_Log; 

    public MainWindow()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
        m_tb_Log = this.FindControl<TextBox>("Log1");

    }

    public void OnClickCommand1()
    {
	    Console.WriteLine(""); 
    }

    public void Log(string str_msg)
    { 
        if(m_tb_Log!=null)
        { 
            m_tb_Log.Text += str_msg + "\r\n";
        }
    }

    public void on_btn_click_send(object sender, RoutedEventArgs e)
    {
	    Console.WriteLine(""); 

        try
        { 
            RastrSrvShare.CRabbitParams par = new RastrSrvShare.CRabbitParams();
            Log("hello");
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
            RastrSrvShare.Ccommunicator m_communicator = new RastrSrvShare.Ccommunicator();
            par.m_str_name = "sender"; //m_configuration.GetValue<string>("r_params:name","");
            RastrSrvShare.CSigner signer_prv = new RastrSrvShare.CSigner();

             string str_path_exe_dir = file_dir_hlp.GetPathExeDir();
            string str_path_prv_key = str_path_exe_dir+"/"+RastrSrvShare.CSigner.str_fname_prv_xml;
            Log($"читаю приватный ключ находящийся [{str_path_prv_key}]");

            int nRes = signer_prv.ReadKey(str_path_prv_key);
            if(nRes<0)
            { 
                Log($"приватный ключ не прочитан.");
                return ;
            }
            m_communicator.Init(par, signer_prv); 

            RastrSrvShare.Ccommunicator.enCommands en_command;
            //en_command = RastrSrvShare.Ccommunicator.Command.StrToCommand(m_str_cmnd);
             
            string str_to = "";
            //string [] str_pars = { $"{str_dir_ftp}/{Path.GetFileName(m_path_to_file)}"};
            //RastrSrvShare.Ccommunicator.Command cmnd_pub = m_communicator. PublishCmnd( en_command, str_to, m_str_role, str_pars );

            Log("ok");

/*
            int nRes = signer_prv.ReadKey(str_path_prv_key);
            if(nRes<0)
            { 
                Log.Error($"приватный ключ не прочитан.");
                return ;
            }
            m_communicator.Init(par, signer_prv); 

            RastrSrvShare.Ccommunicator.enCommands en_command;
            en_command = RastrSrvShare.Ccommunicator.Command.StrToCommand(m_str_cmnd);
             
            string str_to = "";
            string [] str_pars = { $"{str_dir_ftp}/{Path.GetFileName(m_path_to_file)}"};
            RastrSrvShare.Ccommunicator.Command cmnd_pub = m_communicator.
                PublishCmnd( en_command, str_to, m_str_role, str_pars );

            /*
            /*
            //Log.Information($"читаю приватный ключ находящийся [{str_path_prv_key}]");
/*
            int nRes = signer_prv.ReadKey(str_path_prv_key);
            if(nRes<0)
            { 
                Log.Error($"приватный ключ не прочитан.");
                return ;
            }
            m_communicator.Init(par, signer_prv); 

            RastrSrvShare.Ccommunicator.enCommands en_command;
            en_command = RastrSrvShare.Ccommunicator.Command.StrToCommand(m_str_cmnd);
             
            string str_to = "";
            string [] str_pars = { $"{str_dir_ftp}/{Path.GetFileName(m_path_to_file)}"};
            RastrSrvShare.Ccommunicator.Command cmnd_pub = m_communicator.
                PublishCmnd( en_command, str_to, m_str_role, str_pars );
*/
        }
        catch (Exception ex) 
        {
            }
    }

    public void on_btn_click_send2(object sender, RoutedEventArgs e)
    {
	    Console.WriteLine(""); 
    }

    
}