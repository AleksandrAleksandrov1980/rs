using Serilog;
using Npgsql;
using System.Diagnostics;
using FluentFTP;
//using System.IO.Hashing; - in 7?
using System.Net;

using System;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;

//dotnet publish "C:\projects\git_main\rs\frw\frw.csproj" -c Release -o C:\projects\git_main\rs\frw\publish -r win-x64 --self-contained -p:PublishTrimmed=true

string str_path_log = @"C:/rs_wrk/logs/frw.log";
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File(str_path_log,
        rollingInterval: RollingInterval.Day,
        rollOnFileSizeLimit: true)
    .CreateLogger();


static void TestHash()
{
    Stopwatch sw2 = new Stopwatch();
    sw2.Start();
    string str_hash_fun = "MD5";
    //"MD5" - broken for cryptografy - 32 symbols
    //SHA1, - broken for cryptografy
    //SHA256,
    //SHA384,
    //SHA512
    string str_f_path = @"C:\rs_wrk\res";
    string str_hash_val = GetChecksum( str_hash_fun, str_f_path );
    sw2.Stop();
    Log.Warning($"hash {str_hash_fun} [{str_hash_val}] Ok. time {sw2.Elapsed}");
    Console.ReadLine();
}


static string GetChecksum(string str_hash, string filename)
{
    using (var hasher = System.Security.Cryptography.HashAlgorithm.Create(str_hash))
    {
        if(hasher == null)
            throw new Exception("Can't create hasher!");
        using (var stream = System.IO.File.OpenRead(filename))
        {
            var hash = hasher.ComputeHash(stream);
            return BitConverter.ToString(hash).Replace("-", "");
        }
    }
}


