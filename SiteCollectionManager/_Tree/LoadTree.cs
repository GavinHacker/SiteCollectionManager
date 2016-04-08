using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.SharePoint.Navigation;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint;
using System.Collections;
using System.Threading;
using Microsoft.SharePoint.Workflow;
using SiteCollectionManager._Common;


namespace SiteCollectionManager._Tree.LoadTreeCore
{
    /// <summary>
    /// Cfg of load tree.
    /// </summary>
    public class TreeLoadConfig
    {
        public bool IsLoadNavigation = true;
        public bool IsLoadWeb = true;
        public bool IsLoadList = true;
        public bool IsLoadContentType = true;
        public bool IsLoadItem = true;
        public bool IsLoadWorkflowAssociations = true;
    }
    /// <summary>
    /// In site collection manager, have no config file now.
    /// </summary>
    public class LoadTreeHelper
    {
        public static TreeLoadConfig ReadConfig(TreeLoadConfig config)
        {
            //IDictionary reportSetting;
            config.IsLoadNavigation = IsLoadCheck("Navigation");
            config.IsLoadWeb = IsLoadCheck("Web");
            config.IsLoadList = IsLoadCheck("List");
            config.IsLoadItem = IsLoadCheck("Item");
            config.IsLoadContentType = IsLoadCheck("CT");
            return config;
        }
        private static bool IsLoadCheck(string NodeTitle)
        {
            //IDictionary reportSetting;
            //string type = string.Empty;
            //reportSetting = System.Configuration.ConfigurationManager.GetSection(NodeTitle)
            //                as System.Collections.IDictionary;
            //type = reportSetting["isLoad"].ToString();
            //return type.Equals("true") ? true : false;
            if (NodeTitle.Equals("List", StringComparison.OrdinalIgnoreCase) || NodeTitle.Equals("Web", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            return false;
        }
    }

    public class TreeLoader
    {
        private TreeLoadConfig mLoadTreeConfig = null;
        public TreeLoadConfig LoadTreeConfig
        {
            get
            {
                if (mLoadTreeConfig == null)
                {
                    mLoadTreeConfig = new TreeLoadConfig();
                    mLoadTreeConfig = LoadTreeHelper.ReadConfig(mLoadTreeConfig);
                    return mLoadTreeConfig;
                }
                return mLoadTreeConfig;
            }
        }
        private TreeView mTreeView;
        private Thread mTempThreadHandle;
        private TreeNode mCurrentSelectedNode = null;
        private TextBox mMessageBox = null;

        public TreeLoader(TreeView treeView, bool siteCollctionOnly)
        {
            this.mTreeView = treeView;
            if (!siteCollctionOnly)
            {
                this.mTreeView.AfterSelect += new TreeViewEventHandler(treeView_AfterSelect);
            }
        }

        public bool SetLoadConfig(bool siteCollectionOnly) 
        {
            if (siteCollectionOnly)
            {
                mTreeView.AfterSelect -= new TreeViewEventHandler(treeView_AfterSelect);
            }
            else 
            {
                mTreeView.AfterSelect += new TreeViewEventHandler(treeView_AfterSelect);
            }
            return true;
        }

        public bool SetLoadConfig(TreeLoadConfig cfg)
        {
            return false;
        }

        public void GenrateTree()
        {
            //AddTreeNode(treeView.Nodes[0], new TreeNode("ddd"));
            if (mTreeView.Nodes.Count == 0) 
            {
                mTreeView.Nodes.Add("SharePoint");
            }
            LoadWebApps(mTreeView.Nodes[0]);
        }

        public void SetTreeViewContorl(TreeView treeView)
        {
            this.mTreeView = treeView;
            this.mTreeView.AfterSelect += new TreeViewEventHandler(treeView_AfterSelect);
        }

        public void SetMessageBoxControl(TextBox txtbox)
        {
            this.mMessageBox = txtbox;
        }

        private void LoadWebApps(TreeNode ptNode)
        {
            foreach (SPWebApplication spWebApp in SPWebService.ContentService.WebApplications)
            {
                TreeNode webAppNode = new TreeNode();
                webAppNode.Text = spWebApp.AlternateUrls.GetResponseUrl(SPUrlZone.Default).Uri.ToString();

                SPDataBox nodeData = new SPDataBox();
                nodeData.Type = SPObjTypeEnum.WebApp;
                nodeData.SPObj = spWebApp;

                webAppNode.Tag = nodeData;

                AddTreeNode(ptNode, webAppNode);
                LoadSiteCollections(webAppNode);
            }
        }

        private void LoadSiteCollections(TreeNode ptNode)
        {
            SPDataBox ptNodeData = (SPDataBox)ptNode.Tag;
            SPWebApplication webApp = ptNodeData.SPObj as SPWebApplication;

            foreach (SPSite site in webApp.Sites)
            {
                TreeNode siteNode = new TreeNode();
                siteNode.Text = site.Url;

                SPDataBox nodeData = new SPDataBox();
                nodeData.Type = SPObjTypeEnum.Site;
                nodeData.SPObj = site;
                siteNode.Tag = nodeData;

                AddTreeNode(ptNode, siteNode);
                if (LoadTreeConfig.IsLoadNavigation)
                {
                    LoadNavigations(site.RootWeb.Navigation.GlobalNodes, siteNode);
                }
                if (LoadTreeConfig.IsLoadWeb)
                {
                    //LoadRootWeb(siteNode);
                }

            }
        }

        private void LoadRootWeb()
        {
            TreeNode ptNode = mCurrentSelectedNode;
            SPDataBox ptNodeData = (SPDataBox)ptNode.Tag;
            SPSite pSite = ptNodeData.SPObj as SPSite;
            SPWeb rootWeb = pSite.RootWeb;

            TreeNode chNode = new TreeNode();
            chNode.Text = rootWeb.Title;

            SPDataBox treeNodeData = new SPDataBox();
            treeNodeData.Type = SPObjTypeEnum.Web;
            treeNodeData.SPObj = rootWeb;
            chNode.Tag = treeNodeData;

            AddTreeNode(ptNode, chNode);

            TreeNode tempLists = new TreeNode("Lists");
            AddTreeNode(chNode, tempLists);
            TreeNode tempSites = new TreeNode("Sites");
            AddTreeNode(chNode, tempSites);
            //TreeNode tempCTs = new TreeNode("ContentTypes");
            //AddTreeNode(chNode, tempCTs);

            if (LoadTreeConfig.IsLoadNavigation) { }
            if (LoadTreeConfig.IsLoadList)
            {
                LoadLists(rootWeb.Lists, tempLists);
            }
            if (LoadTreeConfig.IsLoadWeb)
            {
                LoadWebs(rootWeb.Webs, tempSites);
            }
            //if (LoadTreeConfig.IsLoadContentType)
            //{
            //    LoadContentType(rootWeb.ContentTypes, tempCTs);
            //}
            AppendTxtFunc(this.mMessageBox, string.Format("==> Load site collection completed. url:{0} ", pSite.ServerRelativeUrl));
        }

        private void LoadWebs(SPWebCollection webs, TreeNode parentNode)
        {
            foreach (SPWeb spWeb in webs)
            {
                TreeNode chNode = new TreeNode();
                chNode.Text = spWeb.Title;

                SPDataBox nodeData = new SPDataBox();
                nodeData.Type = SPObjTypeEnum.Web;
                nodeData.SPObj = spWeb;
                chNode.Tag = nodeData;

                AddTreeNode(parentNode, chNode);

                TreeNode tempLists = new TreeNode("Lists");
                AddTreeNode(chNode, tempLists);
                TreeNode tempSites = new TreeNode("Sites");
                AddTreeNode(chNode, tempSites);
                //TreeNode tempCTs = new TreeNode("ContentTypes");
                //AddTreeNode(chNode, tempCTs);

                if (LoadTreeConfig.IsLoadNavigation) { }
                if (LoadTreeConfig.IsLoadList)
                {
                    LoadLists(spWeb.Lists, tempLists);
                }
                if (LoadTreeConfig.IsLoadWeb)
                {
                    LoadWebs(spWeb.Webs, tempSites);
                }
                //if (LoadTreeConfig.IsLoadContentType)
                //{
                //    LoadContentType(spWeb.ContentTypes, tempCTs);
                //}
            }
        }

        private void LoadLists(SPListCollection lists, TreeNode parentNode)
        {
            foreach (SPList list in lists)
            {
                TreeNode chNode = new TreeNode();
                chNode.Text = list.Title;
                //if (list.WorkflowAssociations.Count == 0)
                //{
                //    chNode.BackColor = System.Drawing.Color.DarkGray;
                //}

                SPDataBox nodeData = new SPDataBox();
                nodeData.Type = SPObjTypeEnum.List;
                nodeData.SPObj = list;
                chNode.Tag = nodeData;

                AddTreeNode(parentNode, chNode);

                //TreeNode tempItems = new TreeNode("Items");
                //AddTreeNode(chNode, tempItems);
                //TreeNode tempCTs = new TreeNode("ContentTypes");
                //AddTreeNode(chNode, tempCTs);
                //TreeNode tempWFAsso = new TreeNode("WorkflowAssociations");
                //AddTreeNode(chNode, tempWFAsso);

                //if (LoadTreeConfig.IsLoadContentType)
                //{
                //    LoadContentType(list.ContentTypes, tempCTs);
                //}
                //if (LoadTreeConfig.IsLoadItem)
                //{
                //    LoadItems(list.Items, tempItems);
                //}
                //if (LoadTreeConfig.IsLoadWorkflowAssociations)
                //{
                //    LoadWorkflowAssociations(list.WorkflowAssociations, tempWFAsso);
                //}
            }
        }

        private void LoadContentType(SPContentTypeCollection cts, TreeNode parentNode)
        {
            foreach (SPContentType ct in cts)
            {
                //if (ct.WorkflowAssociations.Count == 0) 
                //{
                //    continue;
                //}
                TreeNode chNode = new TreeNode();
                chNode.Text = ct.Name;
                if (ct.WorkflowAssociations.Count == 0)
                {
                    //continue;
                    chNode.BackColor = System.Drawing.Color.DarkGray;
                }

                SPDataBox nodeData = new SPDataBox();
                nodeData.Type = SPObjTypeEnum.ContentType;
                nodeData.SPObj = ct;
                chNode.Tag = nodeData;

                AddTreeNode(parentNode, chNode);

                TreeNode tempWFAsso = new TreeNode("WorkflowAssociations");
                AddTreeNode(chNode, tempWFAsso);

                if (LoadTreeConfig.IsLoadWorkflowAssociations)
                {
                    LoadWorkflowAssociations(ct.WorkflowAssociations, tempWFAsso);
                }
                //nodes.Add(chNode);
            }
        }

        private void LoadItems(SPListItemCollection items, TreeNode parentNode)
        {
            foreach (SPListItem item in items)
            {
                TreeNode chNode = new TreeNode();
                if (item.File == null)
                {
                    chNode.Text = item.Title;
                }
                else
                {
                    chNode.Text = item.Name;
                }
                if (item.Workflows.Count == 0)
                {
                    chNode.BackColor = System.Drawing.Color.DarkGray;
                }
                SPDataBox nodeData = new SPDataBox();
                nodeData.Type = SPObjTypeEnum.Item;
                nodeData.SPObj = item;
                chNode.Tag = nodeData;

                AddTreeNode(parentNode, chNode);
            }

        }

        private void LoadNavigations(SPNavigationNodeCollection navNodes, TreeNode parentNode)
        {
            foreach (SPNavigationNode spNav in navNodes)
            {
                TreeNode chNode = new TreeNode();
                chNode.Text = spNav.Title;

                SPDataBox nodeData = new SPDataBox();
                nodeData.Type = SPObjTypeEnum.NavNode;
                nodeData.SPObj = spNav;

                chNode.Tag = nodeData;

                AddTreeNode(parentNode, chNode);
                LoadNavigations(spNav.Children, chNode);
            }
        }

        private void LoadWorkflowAssociations(SPWorkflowAssociationCollection assos, TreeNode parentNode)
        {
            foreach (SPWorkflowAssociation wfAsso in assos)
            {
                TreeNode chNode = new TreeNode();
                chNode.Text = wfAsso.Name;

                SPDataBox nodeData = new SPDataBox();
                nodeData.Type = SPObjTypeEnum.WorkflowAssociation;
                nodeData.SPObj = wfAsso;

                chNode.Tag = nodeData;

                AddTreeNode(parentNode, chNode);
            }
        }

        public void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            SPDataBox nodeData = (SPDataBox)e.Node.Tag;
            if (nodeData == null)
            {
                return;
            }
            if (nodeData.Type == SPObjTypeEnum.Site)
            {
                //SPList list = (SPList)nodeData.SPObj;
                //LoadContentType(list.ContentTypes, e.Node.Nodes);
                if (e.Node.Nodes.Count != 0)
                {
                    return;
                }
                if (mTempThreadHandle != null && mTempThreadHandle.ThreadState == ThreadState.Running)
                {
                    AppendTxtFunc(this.mMessageBox, "==> LoadTreeBusy...");
                    return;
                }
                mCurrentSelectedNode = e.Node;
                //线程
                //mTempThreadHandle = new Thread(new ThreadStart(LoadRootWeb));
                //mTempThreadHandle.Start();
                LoadRootWeb();
            }
        }

        public void ReloadSite()
        {
            SPDataBox nodeData = (SPDataBox)mTreeView.SelectedNode.Tag;
            if (nodeData == null)
            {
                return;
            }
            if (nodeData.Type == SPObjTypeEnum.Site)
            {
                if (mTempThreadHandle != null && mTempThreadHandle.ThreadState == ThreadState.Running)
                {
                    AppendTxtFunc(this.mMessageBox, "==> LoadTreeBusy...");
                    return;
                }
                try
                {
                    SPSite site = (SPSite)nodeData.SPObj;
                    string url = site.Url;
                    site.Dispose();
                    site = new SPSite(url);
                    nodeData.SPObj = site;

                    mCurrentSelectedNode = this.mTreeView.SelectedNode;
                    mCurrentSelectedNode.Tag = nodeData;
                    mCurrentSelectedNode.Nodes.Clear();
                    //mTempThreadHandle = new Thread(new ThreadStart(LoadRootWeb));
                    //mTempThreadHandle.Start();
                    LoadRootWeb();
                }
                catch (Exception e)
                {
                    AppendTxtFunc(this.mMessageBox, "==> " + e.Message + "\r\n" + e.StackTrace + "\r\n");
                    return;
                }
            }
        }

        #region<-Delegate->

        delegate void AddTo(TreeNode parentNode, TreeNode childNode);
        public void AddTreeNode(TreeNode parentNode, TreeNode childNode)
        {
            if (mTreeView.FindForm().InvokeRequired)
            {
                mTreeView.FindForm().Invoke(new AddTo(AddTreeNode), new object[] { parentNode, childNode });
            }
            else
            {
                parentNode.Nodes.Add(childNode);
            }
        }

        delegate void TreeViewAddHandler(TreeView treeView, TreeViewCancelEventHandler treeView_AfterSelect);
        public void AddHandler(TreeView treeView, TreeViewCancelEventHandler handler)
        {
            if (mTreeView.FindForm().InvokeRequired)
            {
                mTreeView.FindForm().Invoke(new TreeViewAddHandler(AddHandler), new object[] { treeView, handler });
            }
            else
            {
                treeView.AfterSelect += treeView_AfterSelect;
            }
        }

        delegate void AppendTxt(TextBox messageBox, string str);
        public void AppendTxtFunc(TextBox messageBox, string str)
        {
            if (mMessageBox.FindForm().InvokeRequired)
            {
                mMessageBox.FindForm().Invoke(new AppendTxt(AppendTxtFunc), new object[] { messageBox, str });
            }
            else
            {
                messageBox.AppendText(string.Format(str + " {0} \r\n", DateTime.Now.ToString()));
            }
        }
        #endregion
    }
}
