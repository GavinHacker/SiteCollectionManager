using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SiteCollectionManager._Common;
using System.Resources;

namespace SiteCollectionManager.UI
{
    class AboutForm : Form, ISingleProvider
    {
        private TextBox mTextBox2About;
        private Button mBtn2Ok;

        private AboutForm() 
        {
            InitializeComponent();
            this.FormClosing += new FormClosingEventHandler(AboutForm_FormClosing);
            ResourceManager manger = new ResourceManager(typeof(AboutForm));
            mTextBox2About.Text = manger.GetString("mAboutString");
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutForm));
            this.mTextBox2About = new System.Windows.Forms.TextBox();
            this.mBtn2Ok = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // mTextBox2About
            // 
            this.mTextBox2About.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mTextBox2About.Location = new System.Drawing.Point(13, 13);
            this.mTextBox2About.Multiline = true;
            this.mTextBox2About.Name = "mTextBox2About";
            this.mTextBox2About.ReadOnly = true;
            this.mTextBox2About.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.mTextBox2About.Size = new System.Drawing.Size(489, 192);
            this.mTextBox2About.TabIndex = 0;
            // 
            // mBtn2Ok
            // 
            this.mBtn2Ok.Location = new System.Drawing.Point(331, 211);
            this.mBtn2Ok.Name = "mBtn2Ok";
            this.mBtn2Ok.Size = new System.Drawing.Size(171, 23);
            this.mBtn2Ok.TabIndex = 1;
            this.mBtn2Ok.Text = "OK";
            this.mBtn2Ok.UseVisualStyleBackColor = true;
            this.mBtn2Ok.Click += new System.EventHandler(this.mBtn2Ok_Click);
            // 
            // AboutForm
            // 
            this.ClientSize = new System.Drawing.Size(514, 238);
            this.Controls.Add(this.mBtn2Ok);
            this.Controls.Add(this.mTextBox2About);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "AboutForm";
            this.Text = "About";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void mBtn2Ok_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void AboutForm_FormClosing(object sender, FormClosingEventArgs e) 
        {
            e.Cancel = true;
            this.Hide();
        }
    }
}
