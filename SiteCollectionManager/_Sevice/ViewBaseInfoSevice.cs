using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SiteCollectionManager.UI;
using System.Windows.Forms;
using SiteCollectionManager._Common;
using Microsoft.SharePoint;
using System.Reflection;

namespace SiteCollectionManager._Sevice
{
    class ViewBaseInfoSevice : GSevice, IDisposable
    {
        private SPDataBox mSPDataBox = null;
        private const int WIDE_FORMAT = 15;
        private const int WIDE_FORMAT_I = 50;
        public ViewBaseInfoSevice(ProgressBarHelper prgBar, TextBox txtBox, SPDataBox box)
            : base(prgBar, txtBox)
        {
            if (box == null) 
            {
                throw new Exception("SPDataBox is null");
            }
            mSPDataBox = box;
        }

        public void Run()
        {
            RunAsyn(null, null);
        }

        public void RunAsyn(object sender, EventArgs e)
        {
            string infoText = "ErrorInfo";
            switch (mSPDataBox.Type)
            {
                case SPObjTypeEnum.Site:
                    SPSite site = (SPSite)mSPDataBox.SPObj;
                    SiteBaseInfo info = new SiteBaseInfo();
                    info.URL = site.Url;
                    info.ID = site.ID;
                    info.Owner = string.IsNullOrEmpty(site.Owner.Email) ? site.Owner.Name : site.Owner.Name + "_" + site.Owner.Email;
                    infoText = FormateObjectInfo(info) + GetAllProperties(site);
                    break;
                case SPObjTypeEnum.Web:
                    SPWeb web = (SPWeb)mSPDataBox.SPObj;
                    WebBaseInfo webInfo = new WebBaseInfo();
                    webInfo.URL = web.Url;
                    webInfo.ID = web.ID;
                    webInfo.Template = web.WebTemplate;
                    infoText = FormateObjectInfo(webInfo) + GetAllProperties(web) ;
                    break;
                case SPObjTypeEnum.List:
                    SPList list = (SPList)mSPDataBox.SPObj;
                    ListBaseInfo listInfo = new ListBaseInfo();
                    listInfo.Title = list.Title;
                    listInfo.URL = list.ParentWeb.Url.TrimEnd('/') + '/' + list.RootFolder.Url.TrimStart('/');
                    listInfo.ID = list.ID;
                    listInfo.Template = (int)list.BaseTemplate;
                    infoText = FormateObjectInfo(listInfo) + GetAllProperties(list);
                    break;
                default :
                    break;
            }
            InvokeSysProcess.StartNotepad(FileHelper.GetTempFile(infoText));
        }

        public string GetAllProperties(object obj)
        {
            StringBuilder strBuilder = null;
            try
            {
                strBuilder = new StringBuilder("\r\n*******************************All Properties**************************************\r\n");
                PropertyInfo[] perporties = obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
                string temp = string.Empty;
                foreach (PropertyInfo info in perporties)
                {
                    try
                    {
                        temp = StringWideFormat(info.Name, WIDE_FORMAT_I) + info.GetValue(obj, null) + "\r\n";
                        strBuilder.Append(temp);
                    }
                    catch 
                    { 
                    }
                }
                return strBuilder.ToString();
            }
            catch(Exception e)
            {
                return string.Empty;
            }
            finally 
            {
                strBuilder.Remove(0, strBuilder.Length);
            }
        }

        public string FormateObjectInfo(object obj)
        {
            StringBuilder strBuilder = new StringBuilder();
            PropertyInfo[] perporties = obj.GetType().GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);
            try
            {
                foreach (PropertyInfo info in perporties)
                {
                    strBuilder.Append(StringWideFormat(info.Name, WIDE_FORMAT));
                    //strBuilder.Append("--> ");
                    strBuilder.Append(info.GetValue(obj, null));
                    strBuilder.Append("\r\n");
                }
                return strBuilder.ToString();
            }
            finally
            {
                strBuilder.Remove(0, strBuilder.Length);
            }
        }

        public string StringWideFormat(string str, int w) 
        {
            int len = str.Length;
            for (int i = 0; i < w-len; ++i)
            {
                str += ' ';
            }
            return str;
        }

        public void Dispose(object sender, EventArgs e)
        {
            System.Console.WriteLine("GlobalThreadHandle invokder completed.");
            Dispose();
        }

        public void Dispose()
        {
        }
    }
}
