using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RastrSrvShare
{
    public class file_dir_hlp
    {
        public static string GetPathExeDir()
        { 
            string str_path_exe_dir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            return str_path_exe_dir;
        }

        public static void CopyFilesRecursively( string sourcePath, string targetPath )
        {
            System.IO.Directory.CreateDirectory(targetPath);
            //Now Create all of the directories
            foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
            {
                Directory.CreateDirectory(dirPath.Replace(sourcePath, targetPath));
            }
            //Copy all the files & Replaces any files with the same name
            foreach (string newPath in Directory.GetFiles(sourcePath, "*.*",SearchOption.AllDirectories))
            {
                File.Copy(newPath, newPath.Replace(sourcePath, targetPath), true);
            }
        }
    }
}
