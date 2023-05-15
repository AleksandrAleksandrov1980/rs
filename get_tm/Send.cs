using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using RastrSrvShare;
using Serilog;
using static RastrSrvShare.Ccommunicator;

namespace publish
{
    internal class CSend
    {
        public string m_path_to_file    { set; get; } = "";
        public string m_ftp_dir         { set; get; } = "";
        public string m_str_cmnd        { set; get; } = "";
        public string m_role            { set; get; } = "";

        public int Run()
        {
            RastrSrvShare.CRabbitParams par = new RastrSrvShare.CRabbitParams();
            RastrSrvShare.ftp_hlp ftp_ = new ftp_hlp();
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
                ftp_.m_str_ftp_host     = con_sec_ftp.GetRequiredSection("host").Value;
                ftp_.m_str_ftp_user     = con_sec_ftp.GetRequiredSection("user").Value;
                ftp_.m_str_ftp_pass     = con_sec_ftp.GetRequiredSection("pass").Value;
                ftp_.m_n_ftp_port       = int.Parse(con_sec_ftp.GetRequiredSection("port").Value);
            }
            catch(Exception ex)
            { 
                Log.Error($"Can't read 'appsettings.json' in current directory exception[{ex}]");
                return -4;
            }
            string str_calc_guid    = Ccommunicator.GetTmMark();
            string str_dir_ftp_calc = $"{CParam.FtpDirCalcs}/{m_ftp_dir}/{str_calc_guid}";
            ftp_.file(ftp_hlp.enFtpDirection.UPLOAD, m_path_to_file, str_dir_ftp_calc);
            RastrSrvShare.Ccommunicator m_communicator = new RastrSrvShare.Ccommunicator();
            par.m_str_name = "sender"; //m_configuration.GetValue<string>("r_params:name","");
            RastrSrvShare.CSigner signer_prv = new RastrSrvShare.CSigner();
            string str_path_exe_dir = file_dir_hlp.GetPathExeDir();
            string str_path_prv_key = str_path_exe_dir+"/"+RastrSrvShare.CSigner.str_fname_prv_xml;
            Log.Information($"читаю приватный ключ находящийся [{str_path_prv_key}]");
            int nRes = signer_prv.ReadKey(str_path_prv_key);
            if(nRes<0)
            { 
                Log.Error($"приватный ключ не прочитан.");
                return -5;
            }
            m_communicator.Init(par, signer_prv); 

            RastrSrvShare.Ccommunicator.enCommands en_command;
            en_command = RastrSrvShare.Ccommunicator.Command.StrToCommand(m_str_cmnd);
             
            string str_to = "";
            string [] str_pars = { $"{m_ftp_dir}/{str_calc_guid}"};
            RastrSrvShare.Ccommunicator.Command cmnd_pub = m_communicator.
                PublishCmnd( en_command, str_to, m_role, str_pars );
            
            return 1;
        }
    }
}
