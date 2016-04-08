using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SiteCollectionManager.UI;
using Microsoft.SharePoint.Utilities;
using System.IO;
using SiteCollectionManager._Common;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using SiteCollectionManager._Common.GException;

namespace SiteCollectionManager._Sevice
{
    class BRInfo 
    {
        public string SiteUrl = string.Empty;
        public bool Overwrite = false;
        public string FileLocation = string.Empty;
        public string FileName = string.Empty;
        public string FileNameEx = string.Empty;
        public bool IsRestoreSecurity = false;
    }

    class STSADMCommandFormat 
    {
        public static string BACKUP_OVERWRITE_DB = "-o backup -url {0} -filename {1} -overwrite";
        public static string BACKUP_NOMAL_DB = "-o backup -url {0} -filename {1}";
        public static string RESTORE_OVERWRITE_DB = "-o restore -url {0} -filename {1} -overwrite";
        public static string RESTORE_NOMAL_DB = "-o restore -url {0} -filename {1}";
        public static string BACKUP_INCLUDESECURITY_API = "-o export -url {0} -filename {1} -includeusersecurity";
        public static string BACKUP_NOMAL_API = "-o export -url {0} -filename {1}";
        public static string RESTORE_INCLUDESECURITY_API = "-o import -url {0} -filename {1} -includeusersecurity";
        public static string RESTORE_NOMAL_API = "-o import -url {0} -filename {1}";
    }

    class BackupSevice : GSevice, IDisposable
    {
        private BRInfo mBackupInfoSingle = null;
        private List<BRInfo> mBackupInfoList = null;
        //private string mBackupFileLocation = string.Empty;
        private string mSTSADMPath = string.Empty;
        public string STSADMPath
        {
            get 
            {
                if (string.IsNullOrEmpty(mSTSADMPath))
                {
                    mSTSADMPath = SPUtility.GetGenericSetupPath("BIN").TrimEnd('\\') + "\\STSADM.EXE"; 
                }
                return mSTSADMPath;
            }
        }
        private string mCommandString = string.Empty;
        public string CommandString
        {
            get;
            set;
        }

        public BackupSevice(ProgressBarHelper prgBar, TextBox txtBox, BRInfo info)
            : base(prgBar, txtBox)
        {
            mBackupInfoSingle = info;
            //mBackupInfoSingle.SiteUrl = info.SiteUrl;
        }

        public BackupSevice(ProgressBarHelper prgBar, TextBox txtBox, List<BRInfo> infoList)
            : base(prgBar, txtBox)
        {
            mBackupInfoList = infoList;
        }

        private void ExecuteInternal() 
        {
            if(string.IsNullOrEmpty(mBackupInfoSingle.FileLocation))
            {
                AppendTxtFunc(mMainFormMessageBox, "==> Please select backup file location.");
                return;
            }
            string gzDir = mBackupInfoSingle.FileLocation.TrimEnd('\\') + "\\" + GenerateFileName().TrimStart('\\');
            DirectoryInfo gzDirInfo = null;
            if (!Directory.Exists(gzDir)) 
            {
                gzDirInfo =  Directory.CreateDirectory(gzDir);
            }
            FileHelper.HiddenDirectory(gzDirInfo);
            mBackupInfoSingle.FileName = gzDirInfo.FullName.TrimEnd('\\') + "\\" + GenerateFileName().TrimStart('\\')+".dat";
            InvokeSysProcess.StartSTSADM(STSADMPath, GetCommand(STSADMCommandFormat.BACKUP_NOMAL_DB, mBackupInfoSingle.SiteUrl,mBackupInfoSingle.FileName), true);
            this.Log("Backup sevice processing...");
            mBackupInfoSingle.FileNameEx = mBackupInfoSingle.FileName.Substring(0, mBackupInfoSingle.FileName.LastIndexOf(".dat"))+".bak";
            InvokeSysProcess.StartSTSADM(STSADMPath, GetCommand(STSADMCommandFormat.BACKUP_NOMAL_API, mBackupInfoSingle.SiteUrl, mBackupInfoSingle.FileNameEx), true);
            FileHelper.GZ(gzDirInfo.FullName, gzDirInfo.FullName + ".gzip");
            Directory.Delete(gzDir, true);
        }

        public void Run() 
        {
            RunAsyn(null, null);
        }

