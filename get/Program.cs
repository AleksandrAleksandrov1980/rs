using Microsoft.Extensions.Configuration;
using RastrSrvShare;
using Serilog;

namespace get
{
    internal class Program
    {
        internal class CGet
        {
            public string m_ftp_path_to_file { set; get; } = "";
            public string m_path_to_out_dir  { set; get; } = "";
            public void Run()
            {
                RastrSrvShare.ftp_hlp ftp_ = new ftp_hlp();
                string str_path_current_dir = System.IO.Directory.GetCurrentDirectory();
                IConfiguration config = new ConfigurationBuilder()
                    .SetBasePath(str_path_current_dir)
                    .AddJsonFile("appsettings.json")
                    .Build();                    
                IConfigurationSection con_sec_params = config.GetSection("r_params");
                IConfigurationSection con_sec_ftp    = con_sec_params.GetSection("ftp");
                ftp_.m_str_ftp_host     = con_sec_ftp.GetRequiredSection("host").Value ?? "error";
                ftp_.m_str_ftp_user     = con_sec_ftp.GetRequiredSection("user").Value ?? "error";
                ftp_.m_str_ftp_pass     = con_sec_ftp.GetRequiredSection("pass").Value ?? "error";
                ftp_.m_n_ftp_port       = int.Parse(con_sec_ftp.GetRequiredSection("port").Value ?? "21");
                string str_ftp_path_to_file_download = $"{CParam.FtpDirCalcs}/{m_ftp_path_to_file}";
                string str_path_to_downloaded_file   = NormalizePath( $"{m_path_to_out_dir}/{CParam.LocDirCalcs}/{Path.GetDirectoryName(m_ftp_path_to_file)}/");
                Log( m_path_to_out_dir, $"download file from ftp [{str_ftp_path_to_file_download }] to local file [{str_path_to_downloaded_file}]");
                ftp_.file(ftp_hlp.enFtpDirection.DOWNLOAD, str_ftp_path_to_file_download, str_path_to_downloaded_file);
            }
        }

        public static string NormalizePath(string path)
        {
            return Path.GetFullPath(new Uri(path).LocalPath)
                       .TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
                       .ToUpperInvariant();
        }

        public static void Log(string str_dir, string str_msg, bool bl_append = false)
        { 
            using (StreamWriter writer = new StreamWriter($"{str_dir}/{System.AppDomain.CurrentDomain.FriendlyName}.log", bl_append))
            {
                Console.WriteLine(str_msg);
                writer.WriteLine(str_msg);
            }
        }

        static int Main(string[] args)
        {
            try
            { 
                CGet getter = new CGet();
                if(args.Length<2)
                { 
                    throw new Exception($"not enough arguments, get [{args.Length}]");
                }
                else 
                { 
                    getter.m_ftp_path_to_file = args[0];
                    getter.m_path_to_out_dir  = args[1];
                    getter.Run();
                }
            }
            catch(Exception ex)
            { 
                if(args.Length<2)
                { 
                    Log( "c:/tmp/", $"exception [{ex}]" );
                }
                else
                { 
                    Log( args[1], $"exception [{ex}]" );
                }
                return 100;
            }
            return 1;
        }
    }
}