int rw_ftp()
{
    // c //alexandrov-7k/-ftp на диск win11u ->  100МБт 1 файл 1.2 сек, 10 = 9 сек, 30 файлов = 29 сек, 100 = 80-90 сек, 1000 = 720сек + few errors? posibly memory
    try
    {
        Log.Warning($"Lets rock!");
        Stopwatch sw = new Stopwatch();
        sw.Start();
        // create an FTP client and specify the host, username and password
        //var client = new FtpClient("192.168.1.59", "anon", "anon");
        FtpClient ftp_client = new FtpClient( "192.168.1.59", "anon", "anon", 21 );
        //          ftp_client.Config.DataConnectionEncryption = false;
        //ftp_client.Config.EncryptionMode = FtpEncryptionMode.Implicit;
        //ftp_client.Config.EncryptionMode = FtpEncryptionMode.None;
        //            ftp_client.Config.EncryptionMode = FtpEncryptionMode.None;
        ftp_client.Config.FXPDataType = FtpDataType.Binary; 
        //       ftp_client.Config.SslProtocols = System.Security.Authentication.SslProtocols.None;
        ftp_client.Config.EncryptionMode = FtpEncryptionMode.None;
        //ftp_client.Config.EncryptionMode = FtpEncryptionMode.Explicit;
        ftp_client.Config.EncryptionMode = FtpEncryptionMode.None;
        //        ftp_client.Config.DataConnectionEncryption = false;
        ftp_client.Config.DownloadDataType = FtpDataType.Binary;
        //      ftp_client.Config.SslProtocols = System.Security.Authentication.SslProtocols.None;
        ftp_client.Config.ValidateCertificateRevocation = false;
        //ftp_client.SslProtocolActive
        //ftp_client.Config.ValidateAnyCertificate = false;
        System.Security.Cryptography.X509Certificates.X509CertificateCollection x = ftp_client.Config.ClientCertificates;
        //ftp_client.AutoDetect
        //ftp_client.Connect()
       /* 
        List<FtpProfile> lfp = ftp_client.AutoDetect(false);
        */
        //ftp_client.Config.DataConnectionType = FtpDataConnectionType.PASV;
        ftp_client.Config.DataConnectionType = FtpDataConnectionType.AutoPassive;
        ftp_client.Config.LogToConsole = true;
        ftp_client.ValidateCertificate += (FluentFTP.Client.BaseClient.BaseFtpClient control, FtpSslValidationEventArgs e)=>{ 
            e.Accept = true;
        };
        //ftp_client.ValidateCertificate 
        //ftp_client.Config.DataConnectionEncryption = true;
        //ftp_client.Config.EnableThreadSafeDataConnections = false;
        //FtpConfig ftp_conf = new FtpConfig();
        //ftp_conf.DataConnectionType = FtpDataConnectionType.AutoPassive;
        // connect to the server and automatically detect working FTP settings
        //FtpProfile ftp_profile = ftp_client.AutoConnect();// вот это к херам переопределяет по новой все настройки в соотвествии с её приоритетами!!! от SFTP -> plain FTP
        FtpProfile ftp_profile = new FtpProfile();
        ftp_profile.Encryption = FtpEncryptionMode.None;
        ftp_client.Connect();
/*
        foreach (FtpListItem item in ftp_client.GetListing("/")) {
            // if this is a file
            if (item.Type == FtpObjectType.File) {
                // get the file size
                long size = ftp_client.GetFileSize(item.FullName);
//                Log.Information($"{item.FullName} : size:{size}");
                // calculate a hash for the file on the server side (default algorithm)
                //FtpHash hash = ftp_client.GetChecksum(item.FullName); // FILEZILLA так не умеет
            }
            // get modified date/time of the file or folder
            DateTime time = ftp_client.GetModifiedTime(item.FullName);
        }
*/        
        Process currentProcess = Process.GetCurrentProcess();
        string str_f_path = @"C:/rs_wrk/res";
        string str_ftp_file = @"/res";
        long size_ftp_file = ftp_client.GetFileSize(str_ftp_file);
        str_f_path += currentProcess.Id.ToString();
        ftp_client.DownloadFile( str_f_path, str_ftp_file );
        FileInfo fi = new FileInfo(str_f_path); 
        if(fi.Length != size_ftp_file)
        {
            throw new Exception("size!!");
        }
        //ftp_client.UploadFile(@"C:/rs_wrk/compile.tar_1", "/compile.tar_2");
        // disconnect! good bye!
        ftp_client.Disconnect();
        sw.Stop();
        Log.Warning($"Ok. read elapsed time {sw.Elapsed}");
        Stopwatch sw2 = new Stopwatch();
        sw2.Start();
        string str_hash_fun = "SHA1";
        //"MD5"
        //	SHA1,
	//SHA256,
	//SHA384,
	//SHA512
        string str_hash_val = GetChecksum( str_hash_fun, str_f_path );
        sw2.Stop();
        Log.Warning($"hash {str_hash_fun} [{str_hash_val}] Ok. time {sw2.Elapsed}");
    }
    catch( Exception ex)
    {
        Log.Error($"ftp: {ex.Message}");
        return -1;
    }
    Console.ReadLine();
    return 1;
}

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

    //string fileName = "test.txt";
    //string sourcePath = @"C:\Users\Public\TestFolder";
    //string targetPath =  @"C:\Users\Public\TestFolder\SubDir";

    // Use Path class to manipulate file and directory paths.
    //string sourceFile = System.IO.Path.Combine(sourcePath, fileName);
    //string destFile = System.IO.Path.Combine(targetPath, fileName);


    // с диска win11u -> //alexandrov-7k/rs_wrk/res 100МБт 1 файл 1,3 сек, 10 = 11 сек, 30 файлов = 30 сек,
    // c //alexandrov-7k/rs_wrk/res на диск win11u ->  100МБт 1 файл 1,3 сек, 10 = 10 сек, 30 файлов = 30 сек (после 50 даже на 30 завливалось), 50 = errros 100-400 сек, 100 = errors + 180-300 сек
    // error "exception while reading from stream"!!!
    int cpy( string str_from, string str_to )
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();
        FileInfo fi1 = new FileInfo(str_from); 
        Log.Warning($"{str_from} --> {str_to}");
        File.Copy( str_from, str_to, true );
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

