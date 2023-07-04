
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

        static int SetVal<T>( Rastr rastr, string str_table, string str_col, int n_row, T t_val )
        { 
            table t = rastr.Tables.Item(str_table);
            col   c = t.Cols.Item(str_col);
            c.Z[n_row] = t_val;
            return 1;
        }

        static int Main(string[] args)
        {
            string str_path_wrk_dir      = "";

            try
            { 
                if(args.Length<1)
                {
                    throw new Exception($"not enough arguments, get [{args.Length}]");
                }
                else
                { 
                    str_path_wrk_dir      = args[0]; // C:\rs_wrk
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

                const int MAGIC_CENTR_NUMBER = 27000;
                int nFilesCounter = 0;
                int nFailsCounter = 0;
                List<string> dirs = new List<string>(Directory.EnumerateDirectories(str_path_wrk_dir));
                List<string> lst_file_fails_to_count = new List<string>();
                foreach( string dir in dirs ) 
                {
                    nFilesCounter++;
                    string str_path_2_file = $"{dir}/roc_debug_after_OC";
                    Console.WriteLine( $"{nFilesCounter}:{dirs.Count} calc {str_path_2_file}" );
                    rastr.Load( RG_KOD.RG_REPL, str_path_2_file , "" );
                    SetVal(  rastr, "com_opf", "centr", 0, MAGIC_CENTR_NUMBER );
                    RastrRetCode rastrRet = rastr.opf("s");
                    if(rastrRet != RastrRetCode.AST_OK)
                    { 
                        Console.WriteLine( $"error to calc {str_path_2_file}" );
                        lst_file_fails_to_count.Add( str_path_2_file );
                    }
                    //rastr.Save(str_path_2_file+"_sav", "");
                }
                nFilesCounter = 0;
                Console.WriteLine( $"error to calc files: {lst_file_fails_to_count.Count} with magic number {MAGIC_CENTR_NUMBER}" );
                foreach( string str_file_failed in lst_file_fails_to_count) 
                { 
                    nFilesCounter++;
                    Console.WriteLine( $"{nFilesCounter}:{str_file_failed}" );
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