        public void RunAsyn(object sender, EventArgs e)
        {
            try
            {
                int completedNum = 0;
                //string fileLocation = DialogHelper.OpenFolderLocation(null, null);
                foreach (BRInfo info in mBackupInfoList)
                {
                    //info.FileLocation = fileLocation;
                    mBackupInfoSingle = info;
                    ExecuteInternal();
                    base.mMainFormProgressBar.InProgress(++completedNum);
                    base.AppendTxtFunc(base.mMainFormMessageBox, string.Format("==> Backup site collection: {0} completed.", mBackupInfoSingle.SiteUrl));
                }
                base.AppendTxtFunc(base.mMainFormMessageBox, string.Format("Backup site collection num: {0} completed.", mBackupInfoList.Count));
            }
            catch (Exception ex)
            {
                this.Log(string.Format("A error occurred while backup site collection. Detail:{0}",ex.ToString()));
            }
        }

        public void RunSingle() 
        {
            //mBackupInfoSingle.FileLocation = DialogHelper.OpenFolderLocation(null, null);
            ExecuteInternal();
        }

        private string GenerateFileName()
        {
            string fileName = mBackupInfoSingle.SiteUrl;
            try
            {
                fileName = mBackupInfoSingle.SiteUrl.ToLower().Replace("https", "[gbak_]").Replace("http", "[gbak]").Replace("://", ".").Replace(":","@").Replace('/', '.') + string.Format(".[{0}{1}{2}{3}]", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Minute);
                return GenerateFileName(mBackupInfoSingle.FileLocation, fileName);
            }
            catch 
            {
                return null;
            }
        }

        private string GenerateFileName(string fileLocation, string fileName)
        {
            return FileHelper.GenerateFileName(fileLocation, fileName);
        }

        private string GetCommand(string frt, params object[] args) 
        {
            return string.Format(frt, args);
        }

        public void Dispose(object sender, EventArgs e) 
        {
            Dispose();
        }

        public void Dispose() 
        {
        }
    }

    class RestoreSevice : GSevice, IDisposable
    {
        private BRInfo mBackupDataInfo = null;
        private string mSelectedFileName = string.Empty;
        private string mSTSADMPath = string.Empty;
        public string STSADMPath
        {
            get
            {
                if (string.IsNullOrEmpty(mSTSADMPath))
                {
                    mSTSADMPath = SPUtility.GetGenericSetupPath("BIN").TrimEnd('\\') + "\\STSADM.EXE";
                }
                return mSTSADMPath;
            }
        }
        private string mCommandString = string.Empty;
        public string CommandString
        {
            get;
            set;
        }

        public RestoreSevice(ProgressBarHelper prgBar, TextBox txtBox, string fileName)
            : base(prgBar, txtBox)
        {
            this.mSelectedFileName = fileName;
        }

        private void ExecuteInternal()
        {
            string tempFolder = mBackupDataInfo.FileName.Substring(0, mBackupDataInfo.FileName.LastIndexOf('.'));
            DirectoryInfo tempFolderInfo = null;
            if (!Directory.Exists(tempFolder))
            {
                tempFolderInfo = Directory.CreateDirectory(tempFolder);
            }
            else
            {
                tempFolderInfo = new DirectoryInfo(tempFolder);
            }
            FileHelper.HiddenDirectory(tempFolderInfo);
            FileHelper.GDeZ(mBackupDataInfo.FileName, tempFolder);

            mBackupDataInfo.FileName = (from tp in tempFolderInfo.GetFiles() where tp.FullName.EndsWith(".dat") select tp.FullName).First<string>();
            mBackupDataInfo.FileNameEx = (from tp in tempFolderInfo.GetFiles() where tp.FullName.EndsWith(".bak") select tp.FullName).First<string>();

            int exitCode = int.MaxValue;
            if (mBackupDataInfo.Overwrite)
            {
                exitCode = InvokeSysProcess.StartSTSADM(STSADMPath, GetCommand(STSADMCommandFormat.RESTORE_OVERWRITE_DB, mBackupDataInfo.SiteUrl, mBackupDataInfo.FileName), true);
            }
            else
            {
                exitCode = InvokeSysProcess.StartSTSADM(STSADMPath, GetCommand(STSADMCommandFormat.RESTORE_NOMAL_DB, mBackupDataInfo.SiteUrl, mBackupDataInfo.FileName), true);
            }
            this.Log("Processing...");
            if (exitCode == -1)
            {
                EnsureSiteCollection(mBackupDataInfo.SiteUrl);
                this.Log("Ensure site collection...");
                if (mBackupDataInfo.IsRestoreSecurity)
                {
                    exitCode = InvokeSysProcess.StartSTSADM(STSADMPath, GetCommand(STSADMCommandFormat.RESTORE_NOMAL_API, mBackupDataInfo.SiteUrl, mBackupDataInfo.FileNameEx), true);
                }
                else
                {
                    exitCode = InvokeSysProcess.StartSTSADM(STSADMPath, GetCommand(STSADMCommandFormat.RESTORE_INCLUDESECURITY_API, mBackupDataInfo.SiteUrl, mBackupDataInfo.FileNameEx), true);
                }
            }
            if (exitCode != 0)
            {
                throw new RestoreSPDataException("A error occurred while restore site collection.", exitCode);
            }
            //Delete source bak data.
            {
                File.Delete(tempFolder + ".gzip");
                FileHelper.GZ(tempFolder, tempFolder + ".gzip");
                Directory.Delete(tempFolder, true);
            }
        }

