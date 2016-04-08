using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SiteCollectionManager._Tree.LoadTreeCore;
using SiteCollectionManager._Sevice;

namespace SiteCollectionManager.UI
{
    public partial class MainForm : Form
    {
        private TreeLoader mTreeLoader = null;
        private NeedClearDataInfo mClearDataInfo = null;

        public MainForm()
        {
            InitializeComponent();
            LoadTreeInit();
            _DelegateInit();
        }

        private int _DelegateInit() 
        {
            mTreeViewOfMainForm.AfterCheck += new TreeViewEventHandler(mTreeViewOfMainForm_AfterCheck);
            //mTreeViewOfMainForm.DoubleClick += new EventHandler(this.propertiesToolStripMenuItem_Click);
            this.MinimumSizeChanged += new EventHandler(MinumSizeChanged);
            this.FormClosing += new FormClosingEventHandler(FormChosingFunc);
            this.mNotifyIcon.MouseDoubleClick += new MouseEventHandler(mNotifyIcon_MouseDoubleClick);
            return 0;
        }

        public void LoadTreeInit() 
        {
            mTreeLoader = new TreeLoader(mTreeViewOfMainForm, true);
            mTreeLoader.SetMessageBoxControl(mTextBox2Message);
            mTreeLoader.GenrateTree();
        }

        public void Log(string msg)
        {
            this.mTextBox2Message.AppendText("==> " + string.Format(msg + " {0} \r\n", DateTime.Now.ToString()));
            this.mTextBox2Message.Focus();
        }

        #region <-Quit->
        private void eixtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //
            this.Close();
            Application.Exit();
        }
        #endregion

        #region <-View->
        private void standandBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!this.mStandandBarToolStripMenuItem2FormView.Checked)
            {
                this.toolStrip1.Visible = false;
                this.groupBox1.Top -= toolStrip1.Size.Height;
                this.groupBox1.Height += toolStrip1.Size.Height;
                this.mGroupBox2PrimaryBar.Top -= toolStrip1.Size.Height;
                this.mGroupBox2PrimaryBar.Height += toolStrip1.Size.Height;
            }
            else 
            {
                this.groupBox1.Top += toolStrip1.Size.Height;
                this.groupBox1.Height -= toolStrip1.Size.Height;
                this.mGroupBox2PrimaryBar.Top += toolStrip1.Size.Height;
                this.mGroupBox2PrimaryBar.Height -= toolStrip1.Size.Height;

                this.toolStrip1.Visible = true;
            }
        }

        private void localBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!mLocalBarToolStripMenuItem.Checked)
            {
                mGroupBox2PrimaryBar.Visible = false;
                this.groupBox1.Tag = mGroupBox2PrimaryBar.Width + (mGroupBox2PrimaryBar.Left - this.groupBox1.Right);
                this.groupBox1.Width = this.groupBox1.Width + mGroupBox2PrimaryBar.Width + (mGroupBox2PrimaryBar.Left - this.groupBox1.Right);
            }
            else 
            {
                this.groupBox1.Width -= (int)this.groupBox1.Tag;
                mGroupBox2PrimaryBar.Visible = true;
            }
        }
        #endregion

        #region <-Notify->
        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            QuitAppliction();
        }

        private void QuitAppliction()
        {
            this.Close();
            this.Dispose();
            Application.Exit();
        }

        private void HideWindows()
        {
            this.Hide();
        }

        private void ShowMainForm()
        {
            this.GActiveForm();
        }

        public void GActiveForm()
        {
            if (this.WindowState == System.Windows.Forms.FormWindowState.Minimized)
            {
                this.WindowState = FormWindowState.Normal;
            }
            this.Show();
            this.TopLevel = true;
            this.Activate();
        }

        private void mNotifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.ShowMainForm();
        }

        private void MinumSizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.HideWindows();
            }
        }

        private void FormChosingFunc(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            HideWindows();
        }
        #endregion

    }
}