//rw_ftp();
//rw_db();
//rw_dir();
/*
partial class Program
{
  static void Main(string[] args)
  {
    foreach (var arg in args)
    {
      Console.WriteLine(arg);
    }
  }
}
*/

static string GetLocalIPAddress()
{
    var host = Dns.GetHostEntry(Dns.GetHostName());
    foreach (var ip in host.AddressList)
    {
        if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
        {
            return ip.ToString();
        }
    }
    throw new Exception("No network adapters with an IPv4 address in the system!");
}

// alexandrov-7k.ems.int
//alexandrov-7k.ems.int
//C:/rs_wrk/res
//\\alexandrov-7k.ems.int\rs_wrk
//file://alexandrov-7k.ems.int/rs_wrk/
//alexandrov-7k.ems.int/rs_wrk/
//frw.exe -uf "12323/res" "alexandrov-7k.ems.int/rs_wrk/res" 
Main("lll");


int SmbCopyFile( string str_from, string str_to )
{
    try
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();
        FileInfo fi1 = new FileInfo(str_from); 
        Log.Information($"copy.begin [{str_from}] --> [{str_to}]");
        File.Copy( str_from, str_to, true );
        FileInfo fi2 = new FileInfo(str_to); 
        if(fi1.Length != fi2.Length)
        {
            throw new Exception("size!!");
        }
        sw.Stop();
        Log.Warning($"");
        Log.Information($"copy.end. [{str_from}] --> [{str_to}] + time {sw.Elapsed}");
        return 1;

    }
    catch(Exception ex)
    {
        Log.Error($"Can't copy file: {ex.Message}");
        return -1;
    }
}

static string GetMd5( string str_path_to_file)
{
    using (var hasher = System.Security.Cryptography.HashAlgorithm.Create("MD5"))
    {
        if(hasher == null)
            throw new Exception("Can't create hasher!");
        using (var stream = System.IO.File.OpenRead(str_path_to_file))
        {
            var hash = hasher.ComputeHash(stream);
            return BitConverter.ToString(hash).Replace("-", "");
        }
    }
}

//GetMd5
static int RegisterFile(string str_path_to_file)
{
    string str_hash = GetMd5(str_path_to_file);
    FileInfo fi = new FileInfo(str_path_to_file); 
    string str = fi.DirectoryName;
    return 1;
}


static byte[]? HashAndSignBytes(byte[] DataToSign, RSAParameters Key)
{
    try
    {
        // Create a new instance of RSACryptoServiceProvider using the
        // key from RSAParameters.
        RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider();
        RSAalg.ImportParameters(Key); // PRIVATE KEY
        // Hash and sign the data. Pass a new instance of SHA256
        // to specify the hashing algorithm.
        return RSAalg.SignData(DataToSign, SHA256.Create());
    }
    catch(CryptographicException e)
    {
        Console.WriteLine(e.Message);
        return null;
    }
}

static bool VerifySignedHash(byte[] DataToVerify, byte[] SignedData, RSAParameters Key)
{
    try
    {
        // Create a new instance of RSACryptoServiceProvider using the
        // key from RSAParameters.
        RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider();
        RSAalg.ImportParameters(Key); // PUBLIC KEY
        // Verify the data using the signature.  Pass a new instance of SHA256
        // to specify the hashing algorithm.
        return RSAalg.VerifyData(DataToVerify, SHA256.Create(), SignedData);
    }
    catch(CryptographicException e)
    {
        Console.WriteLine(e.Message);
        return false;
    }
}

