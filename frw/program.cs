using Serilog;
using Npgsql;
using System.Diagnostics;

//dotnet publish "C:\projects\git_main\rs\frw\frw.csproj" -c Release -o C:\projects\git_main\rs\frw\publish -r win-x64 --self-contained -p:PublishTrimmed=true

string str_path_log = @"C:/rs_wrk/logs/frw.log";
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File(str_path_log,
        rollingInterval: RollingInterval.Day,
        rollOnFileSizeLimit: true)
    .CreateLogger();

int rw_dir()
{
    
    //VAR1
    //http://pinvoke.net/default.aspx/advapi32/LogonUser.html    
    //IntPtr token;
    //LogonUser("username", "domain", "password", LogonType.LOGON32_LOGON_BATCH, LogonProvider.LOGON32_PROVIDER_DEFAULT);
    //WindowsIdentity identity = new WindowsIdentity(token);
    //WindowsImpersonationContext context = identity.Impersonate();
    //try{
    //    File.Copy(@"c:\temp\MyFile.txt", @"\\server\folder\Myfile.txt", true);
    //} finally {
    //  context.Undo();
    //}

    //VAR2
    //string updir = @"\\NetworkDrive\updates\somefile";
    //AppDomain.CurrentDomain.SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);
    //WindowsIdentity identity = new WindowsIdentity(username, password);
    //WindowsImpersonationContext context = identity.Impersonate();
    //File.Copy(updir, @"C:\somefile", true);

    // с диска win11u -> //alexandrov-7k/rs_wrk/res 100МБт 1 файл 1,3 сек, 10 = 11 сек, 30 файлов = 30 сек,
    // c //alexandrov-7k/rs_wrk/res на диск win11u ->  100МБт 1 файл 1,3 сек, 10 = 10 сек, 30 файлов = 30 сек,
    int cpy( string str_from, string str_to )
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();
        FileInfo fi1 = new FileInfo(str_from); 
        Log.Warning($"{str_from} --> {str_to}");
        File.Copy( str_from, str_to, true);
        FileInfo fi2 = new FileInfo(str_to); 
        if(fi1.Length != fi2.Length)
        {
            throw new Exception("size!!");
        }
        sw.Stop();
        Log.Warning($"read elapsed time {sw.Elapsed}");
        return 1;
    }

    try
    {
        string str_f_name     = @"c:/rs_wrk/res";
        //string str_share_name = @"//win11u/rs_wrk/res";   // \\win11u\rs_wrk
        string str_share_name = @"//alexandrov-7k/rs_wrk/res";   // //alexandrov-7k/rs_wrk/res \\win11u\rs_wrk
        Process currentProcess = Process.GetCurrentProcess();
        if(false) // upload to share
        {
            str_share_name += currentProcess.Id.ToString();
            cpy( str_f_name, str_share_name );
        }
        else // download from share
        {
            str_f_name += currentProcess.Id.ToString();
            cpy( str_share_name, str_f_name );
        }
    }
    catch(Exception ex)
    {
        Log.Error($"exception: {ex.Message}");
        return -1;
    }
    Console.ReadLine();
    return 1;
}

int rw_db()
{
    //https://www.nuget.org/packages/Npgsql/
    //https://www.postgresql.org/docs/7.4/jdbc-binary-data.html

    //----------------------------------------------------------------------------
    //!_postgresql.conf
    //lc_messages = 'en_US.UTF-8' #!u!!!
    //work_mem = 100MB #не повлияло на блоб https://github.com/pydanny/pydanny-event-notes/blob/master/DjangoConEurope2012/10-steps-to-better-postgresql-performance.rst 
    //maintenance_work_mem = 1000MB           #!u
    //effective_cache_size = 2GB              #!u
    //----------------------------------------------------------------------------

    //----------------------------------------------------------------------------
    //!_pg_hba.conf
    // hostnossl    all        all             all                     md5
    //----------------------------------------------------------------------------

    // c //alexandrov-7k/-postgresql на диск win11u ->  100МБт 1 файл 1,7 сек, 10 = 46 сек, 30 файлов = 60-180  сек,

    try
    {
        //string str_path_file = @"C:\rs_wrk\compile.tar_1";
        //string str_path_file = @"C:\rs_wrk\rs20230112.log";
        string str_path_file_in  = @"C:/rs_wrk/par.7z";
        string str_path_file_out = @"C:/rs_wrk/res";
        Log.Warning($"lets start party! ");

        string str_db_host = "192.168.1.59";
        string str_db_port = "5433"; //default 5432, but i have two srvrs
        string str_db_user = "postgres";
        string str_db_pass = "dfsdf23df4DF54t";
        string str_db_name = "rstore";
        // PostgeSQL-style connection string
        string str_db_conn = $"Server={str_db_host}; Port={str_db_port}; "+
            $"User Id={str_db_user}; Password={str_db_pass}; Database={str_db_name}; "+
            $"Pooling=false; Timeout=300; CommandTimeout=300; ";
        Process currentProcess = Process.GetCurrentProcess();
        using( var conn = new NpgsqlConnection(str_db_conn) )
        {
            conn.Open();
            /*
            // WRITE in SQL DB!
            using (FileStream pgFileStream = new FileStream( str_path_file, FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader pgReader = new BinaryReader(new BufferedStream(pgFileStream)))
                {
                    //NpgsqlCommand command = new NpgsqlCommand();
                    byte[] ImgByteA = pgReader.ReadBytes(Convert.ToInt32(pgFileStream.Length));
                    //string sQL = "insert into picturetable (id, photo) VALUES(65, @Image)";
                    string sQL = "insert into dumps ( id, dump ) VALUES( 2, @Image )";
                    using (var command = new NpgsqlCommand(sQL, conn))
                    {
                        NpgsqlParameter param = command.CreateParameter();
                        param.ParameterName = "@Image";
                        param.NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Bytea;
                        param.Value = ImgByteA;
                        command.Parameters.Add(param);
                        command.ExecuteNonQuery();
                    }
                }
            }*/
            string sQL = "SELECT dump from dumps WHERE id = 2";
            using( var command = new NpgsqlCommand(sQL, conn) )
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                byte[] productImageByte = null;
                var rdr = command.ExecuteReader();
                if (rdr.Read())
                {
                    productImageByte = (byte[])rdr[0];
                }
                rdr.Close();
                if (productImageByte != null)
                {
                    using (MemoryStream ms = new System.IO.MemoryStream(productImageByte))
                    {
                        str_path_file_out += currentProcess.Id.ToString();
                        using (FileStream file = new FileStream( str_path_file_out, FileMode.Create, System.IO.FileAccess.Write )) 
                        {
                            byte[] bytes = new byte[ms.Length];
                            ms.Read(bytes, 0, (int)ms.Length);
                            file.Write(bytes, 0, bytes.Length);
                            Log.Warning($"saved -> {str_path_file_out}  : length {ms.Length}");
                            ms.Close();
                        }
                    }
                }
                sw.Stop();
                Log.Warning($"read elapsed time {sw.Elapsed}");
            }
        }
    }
    catch(Exception ex)
    {
        Log.Error($"exception:{ex.Message}");
    }
    Console.ReadLine();
    return 1;
}

rw_db();
//rw_dir();

