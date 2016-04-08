using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.AccessControl;
using ICSharpCode.SharpZipLib.Zip;

namespace SiteCollectionManager._Common
{
    internal class FileHelper
    {
        private static List<string> mTempFileList = new List<string>();
        public static List<string> TempFiles
        {
            get { return mTempFileList; }
        }

        private static string GetOrGenerateTempFolder() 
        {
            ClearTempFiles();
            string path = string.Empty;
            foreach(FileSystemInfo file in (new DirectoryInfo(Environment.CurrentDirectory)).GetFileSystemInfos())
            {
                if (file is DirectoryInfo && file.Name.Equals("_temp", StringComparison.OrdinalIgnoreCase))
                {
                    path = file.FullName;
                }
            }
            if (string.IsNullOrEmpty(path)) 
            {
                DirectoryInfo dirInfo = null;
                dirInfo = Directory.CreateDirectory(Environment.CurrentDirectory.TrimEnd('\\') + "\\" + "_temp");
                File.SetAttributes(dirInfo.FullName, File.GetAttributes(dirInfo.FullName)|FileAttributes.Hidden);
                path = dirInfo.FullName;
            }
            return path;
        }

        public static int HiddenDirectory(DirectoryInfo dirInfo) 
        {
            try
            {
                File.SetAttributes(dirInfo.FullName, File.GetAttributes(dirInfo.FullName) | FileAttributes.Hidden);
            }
            catch 
            {
                return -1;
            }
            return 0;
        }

        public static string GetTempFile(string txt)
        {
            string folderPath = GetOrGenerateTempFolder();
            string fileFullName = string.Empty;
            fileFullName = GenerateFileName(
                folderPath,
                string.Format("{0}.[{1}.{2}.{3}.{4}].dat", "temp", DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString(), DateTime.Now.Minute.ToString())
                );
            fileFullName = folderPath.TrimEnd('\\') + "\\" + fileFullName;
            File.Create(fileFullName).Close();
            mTempFileList.Add(fileFullName);

            StreamWriter writer = new StreamWriter(fileFullName, false, Encoding.UTF8);
            writer.Write(txt.ToCharArray());
            writer.Flush();
            writer.Close();

            return fileFullName;
        }

        public static string GenerateFileName(string fileLocation, string fileName)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(fileLocation);
            bool boo = false;
            while (true)
            {
                boo = false;
                foreach (FileSystemInfo fileSystemInfo in dirInfo.GetFileSystemInfos())
                {
                    if (fileSystemInfo is FileInfo)
                    {
                        string tempName = fileSystemInfo.Name.Substring(0, fileSystemInfo.Name.LastIndexOf('.'));
                        //FileInfo fInfo = (FileInfo)fileSystemInfo;
                        while (tempName.Equals(fileName))
                        {
                            fileName += '_';
                            boo = true;
                        }
                        if (boo)
                        {
                            break;
                        }
                    }
                }
                if (!boo)
                {
                    break;
                }
            }
            return fileName;
        }

        public static void ClearTempFiles()
        {
            for (int i = mTempFileList.Count-1; i>=0; --i)
            {
                try
                {
                    File.Delete(mTempFileList[i]);
                    mTempFileList.Remove(mTempFileList[i]);
                }
                catch 
                { }
            }
        }

        public static int GDeZ(string compressFile, string decompressDir)
        {
            if (string.IsNullOrEmpty(compressFile) || !File.Exists(compressFile))
            {
                throw new Exception("Compress file cannot be null.");
            }
            if (string.IsNullOrEmpty(decompressDir))
            {
                decompressDir = compressFile.Substring(0, compressFile.LastIndexOf('.'));
            }
            decompressDir = decompressDir.TrimEnd('\\') + '\\';
            if (!Directory.Exists(decompressDir))
            {
                Directory.CreateDirectory(decompressDir);
            }
            try
            {

                using (ZipInputStream stream = new ZipInputStream(File.OpenRead(compressFile)))
                {

                    ZipEntry zipEntry = null;
                    while ((zipEntry = stream.GetNextEntry()) != null)
                    {
                        string _folder = Path.GetDirectoryName(zipEntry.Name);
                        string _file = Path.GetFileName(zipEntry.Name);
                        if (_folder.Length > 0)
                        {
                            Directory.CreateDirectory(decompressDir + _folder);
                        }
                        _folder = _folder.TrimEnd('\\') + '\\';
                        if (!string.IsNullOrEmpty(_file))
                        {
                            using (FileStream fileStream = File.Create(decompressDir + zipEntry.Name))
                            {
                                int len = 65535;
                                byte[] data = new byte[len];
                                while (true)
                                {
                                    len = stream.Read(data, 0, data.Length);
                                    if (len <= 0)
                                    {
                                        break;
                                    }
                                    fileStream.Write(data, 0, len);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return 0;
        }

        public static int GZ(string compressDir, string compressFile)
        {
            if (string.IsNullOrEmpty(compressDir) || !Directory.Exists(compressDir))
            {
                throw new Exception("Compress directory cannot be null.");
            }
            compressDir = compressDir.TrimEnd('\\');
            if (string.IsNullOrEmpty(compressFile))
            {
                compressFile = compressDir + ".gz";
            }
            try
            {
                string[] files = Directory.GetFiles(compressDir);
                using (ZipOutputStream stream = new ZipOutputStream(File.Create(compressFile)))
                {
                    stream.SetLevel(9);
                    byte[] buff = new byte[65535];
                    foreach (string _file in files)
                    {
                        ZipEntry zipEntry = new ZipEntry(Path.GetFileName(_file));
                        zipEntry.DateTime = DateTime.Now;
                        stream.PutNextEntry(zipEntry);
                        using (FileStream fileStream = File.OpenRead(_file))
                        {
                            int _index = -1;
                            while (true)
                            {
                                _index = fileStream.Read(buff, 0, buff.Length);
                                if (_index <= 0)
                                {
                                    break;
                                }
                                stream.Write(buff, 0, _index);
                            }
                        }
                    }
                    stream.Finish();
                    stream.Close();
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return 0;
        }
    }
}
