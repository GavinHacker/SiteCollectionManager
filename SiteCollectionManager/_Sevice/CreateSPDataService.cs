using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using SiteCollectionManager.UI;
using System.Windows.Forms;

namespace SiteCollectionManager._Sevice
{
    class CreateDataInfo
    {
        public bool mIsCreateSeviceWorking = false;
        public SPWebApplication mWebApp = null;
        public int mCreateNumb = -1;
        public string mCreateTitle = string.Empty;
        public string mUrl = string.Empty;
    }

    class CreateSPDataService : GSevice, IDisposable
    {
        private CreateDataInfo mCreateInfo = null;

        public CreateSPDataService(ProgressBarHelper prgBar, TextBox txtBox, CreateDataInfo createInfo)
            : base(prgBar, txtBox)
        {
            mCreateInfo = createInfo;
        }

        public void Run()
        {
            RunAsyn(null, null);
        }

        public void RunAsyn(object sender, EventArgs e)
        {
            if (mCreateInfo.mCreateNumb == 0)
            {
                AppendTxtFunc(mMainFormMessageBox, "==> Create sevice create no site collection.");
                mCreateInfo.mIsCreateSeviceWorking = false;
                return;
            }

            mMainFormProgressBar.ResetMaxValue(mCreateInfo.mCreateNumb);
           
            int cnt = 0;
            int failedCnt = 0;
            SPSite site = null;
            try
            {
                site = mCreateInfo.mWebApp.Sites.Add(mCreateInfo.mUrl, mCreateInfo.mCreateTitle, "", 1033, SPWebTemplate.WebTemplateSTS, System.Environment.UserDomainName + '\\' + System.Environment.UserName, System.Environment.UserName, null);
                ++cnt;
                AppendTxtFunc(mMainFormMessageBox, string.Format("==>Created url: {0} completed.", site.Url));
                mMainFormProgressBar.InProgress(cnt);
            }
            catch (Exception ex)
            {
                ++failedCnt;
                AppendTxtFunc(mMainFormMessageBox, string.Format("==>Created url: {0} exception. Detail:{1}", site.Url, ex.ToString()));
                mMainFormProgressBar.ResetMaxValue(mCreateInfo.mCreateNumb - failedCnt);
            }
            site.Dispose();
            for (int i = 0; i < mCreateInfo.mCreateNumb - 1; ++i)
            {
                SPSite siteTemp = null;
                try
                {
                    siteTemp = mCreateInfo.mWebApp.Sites.Add(mCreateInfo.mUrl+i, mCreateInfo.mCreateTitle, "", 1033, SPWebTemplate.WebTemplateSTS, System.Environment.UserDomainName + '\\' + System.Environment.UserName, System.Environment.UserName, null);
                    ++cnt;
                    AppendTxtFunc(mMainFormMessageBox, string.Format("==>Created url: {0} completed.", siteTemp.Url));
                    mMainFormProgressBar.InProgress(cnt);
                    
                }
                catch (Exception _ex)
                {
                    ++failedCnt;
                    AppendTxtFunc(mMainFormMessageBox, string.Format("==>Created url: {0} exception. Detail:{1}", siteTemp.Url, _ex.ToString()));
                    mMainFormProgressBar.ResetMaxValue(mCreateInfo.mCreateNumb - failedCnt);
                }
                siteTemp.Dispose();
            }
            mCreateInfo.mIsCreateSeviceWorking = false;
        }


        public void Dispose(object sender, EventArgs e)
        {
            System.Console.WriteLine("GlobalThreadHandle invokder completed.");
            Dispose();
        }

        public void Dispose()
        {
        }
    }
}
