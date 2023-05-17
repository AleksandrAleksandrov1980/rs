using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using FluentFTP;
using static RastrSrvShare.ftp_hlp;

namespace RastrSrvShare
{
    public class ftp_hlp
    {
        public string m_str_ftp_host = "";
        public string m_str_ftp_user = "";
        public string m_str_ftp_pass = "";
        public int    m_n_ftp_port   = 21;

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

        public enum enFtpDirection
        {
            UPLOAD,
            DOWNLOAD
        }

        public void dir( enFtpDirection en_ftp_dir, string str_path_from, string str_path_to )
        {
            //using(FtpClient ftp_client = new FtpClient( "192.168.1.59", "anon", "anon", 21 ) )
            using(FtpClient ftp_client = new FtpClient( m_str_ftp_host, m_str_ftp_user, m_str_ftp_pass, m_n_ftp_port ) )
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                ftp_client.Config.LogToConsole = false;
                ftp_client.Config.FXPDataType = FtpDataType.Binary; 
                ftp_client.Config.EncryptionMode = FtpEncryptionMode.None;
                ftp_client.Config.EncryptionMode = FtpEncryptionMode.None;
                ftp_client.Config.DownloadDataType = FtpDataType.Binary;
                ftp_client.Config.ValidateCertificateRevocation = false;
                System.Security.Cryptography.X509Certificates.X509CertificateCollection x = ftp_client.Config.ClientCertificates;
                ftp_client.Config.DataConnectionType = FtpDataConnectionType.AutoPassive;
                ftp_client.ValidateCertificate += (FluentFTP.Client.BaseClient.BaseFtpClient control, FtpSslValidationEventArgs e)=>{ e.Accept = true; };
                FtpProfile ftp_profile = new FtpProfile();
                ftp_profile.Encryption = FtpEncryptionMode.None;
                ftp_client.Connect();
                if(en_ftp_dir == enFtpDirection.UPLOAD)
                {
                    ftp_client.UploadDirectory( str_path_from, str_path_to, FtpFolderSyncMode.Update, FtpRemoteExists.Overwrite );
                }
                else
                {
                    ftp_client.DownloadDirectory( str_path_to, str_path_from, FtpFolderSyncMode.Update, FtpLocalExists.Overwrite );
                }
                ftp_client.Disconnect();
                sw.Stop();
                Log.Information($"FTP.dir {str_path_from} -> {str_path_to} elapsed time {sw.Elapsed:mm\\:ss\\.f}");
            }
        }


        public void file( enFtpDirection en_ftp_dir, string str_path_from, string str_path_to )
        {
            //using(FtpClient ftp_client = new FtpClient( "192.168.1.59", "anon", "anon", 21 ) )
            using(FtpClient ftp_client = new FtpClient( m_str_ftp_host, m_str_ftp_user, m_str_ftp_pass, m_n_ftp_port ) )
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                ftp_client.Config.LogToConsole = false;
                ftp_client.Config.FXPDataType = FtpDataType.Binary; 
                ftp_client.Config.EncryptionMode = FtpEncryptionMode.None;
                ftp_client.Config.EncryptionMode = FtpEncryptionMode.None;
                ftp_client.Config.DownloadDataType = FtpDataType.Binary;
                ftp_client.Config.ValidateCertificateRevocation = false;
                System.Security.Cryptography.X509Certificates.X509CertificateCollection x = ftp_client.Config.ClientCertificates;
                ftp_client.Config.DataConnectionType = FtpDataConnectionType.AutoPassive;
                ftp_client.ValidateCertificate += (FluentFTP.Client.BaseClient.BaseFtpClient control, FtpSslValidationEventArgs e)=>{ e.Accept = true; };
                FtpProfile ftp_profile = new FtpProfile();
                ftp_profile.Encryption = FtpEncryptionMode.None;
                ftp_client.Connect();
                if(en_ftp_dir == enFtpDirection.UPLOAD)
                {
                    string str_namef    = Path.GetFileName(str_path_from);
                    string str_ftp_file = str_path_to + "/"+str_namef;
                    ftp_client.CreateDirectory(str_path_to);
                    ftp_client.UploadFile(str_path_from,str_ftp_file,FtpRemoteExists.Overwrite);
                    FileInfo fi = new FileInfo(str_path_from); 
                    long size_ftp_file = ftp_client.GetFileSize(str_ftp_file);
                    if(fi.Length != size_ftp_file)
                    {
                        throw new Exception("size!!");
                    }
                }
                else
                {
                    string str_namef           = Path.GetFileName(str_path_from);
                    string str_path_file_local = str_path_to+"/"+str_namef;
                    //string str_ftp_file = str_path_ftp_dir+ "/"+str_namef;
                    //ftp_client.CreateDirectory(str_path_ftp_dir);
                    long size_ftp_file = ftp_client.GetFileSize(str_path_from);
                    ftp_client.DownloadFile(str_path_file_local,str_path_from,FtpLocalExists.Overwrite);
                    FileInfo fi = new FileInfo(str_path_file_local); 
                    if(fi.Length != size_ftp_file)
                    {
                        throw new Exception("size!!");
                    }
                    //ftp_client.UploadFile(@"C:/rs_wrk/compile.tar_1", "/compile.tar_2");
                    try
                    {
                        Stopwatch sw2 = new Stopwatch();
                        sw2.Start();
                        string str_hash_fun = "MD5"; //"MD5" "SHA1" "SHA256" "SHA384" "SHA512"
                        string str_hash_val = GetChecksum( str_hash_fun, str_path_file_local );
                        sw2.Stop();
                        Log.Warning($"hash {str_hash_fun} [{str_hash_val}] Ok. time {sw2.Elapsed}");
                    }
                    catch(Exception ex)
                    {
                        Log.Error($"hash exception [{ex}]");
                    }
                }
                ftp_client.Disconnect();
                sw.Stop();
                Log.Information($"FTP.file {str_path_from} -> {str_path_to} elapsed time {sw.Elapsed:mm\\:ss\\.f}");
            }
        }
    }
}
