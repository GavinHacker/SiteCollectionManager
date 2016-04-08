using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.SharePoint.Administration;

namespace SiteCollectionManager.UI
{
    public partial class CreateSeviceForm : Form
    {
        private TextBox mTextBox2Url;
        private ComboBox mComboBox2WebApp;
        private Label label3;
        private Label label4;
        private Label label5;
        private TextBox mTextBox2Num;
        private Button mBtn2Ok;
        private TextBox mTextBox2Title;
        private bool mAllowCreate = false;
        public bool AllowCreate 
        {
            get 
            {
                return mAllowCreate;
            }
            set 
            {
                mAllowCreate = value;
            }
        }

        public CreateSeviceForm() 
        {
            InitializeComponent();
            LoadWebAppOfCreateZone();
        }

        public SPWebApplication WebApp 
        {
            get
            {
                return mComboBox2WebApp.SelectedItem as SPWebApplication;
            }
        }

        public string Url
        {
            get
            {
                return mTextBox2Url.Text;
            }
        }

        public int Num
        {
            get 
            {
                if (string.IsNullOrEmpty(mTextBox2Num.Text)) 
                {
                    return 0;
                }
                return int.Parse(mTextBox2Num.Text);
            }
        }

        public string Titie
        {
            get 
            {
                return mTextBox2Title.Text;
            }
        }

        public void LoadWebAppOfCreateZone()
        {
            SPWebService spWebService = SPWebService.ContentService;
            if (spWebService != null)
            {
                try
                {
                    foreach (SPWebApplication webApp in spWebService.WebApplications)
                    {
                        mComboBox2WebApp.Items.Add(webApp);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CreateSeviceForm));
            this.mTextBox2Url = new System.Windows.Forms.TextBox();
            this.mComboBox2WebApp = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.mTextBox2Num = new System.Windows.Forms.TextBox();
            this.mTextBox2Title = new System.Windows.Forms.TextBox();
            this.mBtn2Ok = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // mTextBox2Url
            // 
            this.mTextBox2Url.Location = new System.Drawing.Point(57, 47);
            this.mTextBox2Url.Name = "mTextBox2Url";
            this.mTextBox2Url.Size = new System.Drawing.Size(420, 21);
            this.mTextBox2Url.TabIndex = 0;
            // 
            // mComboBox2WebApp
            // 
            this.mComboBox2WebApp.FormattingEnabled = true;
            this.mComboBox2WebApp.Location = new System.Drawing.Point(12, 12);
            this.mComboBox2WebApp.Name = "mComboBox2WebApp";
            this.mComboBox2WebApp.Size = new System.Drawing.Size(465, 20);
            this.mComboBox2WebApp.TabIndex = 1;
            this.mComboBox2WebApp.SelectedIndexChanged += new System.EventHandler(this.mComboBox2WebApp_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(12, 49);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(28, 14);
            this.label3.TabIndex = 4;
            this.label3.Text = "Url";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(12, 123);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(28, 14);
            this.label4.TabIndex = 5;
            this.label4.Text = "Num";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(12, 87);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(42, 14);
            this.label5.TabIndex = 6;
            this.label5.Text = "Title";
            // 
            // mTextBox2Num
            // 
            this.mTextBox2Num.Location = new System.Drawing.Point(57, 121);
            this.mTextBox2Num.Name = "mTextBox2Num";
            this.mTextBox2Num.Size = new System.Drawing.Size(96, 21);
            this.mTextBox2Num.TabIndex = 7;
            // 
            // mTextBox2Title
            // 
            this.mTextBox2Title.Location = new System.Drawing.Point(57, 85);
            this.mTextBox2Title.Name = "mTextBox2Title";
            this.mTextBox2Title.Size = new System.Drawing.Size(420, 21);
            this.mTextBox2Title.TabIndex = 8;
            // 
            // mBtn2Ok
            // 
            this.mBtn2Ok.Font = new System.Drawing.Font("Consolas", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mBtn2Ok.Location = new System.Drawing.Point(174, 118);
            this.mBtn2Ok.Name = "mBtn2Ok";
            this.mBtn2Ok.Size = new System.Drawing.Size(303, 23);
            this.mBtn2Ok.TabIndex = 9;
            this.mBtn2Ok.Text = "OK";
            this.mBtn2Ok.UseVisualStyleBackColor = true;
            this.mBtn2Ok.Click += new System.EventHandler(this.mBtn2Ok_Click);
            // 
            // CreateSeviceForm
            // 
            this.ClientSize = new System.Drawing.Size(489, 158);
            this.Controls.Add(this.mBtn2Ok);
            this.Controls.Add(this.mTextBox2Title);
            this.Controls.Add(this.mTextBox2Num);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.mComboBox2WebApp);
            this.Controls.Add(this.mTextBox2Url);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CreateSeviceForm";
            this.Text = "CreateSevice";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void mBtn2Ok_Click(object sender, EventArgs e)
        {
            this.mAllowCreate = true;
            this.Hide();
        }

        private void mComboBox2WebApp_SelectedIndexChanged(object sender, EventArgs e)
        {
            mTextBox2Url.Text = WebApp.AlternateUrls.GetResponseUrl(SPUrlZone.Default).Uri.ToString().TrimEnd('/')+"/sites/team_site";
            mTextBox2Title.Text = "Team Site";
            mTextBox2Num.Text = "0";
        }
    }
}