        public void Run() 
        {
            RunAsyn(null, null);
        }

        public void RunAsyn(object sender, EventArgs e)
        {
            //int completedNum = 0;
            string siteUrl = ParserFileName(mSelectedFileName.Substring(mSelectedFileName.LastIndexOf('\\')+1));
            RestoreOptionForm restoreOp = new RestoreOptionForm(siteUrl);
            restoreOp.ShowDialog();
            if (restoreOp.AllowRestore)
            {
                siteUrl = restoreOp.Url.TrimEnd('/');
                try
                {
                    EnsureSiteCollection(siteUrl);
                    this.Log("Ensure site collection...");
                    mBackupDataInfo = new BRInfo();
                    mBackupDataInfo.FileName = mSelectedFileName;
                    mBackupDataInfo.Overwrite = restoreOp.IsOverwrite;// restoreOp.IsOverwrite;
                    mBackupDataInfo.SiteUrl = siteUrl;
                    ExecuteInternal();
                    base.mMainFormProgressBar.EndProgress();
                    base.AppendTxtFunc(base.mMainFormMessageBox, string.Format("==> Restore site collection: {0} completed.", mBackupDataInfo.SiteUrl));
                }
                catch (RestoreSPDataException ex)
                {
                    this.Log(string.Format("A error occurred while restore site collection by all method of site collection manager. Detail: {0}", ex.ToString()));
                }
                catch (Exception _ex)
                {
                    this.Log(string.Format("A error occurred while restore site collection. Unkown detail: {0}", _ex.ToString()));
                }
            }
            else 
            {
                base.mMainFormProgressBar.EndProgress();
                this.Log("Restore no site collection.");
            }
        }

        private void EnsureSiteCollection(string url) 
        {
            try
            {
                bool res = false;
                SPSite expectSite = new SPSite(url);
                System.Uri uri = new Uri(url);
                SPWebApplication webApp = SPWebApplication.Lookup(uri);
                foreach (SPSite site in webApp.Sites)
                {
                    if (expectSite.ID.Equals(site.ID))
                    {
                        res = true;
                        break;
                    }
                }
                if (res)
                {
                    expectSite.Dispose();
                    return;
                }
                throw new Exception("Ensure G.SiteCollection.");
            }
            catch 
            {
                System.Uri uri = new Uri(url);
                SPWebApplication webApp = SPWebApplication.Lookup(uri);
                SPSite site = webApp.Sites.Add(url, url, "", 1033, SPWebTemplate.WebTemplateSTS, System.Environment.UserDomainName + '\\' + System.Environment.UserName, System.Environment.UserName, null);
                site.Dispose();
                //return false;
            }
        }

        public string ParserFileName(string fileName) 
        {
            string siteUrl = string.Empty;
            fileName = fileName.ToLower().Replace("[gbak_]", "https").Replace("[gbak]", "http").Replace("@",":");
            string[] strUnits = fileName.Split(new char[] { '.' });
            siteUrl += strUnits[0] + "://";
            for (int i = 1; i < strUnits.Length-2; ++i) 
            {
                siteUrl += strUnits[i]+"/";
            }
            return siteUrl;
        }

        private string GetCommand(string frt, params object[] args)
        {
            return string.Format(frt, args);
        }

        public void Dispose(object sender, EventArgs e) 
        {
            Dispose();
        }

        public void Dispose() 
        {

        }
    }
}
