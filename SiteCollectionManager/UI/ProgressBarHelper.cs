using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SiteCollectionManager.UI
{
    public class ProgressBarHelper
    {
        private ProgressBar mProgressBar = null;
        private int mMax = -1;

        public ProgressBarHelper(System.Windows.Forms.ProgressBar procBar, int max) 
        {
            mProgressBar = procBar;
            procBar.Maximum = max;
            procBar.Value = 0;
            mMax = max;
        }

        [Obsolete("ProgressBarHelper(System.Windows.Forms.ProgressBar, int)")]
        protected void InitProcessBar(System.Windows.Forms.ProgressBar procBar, int max)
        {
            mProgressBar = procBar;
            procBar.Maximum = max;
            procBar.Value = 0;
            mMax = max;
        }

        public void ResetMaxValue(int max)
        {
            SetProgressValue(max);
            mMax = max;
        }

        public void InProgress()
        {
            int curr = mMax / 2;
            if (curr + 1 < mMax)
            {
                DisProcessFunc(curr + 1);
            }
            else
            {
                DisProcessFunc(curr);
            }
        }

        public void InProgress(int curr)
        {
            DisProcessFunc(curr);
        }

        public void EndProgress()
        {
            try
            {
                DisProcessFunc(mMax);
            }
            catch { }
        }

        public void ExceptionEndProgress()
        {
            try
            {
                //DisProcessFunc(0);
                EndProgress();
            }
            catch { }
        }

        delegate void DisProcess(int i);
        private void DisProcessFunc(int i)
        {
            if (mProgressBar.FindForm().InvokeRequired)
            {
                mProgressBar.FindForm().Invoke(new DisProcess(DisProcessFunc), new object[] { i });
            }
            else
            {
                mProgressBar.Value = i;
            }
        }

        delegate void SetMaxValue(int i);
        private void SetProgressValue(int i) 
        {
            if (mProgressBar.FindForm().InvokeRequired)
            {
                mProgressBar.Invoke(new SetMaxValue(SetProgressValue), new object[] { i });
            }
            else 
            {
                mProgressBar.Maximum = i;
            }
        }
    }
}
