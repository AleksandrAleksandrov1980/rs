
using ASTRALib;
using Microsoft.Win32;
using System.Reflection;
using System.Reflection.Emit;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ObjectiveC;
using System.Text;

namespace calc
{
    internal class Program
    {
        [DllImport("kernel32.dll", CharSet=CharSet.Unicode, SetLastError=true)]
        public static extern IntPtr GetModuleHandle([MarshalAs(UnmanagedType.LPWStr)] string lpModuleName);

        [DllImport("kernel32.dll", SetLastError=true)]
        [PreserveSig]
        public static extern uint GetModuleFileName( [In] IntPtr hModule, [Out] StringBuilder lpFilename,  [In]  [MarshalAs(UnmanagedType.U4)]  int nSize );

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
            string str_path_wrk_dir      = "";
            string str_path_mdp_file     = "";
            string str_ftp_path_file_rg2 = "";

            try
            { 
                if(args.Length<3)
                {
                    throw new Exception($"not enough arguments, get [{args.Length}]");
                }
                else
                { 
                    str_path_wrk_dir      = args[0]; // C:\rs_wrk
                    str_path_mdp_file     = args[1]; // C:\rs_wrk\mdp_debug_1_30
                    str_ftp_path_file_rg2 = args[2]; // C:\rs_wrk\CALCS2\SE\2023_05_18___13_18_47_59013\roc_debug_after_OC
                }
                string str_path_shbl_rg2 = "";
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\RastrWin3"))
                {
                    if(key!=null) // most time service not installed by CURRENT_USER!!
                    { 
                        str_path_shbl_rg2 = key.GetValue("InstallPath") as string; 
                    }
                    else
                    {
                        try
                        { 
                            System.IntPtr pnHandlRastrDll = GetModuleHandle("astra.dll");
                            StringBuilder str_bldr = new StringBuilder(512);
                            GetModuleFileName(pnHandlRastrDll,str_bldr,str_bldr.Capacity);
                        }
                        catch(Exception ex)
                        {
                            Log(  str_path_wrk_dir, $"get shablon [{ex}]");
                        }
                    }
                }
                if( str_path_shbl_rg2==null || str_path_shbl_rg2?.Length<1)
                { 
                    str_path_shbl_rg2 = "C:\\Program Files (x86)\\RastrWin3";
                    Log(  str_path_wrk_dir, $"last chance get shablon from diresctory [{str_path_shbl_rg2}]");
                }
                str_path_shbl_rg2 = $"{str_path_shbl_rg2}\\RastrWin3\\SHABLON\\режим.rg2";
                Rastr rastr = new ASTRALib.Rastr();

                rastr.Load( RG_KOD.RG_REPL, str_path_mdp_file, "" );
                string str_path_file_rg2 = $"{str_path_wrk_dir}/{RastrSrvShare.CParam.LocDirCalcs}/{str_ftp_path_file_rg2}";
                rastr.Load( RG_KOD.RG_KEY, str_path_file_rg2, str_path_shbl_rg2 );

                ASTRALib.table table_ut_vir_common             = rastr.Tables.Item("ut_vir_common");
                ASTRALib.col   ut_vir_common_Col_log_path2file = table_ut_vir_common.Cols.Item("log_path2file");
                //string str_path_log_file = $"{str_path_wrk_dir}/{RastrSrvShare.CParam.LocDirCalcs}/{Path.GetDirectoryName(str_ftp_path_file_rg2)}/{Path.GetFileName(str_path_mdp_file)}.log";
                string str_path_log_file = $"{str_path_wrk_dir}/{Path.GetFileName(str_path_mdp_file)}.log";
                ut_vir_common_Col_log_path2file.Z[0] = str_path_log_file;
                //string str_path_mdp_debug_calc2 = $"{str_path_wrk_dir}/{RastrSrvShare.CParam.LocDirCalcs}/{Path.GetDirectoryName(str_ftp_path_file_rg2)}/{Path.GetFileName(str_path_mdp_file)}__2__";
                string str_path_mdp_debug_calc2 = $"{str_path_wrk_dir}/{Path.GetFileName(str_path_mdp_file)}__2__";
                //rastr.Save(@"C:\rs_wrk\tst.os","");
                object obj = null;
                int nRes = rastr.Emergencies(ref obj);
                rastr.Save(str_path_mdp_debug_calc2,"");
                if(nRes!=1)
                { 
                    throw new Exception($"Emergencies code [{nRes}]");
                }
            }
            catch(Exception ex)
            { 
                if(str_path_wrk_dir.Length<1)
                { 
                    Log( "c:/tmp/", $"exception [{ex}]" );
                }
                else
                { 
                    Log( str_path_wrk_dir, $"exception [{ex}]" );
                }
                return 100;
            }
            return 1;
        }
    }
}