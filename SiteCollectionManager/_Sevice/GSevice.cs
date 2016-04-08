using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SiteCollectionManager.UI;
using System.Windows.Forms;

namespace SiteCollectionManager._Sevice
{
    class GSevice
    {
        protected ProgressBarHelper mMainFormProgressBar = null;
        protected TextBox mMainFormMessageBox = null;

        public TextBox MessageBoxOfUI
        {
            get { return mMainFormMessageBox; }
        }

        public GSevice(ProgressBarHelper prgBar, TextBox txtBox)
        {
            mMainFormProgressBar = prgBar;
            mMainFormMessageBox = txtBox;
        }

        delegate void AppendTxt(TextBox messageBox, string str);
        public void AppendTxtFunc(TextBox messageBox, string str)
        {
            if (mMainFormMessageBox.FindForm().InvokeRequired)
            {
                mMainFormMessageBox.FindForm().Invoke(new AppendTxt(AppendTxtFunc), new object[] { messageBox, str });
            }
            else
            {
                messageBox.AppendText(string.Format(str + " {0} \r\n", DateTime.Now.ToString()));
            }
        }
    }
}
