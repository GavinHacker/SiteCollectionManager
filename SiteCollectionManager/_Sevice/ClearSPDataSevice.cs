using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.SharePoint;
using SiteCollectionManager._Tree.LoadTreeCore;
using SiteCollectionManager._Common;
using SiteCollectionManager.UI;

namespace SiteCollectionManager._Sevice
{
    class NeedClearDataInfo
    {
        public List<SPDataBox> mDeletingObjs = new List<SPDataBox>();
        public int mNeedDeleteNum = 0;
        public int mDeletedNum = 0;
        public bool mIsDeleting = false;
    }

    class ClearSPDataSevice : GSevice, IDisposable
    {
        private NeedClearDataInfo mNeedClearInfo = null;

        public ClearSPDataSevice(ProgressBarHelper prgBar, TextBox txtBox, NeedClearDataInfo clearInfo)
            : base(prgBar, txtBox)
        {
            mNeedClearInfo = clearInfo;
        }

        public void Run()
        {
            //GlobalThreadHandle.Start(RunAsyn, System.Threading.ThreadPriority.Highest, "SiteCollectionManager.ClearSevice");
            //GlobalThreadHandle threadHandle = new GlobalThreadHandle(this.GetType().FullName, RunAsyn, Dispose);
            RunAsyn(null, null);
        }

        public void RunAsyn(object sender, EventArgs e) 
        {
            var url = string.Empty;
            var type = string.Empty;
            int cnt = 0;
            for (int i = mNeedClearInfo.mDeletingObjs.Count - 1; i >= 0; --i)
            {
                url = mNeedClearInfo.mDeletingObjs[i].Url;
                type = mNeedClearInfo.mDeletingObjs[i].Type.ToString();
                DeleteInteral(mNeedClearInfo.mDeletingObjs[i]);
                AppendTxtFunc(mMainFormMessageBox, string.Format("==>Deleted {0} url:{1} completed.", type, url));
                mMainFormProgressBar.InProgress(mNeedClearInfo.mDeletedNum);
            }
        }

        internal void DeleteInteral(SPDataBox data)
        {
            data.DeleteSelf();
            mNeedClearInfo.mDeletingObjs.Remove(data);
            ++mNeedClearInfo.mDeletedNum;
        }

        public void Dispose(object sender, EventArgs e) 
        {
            System.Console.WriteLine("GlobalThreadHandle invokder completed.");
            Dispose();
        }

        public void Dispose() 
        {
            mNeedClearInfo.mDeletingObjs.Clear();
            mNeedClearInfo.mIsDeleting = false;
            mNeedClearInfo.mNeedDeleteNum = 0;
            mNeedClearInfo.mDeletedNum = 0;
        }
    }
}
