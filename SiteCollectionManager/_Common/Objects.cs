using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using SiteCollectionManager._Common.GException;

namespace SiteCollectionManager._Common
{
    public enum SPObjTypeEnum
    {
        WebApp,
        Site,
        Web,
        List,
        Item,
        ContentType,
        Field,
        EventReciver,
        View,
        Feature,
        NavNode,
        WorkflowAssociation,
        Invalid
    }

    internal class SPDataBox
    {
        public SPObjTypeEnum Type;
        public object SPObj;
        private bool mIsDirty = false;
        public bool IsDirity 
        {
            get 
            {
                return mIsDirty;
            }
        }

        private SPSite mParentSite = null;
        public SPSite ParentSite 
        {
            get
            {
                if (mParentSite == null)
                {
                    //var url = string.Empty;
                    switch (Type)
                    {
                        case SPObjTypeEnum.Site:
                            {
                               mParentSite = (SPSite)SPObj;
                               break;
                            }
                        case SPObjTypeEnum.Web:
                            {
                                SPWeb web = (SPWeb)SPObj;
                                mParentSite = web.Site;
                                break;
                            }
                        case SPObjTypeEnum.List:
                            {
                                SPList list = (SPList)SPObj;
                                mParentSite = list.ParentWeb.Site;
                                break;
                            }
                        case SPObjTypeEnum.Field:
                            {
                                SPField field = (SPField)SPObj;
                                mParentSite = field.ParentList.ParentWeb.Site;
                                break;
                            }

                        default:
                            {
                                break;
                            }
                    }
                }
                return mParentSite;
            }
        }

        private SPObjTypeEnum mParentType = SPObjTypeEnum.Invalid;
        public SPObjTypeEnum ParentType
        {
            get
            {
                if (mParentType == SPObjTypeEnum.Invalid)
                {
                    //var url = string.Empty;
                    switch (Type)
                    {
                        case SPObjTypeEnum.Site:
                            {
                                mParentType = SPObjTypeEnum.WebApp;
                                break;
                            }
                        case SPObjTypeEnum.Web:
                            {
                                mParentType = SPObjTypeEnum.Site;
                                break;
                            }
                        case SPObjTypeEnum.List:
                            {
                                mParentType = SPObjTypeEnum.Web;
                                break;
                            }
                        case SPObjTypeEnum.Field:
                            {
                                break;
                            }
                        default:
                            {
                                break;
                            }
                    }
                }
                return mParentType;
            }
            set 
            {
                mParentType = value;
            }
        }

        private string mUrl = string.Empty;
        public string Url
        {
            get
            {
                if (string.IsNullOrEmpty(mUrl))
                {
                    //var url = string.Empty;
                    switch (Type)
                    {
                        case SPObjTypeEnum.Site:
                            {
                                SPSite site = (SPSite)SPObj;
                                mUrl = site.Url;
                                break;
                            }
                        case SPObjTypeEnum.Web:
                            {
                                SPWeb web = (SPWeb)SPObj;
                                mUrl = web.Url;
                                break;
                            }
                        case SPObjTypeEnum.List:
                            {
                                SPList list = (SPList)SPObj;
                                mUrl = list.ParentWeb.Url.TrimEnd('/') + '/' + list.RootFolder.Url.TrimStart('/');
                                break;
                            }
                        case SPObjTypeEnum.Field:
                            {
                                SPField field = (SPField)SPObj;
                                mUrl = string.Empty;
                                break;
                            }
                        default:
                            {
                                break;
                            }
                    }
                }
                return mUrl;
            }
        }

        private string mTitle = string.Empty;
        public string Title 
        {
            get
            {
                if (string.IsNullOrEmpty(mTitle))
                {
                    //var url = string.Empty;
                    switch (Type)
                    {
                        case SPObjTypeEnum.Site:
                            {
                                SPSite site = (SPSite)SPObj;
                                mTitle = site.RootWeb.Title;
                                break;
                            }
                        case SPObjTypeEnum.Web:
                            {
                                SPWeb web = (SPWeb)SPObj;
                                mTitle = web.Title;
                                break;
                            }
                        case SPObjTypeEnum.List:
                            {
                                SPList list = (SPList)SPObj;
                                mTitle = list.Title;
                                break;
                            }
                        case SPObjTypeEnum.Field:
                            {
                                SPField field = (SPField)SPObj;
                                mTitle = field.Title;
                                break;
                            }

                        default:
                            {
                                break;
                            }
                    }
                }
                return mTitle;
            }
        }

        private bool mMarkAsDelte = false;
        public bool MarkAsDelete
        {
            get;
            set;
        }

        public bool CanDelete
        {
            get 
            {
                if (Type != SPObjTypeEnum.Site && Type != SPObjTypeEnum.Web && Type != SPObjTypeEnum.List) 
                {
                    return false;
                }
                switch (Type)
                {
                    case SPObjTypeEnum.Site:
                        {
                            //SPSite site = (SPSite)SPObj;
                            break;
                        }
                    case SPObjTypeEnum.Web:
                        {
                            SPWeb web = (SPWeb)SPObj;                            
                            return !web.IsRootWeb;
                        }
                    case SPObjTypeEnum.List:
                        {
                            SPList list = (SPList)SPObj;
                            return list.AllowDeletion;
                        }
                    default:
                        {
                            break;
                        }
                }
                return true;
            }
        }

        public bool DeleteSelf() 
        {
            if (mIsDirty) 
            {
                return false;
            }

            switch (Type)
            {
                case SPObjTypeEnum.Site:
                    {
                        SPSite site = (SPSite)SPObj;
                        try
                        {
                            site.Delete();
                        }
                        catch (Exception ex)
                        {
                            string eStr = string.Format("Delete site failed. site url: {0}\r\n{1}", site.Url, ex.Message);
                            throw new ClearSeviceException(ex, eStr);
                        }
                        break;
                    }
                case SPObjTypeEnum.Web:
                    {
                        SPWeb web = (SPWeb)SPObj;
                        try
                        {
                            web.Delete();
                        }
                        catch(Exception ex)
                        {
                            string eStr = string.Format("Delete web failed. web title: {0}\r\n{1}", web.Title, ex.Message);
                            throw new ClearSeviceException(ex, eStr);
                        }
                        break;
                    }
                case SPObjTypeEnum.List:
                    {
                        SPList list = (SPList)SPObj;
                        try
                        {
                            list.Delete();
                        }
                        catch (Exception ex)
                        {
                            string eStr = string.Format("Delete list failed. ListTitle: {0}\r\n{1}", list.Title, ex.Message);
                            throw new ClearSeviceException(ex, eStr);
                        }
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
            return mIsDirty = true;
        }

    }

    class SiteBaseInfo 
    {
        public string URL { get; set; }
        public Guid ID { get; set; }
        public string Owner { get; set; }
    }

    class WebBaseInfo 
    {
        public string URL { get; set; }
        public Guid ID { get; set; }
        public string Template { get; set; }
        public string Title { get; set; }
    }

    class ListBaseInfo
    {
        public string Title { get; set; }
        public string URL { get; set; }
        public Guid ID { get; set; }
        public int Template { get; set; }        
    }
}
