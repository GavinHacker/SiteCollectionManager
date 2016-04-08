using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SiteCollectionManager._Common;

namespace SiteCollectionManager.UI
{
    class GPropsForm : Form, ISingleProvider
    {
        private TabControl mTabControl;

        private GPropsForm() 
        {
            InitializeComponent();
            this.Text = "All Properties";
            this.FormClosing += new FormClosingEventHandler(FormChosingFunc);
        }

        private GTabPropertyPage mTabPropertyPage = null;
        private GTabPropertyPage GTabProps
        {
            get
            {
                if (mTabPropertyPage == null)
                {
                    mTabPropertyPage = new GTabPropertyPage("GProps", new object());
                }
                return mTabPropertyPage;
            }
        }

        public void Show(object obj, string text) 
        {
            if (mTabControl.TabPages.ContainsKey(GTabProps.Name))
            {
                ((GTabPropertyPage)mTabControl.TabPages[GTabProps.Name]).Grid.SelectedObject = obj;
                ((GTabPropertyPage)mTabControl.TabPages[GTabProps.Name]).Text = text;
            }
            else 
            {
                GTabProps.Grid.SelectedObject = obj;
                GTabProps.Text = text;
                mTabControl.TabPages.Add(GTabProps);
            }
            base.Show();
        }

        private void FormChosingFunc(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            HideWindows();
        }

         private void HideWindows()
         {
             this.Hide();
         }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GPropsForm));
            this.mTabControl = new System.Windows.Forms.TabControl();
            this.SuspendLayout();
            // 
            // mTabControl
            // 
            this.mTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mTabControl.Location = new System.Drawing.Point(0, 0);
            this.mTabControl.Name = "mTabControl";
            this.mTabControl.SelectedIndex = 0;
            this.mTabControl.Size = new System.Drawing.Size(387, 491);
            this.mTabControl.TabIndex = 0;
            // 
            // GPropsForm
            // 
            this.ClientSize = new System.Drawing.Size(387, 491);
            this.Controls.Add(this.mTabControl);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "GPropsForm";
            this.ResumeLayout(false);

        }
    }
}
