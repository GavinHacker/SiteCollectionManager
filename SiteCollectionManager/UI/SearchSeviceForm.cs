using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SiteCollectionManager._Common;
using SiteCollectionManager._Sevice;

namespace SiteCollectionManager.UI
{
    class SearchSeviceForm:Form, ISingleProvider
    {
        private ComboBox mComboBox2SearchType;
        private Panel panel1;
        private ComboBox mComboBox2SearchBy;
        private GroupBox groupBox1;
        private Button mButton2Search;
        private SplitContainer splitContainer1;
        private ListView mListView2SearchResusts;
        private TabControl mTabControl2DisSPObj;
        private ColumnHeader Title;
        private ColumnHeader columnHeader1;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel mToolStripStatusLabel2SearchStatus;
        private TextBox mTextBox2SearchIndex;

        private string STATUS_READY 
        {
            get { return "Ready " + DateTime.Now.ToString(); }
        }
        private string STATUS_COMPLETED
        {
            get { return "Completed " + DateTime.Now.ToString(); }
        }
        private string STATUS_NOT_FOUND 
        {
            get { return "Not found " + DateTime.Now.ToString(); }
        }
        private string STATUS_EXCEPTION 
        {
            get { return "Exception " + DateTime.Now.ToString(); }
        }
        private string STATUS_RUNNING 
        {
            get { return "Running " + DateTime.Now.ToString(); }
        }

        private SearchSeviceForm() 
        {
            InitializeComponent();
            Init();
        }

        private void Init() 
        {
            mComboBox2SearchType.Items.Add(SPObjTypeEnum.ContentType);
            mComboBox2SearchType.Items.Add(SPObjTypeEnum.Feature);
            mComboBox2SearchType.Items.Add(SPObjTypeEnum.Field);
            //mComboBox2SearchType.Items.Add(SPObjTypeEnum.Item);
            //mComboBox2SearchType.Items.Add(SPObjTypeEnum.List);
            mComboBox2SearchType.Items.Add(SPObjTypeEnum.View);
            mComboBox2SearchType.SelectedItem = SPObjTypeEnum.Feature;

            mComboBox2SearchBy.Items.Add(SearchByEumn.ID);
            mComboBox2SearchBy.Items.Add(SearchByEumn.Title);
            mComboBox2SearchBy.Items.Add(SearchByEumn.URL);
            mComboBox2SearchBy.SelectedItem = SearchByEumn.ID;
            mComboBox2SearchBy.Enabled = false;
        }

        private GTabPropertyPage mTabPropertyPage = null;
        private GTabPropertyPage TabPropsPage
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

