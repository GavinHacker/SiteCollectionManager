using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SiteCollectionManager.UI
{
    public partial class RestoreOptionForm : Form
    {
        private Button mBtn2Ok;
        private TextBox mTextBox2Url;
        private CheckBox mCheckBox2IsEidit;
        private Label label1;
        private RadioButton mRadioBtn2Overwrite;
        private RadioButton mRadioBtn2NotOverwrite;

        private bool mAllowRestore = false;
        public bool AllowRestore
        {
            get 
            {
                return mAllowRestore;
            }
            set
            {
                mAllowRestore = value;
            }
        }

        public bool IsOverwrite 
        {
            get 
            {
                return mRadioBtn2Overwrite.Checked;
            }
        }

        public string Url 
        {
            get 
            {
                return mTextBox2Url.Text;
            }
        }

        public RestoreOptionForm(string url)
        {
            InitializeComponent();
            mTextBox2Url.Text = url;
            mTextBox2Url.Enabled = false;
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RestoreOptionForm));
            this.mBtn2Ok = new System.Windows.Forms.Button();
            this.mTextBox2Url = new System.Windows.Forms.TextBox();
            this.mCheckBox2IsEidit = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.mRadioBtn2Overwrite = new System.Windows.Forms.RadioButton();
            this.mRadioBtn2NotOverwrite = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // mBtn2Ok
            // 
            this.mBtn2Ok.Location = new System.Drawing.Point(408, 48);
            this.mBtn2Ok.Name = "mBtn2Ok";
            this.mBtn2Ok.Size = new System.Drawing.Size(99, 23);
            this.mBtn2Ok.TabIndex = 0;
            this.mBtn2Ok.Text = "Restore";
            this.mBtn2Ok.UseVisualStyleBackColor = true;
            this.mBtn2Ok.Click += new System.EventHandler(this.mBtn2Ok_Click);
            // 
            // mTextBox2Url
            // 
            this.mTextBox2Url.Location = new System.Drawing.Point(46, 12);
            this.mTextBox2Url.Name = "mTextBox2Url";
            this.mTextBox2Url.Size = new System.Drawing.Size(366, 22);
            this.mTextBox2Url.TabIndex = 1;
            // 
            // mCheckBox2IsEidit
            // 
            this.mCheckBox2IsEidit.AutoSize = true;
            this.mCheckBox2IsEidit.Location = new System.Drawing.Point(429, 15);
            this.mCheckBox2IsEidit.Name = "mCheckBox2IsEidit";
            this.mCheckBox2IsEidit.Size = new System.Drawing.Size(54, 18);
            this.mCheckBox2IsEidit.TabIndex = 2;
            this.mCheckBox2IsEidit.Text = "Edit";
            this.mCheckBox2IsEidit.UseVisualStyleBackColor = true;
            this.mCheckBox2IsEidit.CheckedChanged += new System.EventHandler(this.mCheckBox2IsEidit_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(28, 14);
            this.label1.TabIndex = 3;
            this.label1.Text = "Url";
            // 
            // mRadioBtn2Overwrite
            // 
            this.mRadioBtn2Overwrite.AutoSize = true;
            this.mRadioBtn2Overwrite.Checked = true;
            this.mRadioBtn2Overwrite.Location = new System.Drawing.Point(46, 51);
            this.mRadioBtn2Overwrite.Name = "mRadioBtn2Overwrite";
            this.mRadioBtn2Overwrite.Size = new System.Drawing.Size(88, 18);
            this.mRadioBtn2Overwrite.TabIndex = 4;
            this.mRadioBtn2Overwrite.TabStop = true;
            this.mRadioBtn2Overwrite.Text = "Overwrite";
            this.mRadioBtn2Overwrite.UseVisualStyleBackColor = true;
            // 
            // mRadioBtn2NotOverwrite
            // 
            this.mRadioBtn2NotOverwrite.AutoSize = true;
            this.mRadioBtn2NotOverwrite.Location = new System.Drawing.Point(190, 51);
            this.mRadioBtn2NotOverwrite.Name = "mRadioBtn2NotOverwrite";
            this.mRadioBtn2NotOverwrite.Size = new System.Drawing.Size(109, 18);
            this.mRadioBtn2NotOverwrite.TabIndex = 5;
            this.mRadioBtn2NotOverwrite.Text = "NotOverwrite";
            this.mRadioBtn2NotOverwrite.UseVisualStyleBackColor = true;
            // 
            // RestoreOptionForm
            // 
            this.ClientSize = new System.Drawing.Size(519, 85);
            this.Controls.Add(this.mRadioBtn2NotOverwrite);
            this.Controls.Add(this.mRadioBtn2Overwrite);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.mCheckBox2IsEidit);
            this.Controls.Add(this.mTextBox2Url);
            this.Controls.Add(this.mBtn2Ok);
            this.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "RestoreOptionForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void mCheckBox2IsEidit_CheckedChanged(object sender, EventArgs e)
        {
            mTextBox2Url.Enabled = true;
        }

        private void mBtn2Ok_Click(object sender, EventArgs e)
        {
            AllowRestore = true;
            this.Hide();
        }
    }
}
