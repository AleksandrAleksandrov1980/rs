
using Serilog;
using Npgsql;
// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");
//https://www.nuget.org/packages/Npgsql/
//https://www.postgresql.org/docs/7.4/jdbc-binary-data.html

//pg_hba.conf!!
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

    string tbHost= "192.168.1.59";
    //string tbPort="5432";
    string tbPort="5433";
        //string tbUser= "postgres";
        //string tbPass= "../3,.4eV#$%3,xcmx2345ASD";
    //string tbUser = "utest";
    string tbUser = "postgres";
    //string tbPass = "sdasd234df";
    string tbPass = "dfsdf23df4DF54t";
    //string tbDataBaseName = "tst1";
    string tbDataBaseName = "rstore";
    // PostgeSQL-style connection string
    string connstring = String.Format("Server={0};Port={1};User Id={2};Password={3};Database={4};"+
    "Pooling=false; Timeout=300; CommandTimeout=300;", tbHost, tbPort, tbUser,tbPass, tbDataBaseName);
    using (var conn = new NpgsqlConnection(connstring))
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

        //string sQL = "SELECT photo from picturetable WHERE id = 65";
        string sQL = "SELECT dump from dumps WHERE id = 2";
        using (var command = new NpgsqlCommand(sQL, conn))
        {
            byte[] productImageByte = null;
            var rdr = command.ExecuteReader();
            if (rdr.Read())
            {
                productImageByte = (byte[])rdr[0];
            }
            rdr.Close();
            if (productImageByte != null)
            {
                //using (MemoryStream productImageStream = new System.IO.MemoryStream(productImageByte))
                using (MemoryStream ms = new System.IO.MemoryStream(productImageByte))
                {
                    //ImageConverter imageConverter = new System.Drawing.ImageConverter();
                    //pictureBox1.Image = imageConverter.ConvertFrom(productImageByte) as System.Drawing.Image;
                    using (FileStream file = new FileStream( str_path_file_out, FileMode.Create, System.IO.FileAccess.Write )) 
                    {
                        byte[] bytes = new byte[ms.Length];
                        ms.Read(bytes, 0, (int)ms.Length);
                        file.Write(bytes, 0, bytes.Length);
                        Log.Warning($"saved -> {str_path_file_out}  : length {ms.Length}");
                        ms.Close();
                        /*
                        byte[] bytes = new byte[ms.Length];
                        ms.Read(bytes, 0, (int)ms.Length);
                        file.Write(bytes, 0, bytes.Length);
                        ms.Close();
                        */
                    }
                }
            }
        }

    }
}
catch(Exception ex)
{
    Log.Error($"exception:{ex.Message}");
}