        public SPDataBox SearchScopeObj
        {
            get;
            set;
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SearchSeviceForm));
            this.mTextBox2SearchIndex = new System.Windows.Forms.TextBox();
            this.mComboBox2SearchType = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.mListView2SearchResusts = new System.Windows.Forms.ListView();
            this.Title = new System.Windows.Forms.ColumnHeader();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.mTabControl2DisSPObj = new System.Windows.Forms.TabControl();
            this.mComboBox2SearchBy = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.mButton2Search = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.mToolStripStatusLabel2SearchStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.panel1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // mTextBox2SearchIndex
            // 
            this.mTextBox2SearchIndex.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.mTextBox2SearchIndex.Location = new System.Drawing.Point(6, 15);
            this.mTextBox2SearchIndex.Multiline = true;
            this.mTextBox2SearchIndex.Name = "mTextBox2SearchIndex";
            this.mTextBox2SearchIndex.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.mTextBox2SearchIndex.Size = new System.Drawing.Size(453, 56);
            this.mTextBox2SearchIndex.TabIndex = 2;
            // 
            // mComboBox2SearchType
            // 
            this.mComboBox2SearchType.FormattingEnabled = true;
            this.mComboBox2SearchType.Location = new System.Drawing.Point(465, 15);
            this.mComboBox2SearchType.Name = "mComboBox2SearchType";
            this.mComboBox2SearchType.Size = new System.Drawing.Size(91, 20);
            this.mComboBox2SearchType.TabIndex = 3;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.splitContainer1);
            this.panel1.Location = new System.Drawing.Point(1, 81);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(633, 452);
            this.panel1.TabIndex = 4;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.mListView2SearchResusts);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.mTabControl2DisSPObj);
            this.splitContainer1.Size = new System.Drawing.Size(633, 452);
            this.splitContainer1.SplitterDistance = 211;
            this.splitContainer1.TabIndex = 2;
            // 
            // mListView2SearchResusts
            // 
            this.mListView2SearchResusts.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Title,
            this.columnHeader1});
            this.mListView2SearchResusts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mListView2SearchResusts.Location = new System.Drawing.Point(0, 0);
            this.mListView2SearchResusts.Name = "mListView2SearchResusts";
            this.mListView2SearchResusts.Size = new System.Drawing.Size(211, 452);
            this.mListView2SearchResusts.TabIndex = 1;
            this.mListView2SearchResusts.UseCompatibleStateImageBehavior = false;
            this.mListView2SearchResusts.View = System.Windows.Forms.View.Details;
            this.mListView2SearchResusts.Click += new System.EventHandler(this.mListView2SearchResusts_Click);
            // 
            // Title
            // 
            this.Title.Text = "Title";
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "ParentType";
            this.columnHeader1.Width = 119;
            // 
            // mTabControl2DisSPObj
            // 
            this.mTabControl2DisSPObj.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mTabControl2DisSPObj.Location = new System.Drawing.Point(0, 0);
            this.mTabControl2DisSPObj.Name = "mTabControl2DisSPObj";
            this.mTabControl2DisSPObj.SelectedIndex = 0;
            this.mTabControl2DisSPObj.Size = new System.Drawing.Size(418, 452);
            this.mTabControl2DisSPObj.TabIndex = 0;
            // 
            // mComboBox2SearchBy
            // 
            this.mComboBox2SearchBy.FormattingEnabled = true;
            this.mComboBox2SearchBy.Location = new System.Drawing.Point(562, 14);
            this.mComboBox2SearchBy.Name = "mComboBox2SearchBy";
            this.mComboBox2SearchBy.Size = new System.Drawing.Size(61, 20);
            this.mComboBox2SearchBy.TabIndex = 5;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.mButton2Search);
            this.groupBox1.Controls.Add(this.mTextBox2SearchIndex);
            this.groupBox1.Controls.Add(this.mComboBox2SearchBy);
            this.groupBox1.Controls.Add(this.mComboBox2SearchType);
            this.groupBox1.Location = new System.Drawing.Point(1, -3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(633, 78);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            // 
            // mButton2Search
            // 
            this.mButton2Search.Font = new System.Drawing.Font("Consolas", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mButton2Search.Location = new System.Drawing.Point(465, 42);
            this.mButton2Search.Name = "mButton2Search";
            this.mButton2Search.Size = new System.Drawing.Size(158, 28);
            this.mButton2Search.TabIndex = 6;
            this.mButton2Search.Text = "Search";
            this.mButton2Search.UseVisualStyleBackColor = true;
            this.mButton2Search.Click += new System.EventHandler(this.mButton2Search_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mToolStripStatusLabel2SearchStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 511);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(636, 22);
            this.statusStrip1.TabIndex = 7;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // mToolStripStatusLabel2SearchStatus
            // 
            this.mToolStripStatusLabel2SearchStatus.Name = "mToolStripStatusLabel2SearchStatus";
            this.mToolStripStatusLabel2SearchStatus.Size = new System.Drawing.Size(0, 17);
            // 
            // SearchSeviceForm
            // 
            this.ClientSize = new System.Drawing.Size(636, 533);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SearchSeviceForm";
            this.Text = "Search Sevice";
            this.panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void mButton2Search_Click(object sender, EventArgs e)
        {
            mToolStripStatusLabel2SearchStatus.Text = STATUS_READY;
            try
            {
                this.mListView2SearchResusts.Items.Clear();
                SearchSevice search = new SearchSevice(null, null,
                    new SearchInfo
                    {
                        mSearchBy = (SearchByEumn)mComboBox2SearchBy.SelectedItem,
                        mSearchIndex = mTextBox2SearchIndex.Text,
                        mSearchScop = SearchScopeObj,
                        mSearchType = (SPObjTypeEnum)mComboBox2SearchType.SelectedItem
                    });
                mToolStripStatusLabel2SearchStatus.Text = STATUS_RUNNING;
                List<SPDataBox> list = search.Run();
                //ListView2SearchResusts.Items.AddRange(list.Select(obj => new ListViewItem(new string[] { obj.Title })).ToArray());
                if (list.Count == 0) 
                {
                    mToolStripStatusLabel2SearchStatus.Text = STATUS_NOT_FOUND;
                    TabPropsPage.Text = STATUS_NOT_FOUND;
                    return;
                }
                foreach (SPDataBox box in list)
                {
                    ListViewItem item = new ListViewItem(new string[] { box.Title, box.ParentType.ToString() });
                    item.Tag = box.SPObj;
                    mListView2SearchResusts.Items.Add(item);
                }
                mToolStripStatusLabel2SearchStatus.Text = STATUS_COMPLETED;
            }
            catch (Exception ex)
            {
                mToolStripStatusLabel2SearchStatus.Text = STATUS_EXCEPTION;
                if (!mTabControl2DisSPObj.TabPages.ContainsKey(TabPropsPage.Name))
                {
                    TabPropsPage.Grid.SelectedObject = ex;
                    TabPropsPage.Text = "Exception: " + ex.Message;
                    this.mTabControl2DisSPObj.TabPages.Add(TabPropsPage);
                }
                else
                {
                    ((GTabPropertyPage)mTabControl2DisSPObj.TabPages[TabPropsPage.Name]).Grid.SelectedObject = ex;
                    ((GTabPropertyPage)mTabControl2DisSPObj.TabPages[TabPropsPage.Name]).Text = "Exception: " + ex.Message; 
                }
            }
        }

        private void mListView2SearchResusts_Click(object sender, EventArgs e) 
        {
            ListViewItem item = mListView2SearchResusts.SelectedItems[0];
            TabPropsPage.Grid.SelectedObject = item.Tag;
            if (!mTabControl2DisSPObj.TabPages.ContainsKey(TabPropsPage.Name))
            {
                TabPropsPage.Text = "GProps";
                this.mTabControl2DisSPObj.TabPages.Add(TabPropsPage);
            }
            else 
            {
                ((GTabPropertyPage)mTabControl2DisSPObj.TabPages[TabPropsPage.Name]).Grid.SelectedObject = item.Tag;
                ((GTabPropertyPage)mTabControl2DisSPObj.TabPages[TabPropsPage.Name]).Text = "GProps";
            }
        }
    }
}
