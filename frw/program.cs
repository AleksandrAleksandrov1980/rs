using Serilog;
using Npgsql;
using System.Diagnostics;

//https://www.nuget.org/packages/Npgsql/
//https://www.postgresql.org/docs/7.4/jdbc-binary-data.html

//!_postgresql.conf
//lc_messages = 'en_US.UTF-8'

//!_pg_hba.conf
// hostnossl    all        all             all                     md5

try
{
    string str_path_log = @"C:/rs_wrk/logs/frw.log";
    Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Information()
        .WriteTo.Console()
        .WriteTo.File(str_path_log,
            rollingInterval: RollingInterval.Day,
            rollOnFileSizeLimit: true)
        .CreateLogger();

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
    using( var conn = new NpgsqlConnection(str_db_conn) )
    {
        conn.Open();
        /*
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

