using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.SharePoint;
using SiteCollectionManager._Tree.LoadTreeCore;
using SiteCollectionManager._Common;
using SiteCollectionManager._Common.GException;
using SiteCollectionManager._Sevice;
using System.Threading;
namespace SiteCollectionManager.UI
{
    public partial class MainForm : Form
    {
        private string[] mCurTreeStatusStack = null;

        private TreeNode mLaseClickedTreeNode = null;

        private bool mIsCreateSeviceWorking = false;

        private CreateDataInfo mCreateSPDataInfo = new CreateDataInfo();

        private void mCheckBox2SiteCollOnly_CheckedChanged(object sender, EventArgs e)
        {
            if (mCheckBox2SiteCollOnly.Checked)
            {
                mTreeLoader.SetLoadConfig(true);
                Log("Only loading site collection.");
            }
            else
            {
                mTreeLoader.SetLoadConfig(false);
                Log("Will loading sub sites and lists.");
            }
        }

        private void mTreeViewOfMainForm_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Checked)
            {
                mLaseClickedTreeNode = e.Node;
            }
            if (mClearDataInfo != null && mClearDataInfo.mIsDeleting) 
            {
                Log("Site Collection Manager Delete Sevice Buzy...");
                return;
            }
            this.mTreeViewOfMainForm.AfterCheck -= new TreeViewEventHandler(this.mTreeViewOfMainForm_AfterCheck);
            DFSClickTreeNodes(e.Node, e.Node.Checked);
            this.mTreeViewOfMainForm.AfterCheck += new TreeViewEventHandler(this.mTreeViewOfMainForm_AfterCheck);
        }

        #region<-Refresh->
        private void mBtn2Reload_Click(object sender, EventArgs e)
        {
            mTreeViewOfMainForm.Nodes[0].Nodes.Clear();
            mTreeLoader.GenrateTree();
            Log("Reload tree completed.");
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mBtn2Reload_Click(sender, e);
        }

        private void mToolStripBtn2Refresh_Click(object sender, EventArgs e)
        {
            mBtn2Reload_Click(sender, e);
        }
        #endregion

        private void DFSClickTreeNodes(TreeNode node, bool _isCheck) 
        {
            foreach (TreeNode childNode in node.Nodes) 
            {
                childNode.Checked = _isCheck;
                DFSClickTreeNodes(childNode, _isCheck);
            }
        }

        /// <summary>
        /// 限制在只能选择一个Site Collection
        /// </summary>
        /// <param name="node">Root Node</param>
        /// <param name="selectedNum">如果返回-1即选择了多个</param>
        /// <returns></returns>
        private TreeNode GetSelectedSiteCollection(TreeNode node, ref int selectedNum)
        {
            TreeNode res = null;
            foreach (TreeNode childNode in node.Nodes)
            {
                if (childNode.Checked && childNode.Tag is SPDataBox && ((SPDataBox)childNode.Tag).Type == SPObjTypeEnum.Site)
                {

                    if (++selectedNum > 1)
                    {
                        selectedNum = -1;
                        return null;
                    }
                    res = childNode;
                    continue;
                }
                else if (childNode.Tag is SPDataBox && ((SPDataBox)childNode.Tag).Type == SPObjTypeEnum.Site)
                {
                    continue;
                }
                var temp = GetSelectedSiteCollection(childNode, ref selectedNum);
                if (temp != null)
                {
                    res = temp;
                }
                if (selectedNum == -1)
                {
                    return null;
                }
            }
            return res;
        }

        /// <summary>
        /// 至少选择一个Site Colletion
        /// </summary>
        /// <param name="node">Root node</param>
        /// <returns></returns>
        private List<TreeNode> GetSelectedSiteCollection(TreeNode node)
        {
            List<TreeNode> listRes = new List<TreeNode>();
            foreach (TreeNode childNode in node.Nodes)
            {
                if (childNode.Checked && childNode.Tag is SPDataBox && ((SPDataBox)childNode.Tag).Type == SPObjTypeEnum.Site)
                {
                    listRes.Add(childNode);
                    continue;
                }
                else if (childNode.Tag is SPDataBox && ((SPDataBox)childNode.Tag).Type == SPObjTypeEnum.Site)
                {
                    continue;
                }
                var temp = GetSelectedSiteCollection(childNode);
                if (temp != null)
                {
                    listRes.AddRange(temp);
                }
            }
            return listRes;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="selectedNum"></param>
        /// <returns></returns>
        [Obsolete("")]
        private TreeNode GetSelectedNode(TreeNode node, ref int selectedNum)
        {
            Queue<TreeNode> queNode = new Queue<TreeNode>();
            TreeNode res = null;
            foreach (TreeNode childNode in node.Nodes)
            {
                queNode.Enqueue(childNode);
                if (childNode.Checked && childNode.Tag is SPDataBox)
                {
                    if (++selectedNum > 1)
                    {
                        selectedNum = -1;
                        return null;
                    }
                    res = childNode;
                    continue;
                }
            }
            if (selectedNum == 1) 
            {
                //return res;
                return null;
            }
            //while (queNode.Count != 0) 
            //{
            //    var temp = GetSelectedSiteCollection(queNode.Dequeue(), ref selectedNum);
            //}
            
            //if (temp != null)
            //{
            //    res = temp;
            //}
            //if (selectedNum == -1)
            //{
            //    return null;
            //}
            //return res;
            return null;
        }

        #region AmenityModule

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                SPDataBox treeNodeData = (SPDataBox)mTreeViewOfMainForm.SelectedNode.Tag;
                InvokeSysProcess.OpenInExplorer(treeNodeData.Url);
                Log(string.Format("Open url {0} completed.", treeNodeData.Url));
            }
            catch (InvokerProcessException _e)
            {
                Log(string.Format("A error occurred while invoke sys process.Detail{0}", _e.ToString()));
            }
            catch (Exception ex) 
            {
                Log(string.Format("A error occurred while open sharepoint url.Detail{0}", ex.ToString()));
            }
        }

        private void openToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.openToolStripMenuItem_Click(sender, e);
        }

        private void copyUrlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                SPDataBox treeNodeData = (SPDataBox)mTreeViewOfMainForm.SelectedNode.Tag;
                InvokeSysProcess.CoypeUrlToClipboard(treeNodeData.Url);
                Log(string.Format("Clipboard:{0} {1}", treeNodeData.Url, DateTime.Now.ToString()));
            }
            catch (InvokerProcessException _e)
            {
                Log(string.Format("A error occurred while invoke sys process in copyUrlToolStripMenuItem_Click.Detail{0}", _e.ToString()));
            }
            catch (Exception ex)
            {
                Log(string.Format("A error occurred while copy sharepoint url in copyUrlToolStripMenuItem_Click.Detail{0}", ex.ToString()));
            }
        }

        private void copyeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            copyUrlToolStripMenuItem_Click(sender, e);
        }

        #endregion

        #region <-ClearSPDataSevice->

        private void DFSCheckedSPObj(TreeNode node)
        {
            SPDataBox box = null;
            if (node.Checked)
            {
                if (node.Tag is SPDataBox)
                {
                    box = (SPDataBox)node.Tag;
                    if (box.CanDelete)
                    {
                        this.mClearDataInfo.mDeletingObjs.Add(box);
                        box.MarkAsDelete = true;
                        ++mClearDataInfo.mNeedDeleteNum;
                    }
                    else
                    {
                        //node.Checked = false;
                        Log(string.Format("SPData: {0} cannot be deleted.", box.Url));
                    }
                    return;
                }
            }

            foreach (TreeNode t in node.Nodes)
            {
                DFSCheckedSPObj(t);
            }
        }

        private void mBtn2Dele_Click(object sender, EventArgs e)
        {
            if (mClearDataInfo != null && mClearDataInfo.mIsDeleting)
            {
                Log("DeleteSevice Buzy...");
                return;
            }
            ProgressBarHelper progBarHelper = null;
            try
            {
                mClearDataInfo = new NeedClearDataInfo();
                mClearDataInfo.mNeedDeleteNum = 0;
                mClearDataInfo.mDeletedNum = 0;
                mClearDataInfo.mIsDeleting = true;
                DFSCheckedSPObj(mTreeViewOfMainForm.Nodes[0]);
                {
                    if (mClearDataInfo.mNeedDeleteNum == 0)
                    {
                        Log("You have not selected deletable sharepoint data on tree.");
                        mClearDataInfo.mIsDeleting = false;
                        return;
                    }
                }
                progBarHelper = new ProgressBarHelper(mProgressBar, mClearDataInfo.mNeedDeleteNum);
                //progBarHelper.InitProcessBar(mProgressBar, mClearDataInfo.mNeedDeleteNum);
                using (ClearSPDataSevice sevice = new ClearSPDataSevice(progBarHelper, mTextBox2Message, mClearDataInfo))
                {
                    sevice.Run();
                }
                RemoveCheckedNode(mTreeViewOfMainForm.Nodes[0]);
            }
            catch (ClearSeviceException clearEx)
            {
                string eStr = string.Format("A error occurred while delete sharepoint objs.  detail:{0}", clearEx.ToString());
                Log(eStr);
                RemoveCheckedNode(mTreeViewOfMainForm.Nodes[0]);
            }
            catch (Exception ex)
            {
                string eStr = string.Format("A error occurred while delete sharepoint data.  detail:{0}", ex.ToString());
                Log(eStr);
            }
            finally 
            {
                if(progBarHelper != null)
                {
                    progBarHelper.ExceptionEndProgress();
                }
            }
        }

        private void deleteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            mBtn2Dele_Click(sender, e);
        }

        private void RemoveCheckedNode(TreeNode node) 
        {
            if (node.Checked && node.Tag is SPDataBox && ((SPDataBox)node.Tag).MarkAsDelete && ((SPDataBox)node.Tag).IsDirity) 
            {
                RemoveSubCheckedNode(node);
                node.Remove();
            }
            for (int i = node.Nodes.Count - 1; i >= 0; --i) 
            {
                RemoveCheckedNode(node.Nodes[i]);
            }
        }

        private void DmToolStandBtn2Delete_Click(object sender, EventArgs e)
        {
            mBtn2Dele_Click(sender, e);
        }

        /// <summary>
        /// 删除Tree上的节点
        /// </summary>
        /// <param name="node"></param>
        /// <param name="_index">_index == 0 代表被选中的顶层节点</param>
        private void RemoveSubCheckedNode(TreeNode node) 
        {
            for (int i = node.Nodes.Count-1; i >= 0; -- i)
            {
                RemoveSubCheckedNode(node.Nodes[i]);
                node.Nodes[i].Remove();
                //node.Nodes[i].Remove();
            }
        }
        #endregion

        #region <-Backup&RestoreSevice->

        private void mBtn2Backup_Click(object sender, EventArgs e)
        {
            //GetUrl.
            //int _isInvaild = 0;
            //TreeNode selectedNode = GetSelectedSiteCollection(mTreeViewOfMainForm.Nodes[0], ref _isInvaild);
            //_isInvaild = 0;
            List<TreeNode> selectedNodes = GetSelectedSiteCollection(mTreeViewOfMainForm.Nodes[0]);
            List<BRInfo> bakInfos = new List<BRInfo>();
            if (selectedNodes.Count == 0) 
            {
                Log("You must select at least one site collection node.");
                return;
            }
            string fileLocation = DialogHelper.OpenFolderLocation(null, null);
            if (string.IsNullOrEmpty(fileLocation))
            {
                Log("You must select backup data folder.");
                return;
            }
            foreach (TreeNode selectedNode in selectedNodes)
            {
                SPDataBox box = (SPDataBox)selectedNode.Tag;
                //Init
                BRInfo info = new BRInfo();
                info.FileLocation = fileLocation;
                info.SiteUrl = box.Url;
                bakInfos.Add(info);
            }
            ProgressBarHelper progBarHelper = null;
            GlobalThreadHandle threadHandle = null;
            try
            {
                progBarHelper = new ProgressBarHelper(mProgressBar, bakInfos.Count);
                BackupSevice backup = new BackupSevice(progBarHelper, mTextBox2Message, bakInfos);
                threadHandle = new GlobalThreadHandle(typeof(BackupSevice).FullName, backup.RunAsyn, backup.Dispose);
                {
                    threadHandle.Start();
                }
                //Log(string.Format("Backup site collection num: {0} completed.", bakInfos.Count));
            }
            catch (Exception ex)
            {
                Log(string.Format("A error occurred while backup site collection. Detail:{0}",ex.ToString()));
            }
            finally 
            {
                if (progBarHelper != null && threadHandle != null && !threadHandle.IsBusy)
                {
                    progBarHelper.EndProgress();
                }
            }
        }

        private void mBtn2Restore_Click(object sender, EventArgs e)
        {
            ProgressBarHelper progBarHelper = null;
            try
            {
                string fileName = DialogHelper.OpenFileLocation(null, null);
                if (string.IsNullOrEmpty(fileName))
                {
                    Log("Please select backup data.");
                    return;
                }
                progBarHelper = new ProgressBarHelper(mProgressBar, 3);
                RestoreSevice restore = new RestoreSevice(progBarHelper, mTextBox2Message, fileName);
                GlobalThreadHandle gThreadHandle = new GlobalThreadHandle(typeof(RestoreSevice).FullName, restore.RunAsyn, restore.Dispose); 
                {
                    gThreadHandle.Start();
                }
            }
            catch
            {
            }
            finally 
            {
            }
        }

        private void backupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mBtn2Backup_Click(sender, e);
        }

        private void mToolStripButton2BackupSevice_Click(object sender, EventArgs e)
        {
            mBtn2Backup_Click(sender, e);
        }

        private void restoreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mBtn2Restore_Click(sender, e);
        }

        private void mToolStripButton2RestoreSevice_Click(object sender, EventArgs e)
        {
            mBtn2Restore_Click(sender, e);
        }

        #endregion

        #region<-Delete SPData with backup->
        private void mBtn2DeleWithBak_Click(object sender, EventArgs e)
        {
            List<TreeNode> selectedNodes = GetSelectedSiteCollection(mTreeViewOfMainForm.Nodes[0]);
            List<BRInfo> bakInfos = new List<BRInfo>();
            if (selectedNodes.Count == 0)
            {
                Log("You must select at least one site collection node.");
                return;
            }
            string fileLocation = DialogHelper.OpenFolderLocation(null, null);
            if (string.IsNullOrEmpty(fileLocation))
            {
                Log("You must select backup data folder.");
                return;
            }
            foreach (TreeNode selectedNode in selectedNodes)
            {
                SPDataBox box = (SPDataBox)selectedNode.Tag;
                //Init
                BRInfo info = new BRInfo();
                info.FileLocation = fileLocation;
                info.SiteUrl = box.Url;
                bakInfos.Add(info);
            }

            #region backup sevice init.
            //if (mClearDataInfo != null && mClearDataInfo.mIsDeleting)
            //{
            //    Log("DeleteSevice Buzy...");
            //    return;
            //}
            //if (mClearDataInfo != null)
            //{
            //    mClearDataInfo.mDeletingObjs.Clear();
            //    mClearDataInfo.mNeedDeleteNum = selectedNodes.Count;
            //    mClearDataInfo.mDeletedNum = 0;
            //    mClearDataInfo.mIsDeleting = true;
            //}
            //else
            //{
            //    mClearDataInfo = new NeedClearDataInfo();
            //    mClearDataInfo.mNeedDeleteNum = selectedNodes.Count;
            //    mClearDataInfo.mDeletedNum = 0;
            //    mClearDataInfo.mIsDeleting = true;
            //}
            //mClearDataInfo.mDeletingObjs.AddRange(selectedNodes.Select(dto => (SPDataBox)dto.Tag));
            #endregion

            ProgressBarHelper progBarHelper = new ProgressBarHelper(mProgressBar, bakInfos.Count);
            GlobalThreadHandle threadHandle = null;
            try
            {
                BackupSevice backup = new BackupSevice(progBarHelper, mTextBox2Message, bakInfos);
                //ClearSPDataSevice sevice = new ClearSPDataSevice(progBarHelper, mTextBox2Message, mClearDataInfo);
                threadHandle = new GlobalThreadHandle(typeof(BackupSevice).FullName, backup.RunAsyn, this.mBtn2Dele_Click);
                {
                    threadHandle.Start();
                }
            }
            catch (Exception ex)
            {
                Log(string.Format("A error occurred while delete with backup site collection. Detail:{0}", ex.ToString()));
            }
            finally
            {
                if (progBarHelper != null && threadHandle != null && !threadHandle.IsBusy)
                {
                    progBarHelper.EndProgress();
                }
            }
        }

        private void deleteWithBackupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mBtn2DeleWithBak_Click(sender, e);
        }

        private void mToolStripButton2DeleteWiteBackup_Click(object sender, EventArgs e)
        {
            mBtn2DeleWithBak_Click(sender, e);
        }
        #endregion

        #region<-View SPData BaseInfo->

        private void mBtn2ViewBaseInfo_Click(object sender, EventArgs e)
        {
            int _index = 0;
            //TreeNode node = GetSelectedSiteCollection(mTreeViewOfMainForm.Nodes[0], ref _index);
            TreeNode node = mLaseClickedTreeNode;
            if (node == null || _index == -1)
            {
                Log("You must select one site collection node.");
                return;
            }
            Log(string.Format("View Base Info: Your last selected tree node is {0}.", node.Text));
            ProgressBarHelper progBarHelper = new ProgressBarHelper(mProgressBar, 1);
            try
            {
                using (ViewBaseInfoSevice sevice = new ViewBaseInfoSevice(progBarHelper, mTextBox2Message, node.Tag as SPDataBox))
                {
                    sevice.Run();
                }
            }
            catch (Exception ex)
            {
                Log(string.Format("A error occurred while view spdata base information. Detail:{0}", ex.ToString()));
            }
            finally
            {
                if (progBarHelper != null)
                {
                    progBarHelper.EndProgress();
                }
            }
        }

        private void viewBaseInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mBtn2ViewBaseInfo_Click(sender, e);
        }

        private void mToolStripButton2ExportProps_Click(object sender, EventArgs e)
        {
            mBtn2ViewBaseInfo_Click(sender, e);
        }
        #endregion

        #region<-Create SPdata Sevice->

        private void mBtn2CreateModule_Click(object sender, EventArgs e)
        {
            if (mCreateSPDataInfo.mIsCreateSeviceWorking) 
            {
                Log("Create Sevice Buzy...");
                return;
            }
            mCreateSPDataInfo.mIsCreateSeviceWorking = true;

            GlobalThreadHandle threadHandle = null;
            ProgressBarHelper progBarHelper = new ProgressBarHelper(mProgressBar, 1);
            try
            {
                CreateSeviceForm form = new CreateSeviceForm();
                form.ShowDialog();
                if (!form.AllowCreate)
                {
                    Log("No site collection created.");
                    mCreateSPDataInfo.mIsCreateSeviceWorking = false;
                }
                else
                {
                    mCreateSPDataInfo.mCreateNumb = form.Num;
                    mCreateSPDataInfo.mCreateTitle = form.Titie;
                    mCreateSPDataInfo.mUrl = form.Url;
                    mCreateSPDataInfo.mWebApp = form.WebApp;

                    CreateSPDataService sevice = new CreateSPDataService(progBarHelper, mTextBox2Message, mCreateSPDataInfo);
                    threadHandle = new GlobalThreadHandle(typeof(CreateSPDataService).FullName, sevice.RunAsyn, sevice.Dispose);
                    {
                        threadHandle.Start();
                    }
                }
            }
            catch (Exception ex)
            {
                Log(string.Format("A error occurred while create spdata. Detail:{0}", ex.ToString()));
            }
            finally
            {
                if (progBarHelper != null && threadHandle != null && !threadHandle.IsBusy)
                {
                    progBarHelper.EndProgress();
                }
            }
        }

        private void createToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mBtn2CreateModule_Click(sender, e);
        }

        private void mToolStripButton2CreateSevice_Click(object sender, EventArgs e)
        {
            mBtn2CreateModule_Click(sender, e);
        }
        #endregion

        #region<-Search Sevice->
        private void mBtn2SearchModule_Click(object sender, EventArgs e)
        {
            try
            {

                int _index = 0;
                TreeNode node = mLaseClickedTreeNode;
                if (node == null || _index == -1)
                {
                    Log("You must select one site collection node.");
                    return;
                }
                Log(string.Format("Search: Your last selected tree node is {0}.", node.Text));
                if (mLaseClickedTreeNode.Tag is SPDataBox)
                {
                    SearchSeviceForm search = SingleProvider<SearchSeviceForm>.Instance;
                    SPDataBox box = (SPDataBox)mLaseClickedTreeNode.Tag;
                    if (box.Type == SPObjTypeEnum.Site || box.Type == SPObjTypeEnum.Web)
                    {
                        search.Text = "Search Scope: " + box.Url;
                        search.SearchScopeObj = box;
                    }
                    else
                    {
                        Log("In current version scoll manager can only search in site collection/web level.");
                        return;
                    }
                    search.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                Log(string.Format("A error occurred while search sharepoint data. Detail: {0}", ex.ToString()));
            }
        }

        private void searchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mBtn2SearchModule_Click(sender, e);
        }

        private void mToolStripBtn2SearchSevice_Click(object sender, EventArgs e)
        {
            mBtn2SearchModule_Click(sender, e);
        }
        #endregion

        #region<-All props->
        /// <summary>
        /// context tool bar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GPropsForm gprops = SingleProvider<GPropsForm>.Instance;
            try
            {
                if (mTreeViewOfMainForm.SelectedNode.Tag is SPDataBox)
                {
                    gprops.Show(((SPDataBox)mTreeViewOfMainForm.SelectedNode.Tag).SPObj, ((SPDataBox)mTreeViewOfMainForm.SelectedNode.Tag).Url);
                }
                else 
                {
                    gprops.Show(new object(), "SPCollection");
                }
            }
            catch(Exception ex)
            {
                Log(ex.ToString());
            }
        }

        /// <summary>
        /// from menu tool bar.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void propertiesToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            mToolStripButton2ViewAllProperties_Click(sender, e);
        }

        private void mToolStripButton2ViewAllProperties_Click(object sender, EventArgs e)
        {
            //propertiesToolStripMenuItem_Click(sender, e);
            try
            {
                TreeNode node = mLaseClickedTreeNode;
                if (node == null)
                {
                    Log("You must check one node.");
                    return;
                }
                Log(string.Format("Search: Your last checked tree node is {0}.", node.Text));

                GPropsForm gprops = SingleProvider<GPropsForm>.Instance;
                if (node.Tag is SPDataBox)
                {
                    gprops.Show(((SPDataBox)node.Tag).SPObj, ((SPDataBox)node.Tag).Url);
                }
                else
                {
                    gprops.Show(new object(), "SPCollection");
                }
            }
            catch (Exception ex)
            {
                Log(string.Format("A error occurred while search sharepoint data. Detail: {0}", ex.ToString()));
            }
        }

        private void mBtn2ViewAllProps_Click(object sender, EventArgs e)
        {
            mToolStripButton2ViewAllProperties_Click(sender, e);
        }
        #endregion

        #region <-About Form->
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                AboutForm about = SingleProvider<AboutForm>.Instance;
                about.ShowDialog();
            }
            catch (Exception ex)
            {
                Log(ex.ToString());
            }
        }
        #endregion
    }
}