void tst_crypto()
{
    try
    {
        // Create a UnicodeEncoder to convert between byte array and string.
        ASCIIEncoding ByteConverter = new ASCIIEncoding();
        string dataString = "Data to Sign fghfgdhd ghghgfhfghddghfghdghfgdhfghfd"+
        "Data to Sign fghfgdhd ghghgfhfghddghfghdghfgdhfghfd"+
        "Data to Sign fghfgdhd ghghgfhfghddghfghdghfgdhfghfd"+
        "Data to Sign fghfgdhd ghghgfhfghddghfghdghfgdhfghfd"+
        "Data to Sign fghfgdhd ghghgfhfghddghfghdghfgdhfghfd"+
        "Data to Sign fghfgdhd ghghgfhfghddghfghdghfgdhfghfd"+
        "Data to Sign fghfgdhd ghghgfhfghddghfghdghfgdhfghfd"+
        "Data to Sign fghfgdhd ghghgfhfghddghfghdghfgdhfghfd"+
        "Data to Sign fghfgdhd ghghgfhfghddghfghdghfgdhfghfd"+
        "Data to Sign fghfgdhd ghghgfhfghddghfghdghfgdhfghfd"+
        "Data to Sign fghfgdhd ghghgfhfghddghfghdghfgdhfghfd"+
        "Data to Sign fghfgdhd ghghgfhfghddghfghdghfgdhfghfd";
        // Create byte arrays to hold original, encrypted, and decrypted data.
        byte[] originalData = ByteConverter.GetBytes(dataString);
        byte[] signedData;
        // Create a new instance of the RSACryptoServiceProvider class
        // and automatically create a new key-pair.
        RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider();
        // Export the key information to an RSAParameters object.
        // You must pass true to export the private key for signing.
        // However, you do not need to export the private key
        // for verification.
        RSAParameters Key = RSAalg.ExportParameters(true);
        RSAParameters Key1 = RSAalg.ExportParameters(true);
        byte[] b_priv_key = RSAalg.ExportRSAPrivateKey();
        byte[] b_priv_key1 = RSAalg.ExportRSAPrivateKey();
        byte[] b_publ_key = RSAalg.ExportRSAPublicKey();
        byte[] b_publ_key2 = RSAalg.ExportRSAPublicKey();


        string str_xml1 = RSAalg.ToXmlString(true);
        string str_xml2 = RSAalg.ToXmlString(false);

         XDocument doc1 = XDocument.Parse(str_xml1);
         string s1 = doc1.ToString();

         XDocument doc2 = XDocument.Parse(str_xml2);
         string s2 = doc2.ToString();

        doc1.Save("key_prv.xml");
        doc2.Save("key_pub.xml");


        XDocument doc_prv_2 = XDocument.Load("key_prv.xml");
        XDocument doc_pub_2 = XDocument.Load("key_pub.xml");

        RSAalg.FromXmlString(doc_prv_2.ToString());
        //RSAalg.FromXmlString(doc_pub_2.ToString());

        
        string str_xml4 = RSAalg.ToXmlString(false);
        string str_xml3 = RSAalg.ToXmlString(true);
        
        //RSAParameters Key = RSAalg.ExportParameters(false);
        // Hash and sign the data.
        signedData = HashAndSignBytes(originalData, Key);
        // Verify the data and display the result to the
        // console.
        if(VerifySignedHash(originalData, signedData, Key))
        {
            Console.WriteLine("The data was verified.");
        }
        else
        {
            Console.WriteLine("The data does not match the signature.");
        }
    }
    catch(ArgumentNullException)
    {
        Console.WriteLine("The data was not signed or verified");
    }
}

void Main(string s)
{
    try
    {

        tst_crypto();
        //return;

        int nRes = 0;
        Console.WriteLine(GetLocalIPAddress());
        switch(args[0] )
        {
            case "-f":
                if(args.Length < 3 )
                {
                    new Exception($"not enough arguments get only {args.Length}");
                }
                nRes = SmbCopyFile( args[1], args[2] );// 1->2
            break;

            default :
                throw new Exception($" unknown first command : {args[0]}");
            break;
        }
        Console.WriteLine(args);
    }
    catch(Exception ex)
    {
        Log.Error($"Exception: {ex.Message}");
        Environment.ExitCode = 13;
    }
    Environment.ExitCode = 1;
}

