using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SiteCollectionManager._Common;
using SiteCollectionManager.UI;
using System.Windows.Forms;
using Microsoft.SharePoint;

namespace SiteCollectionManager._Sevice
{
    class SearchInfo
    {
        public SearchByEumn mSearchBy = SearchByEumn.ID;
        public SPDataBox mSearchScop = null;
        public SPObjTypeEnum mSearchType = SPObjTypeEnum.Invalid;
        public object mSearchIndex = null;
    }

    public enum SearchByEumn
    {
        ID,
        Title,
        URL
    }

    class SearchSevice: GSevice, IDisposable
    {
        private SearchInfo mSearchInfo = null;

        private List<SPDataBox> mRes = new List<SPDataBox>();

        public SearchSevice(ProgressBarHelper prgBar, TextBox txtBox, SearchInfo searchInfo)
            : base(prgBar, txtBox)
        {
            mSearchInfo = searchInfo;
        }

        public List<SPDataBox> Run()
        {
            switch (mSearchInfo.mSearchScop.Type) 
            {
                case SPObjTypeEnum.Site:
                    SearchInternal(((SPSite)mSearchInfo.mSearchScop.SPObj).RootWeb);
                    return mRes;
                    break;
                case SPObjTypeEnum.Web:
                    SearchInternal((SPWeb)mSearchInfo.mSearchScop.SPObj);
                    return mRes;
                    break;
                default:
                    return mRes;
                    break;
            }
        }

        private void SearchInternal(SPWeb parentWeb) 
        {
            switch (mSearchInfo.mSearchType) 
            {
                case SPObjTypeEnum.Field:
                    SearchField(parentWeb);
                    break;
                case SPObjTypeEnum.ContentType:
                    SearchContentType(parentWeb);
                    break;
                case SPObjTypeEnum.Feature:
                    SearchFeature(parentWeb.Site);
                    break;
                case SPObjTypeEnum.View:
                    SearchView(parentWeb);
                    break;
                default:
                    break;
            }
        }

        public void SearchField(SPWeb parentWeb) 
        {
            switch (mSearchInfo.mSearchBy) 
            {
                case SearchByEumn.ID:
                    Guid id = new Guid((string)mSearchInfo.mSearchIndex);
                    try
                    {
                        SPField field = parentWeb.Fields[id];
                        SPDataBox box = new SPDataBox();
                        box.SPObj = field;
                        box.Type = SPObjTypeEnum.Field;
                        box.ParentType = SPObjTypeEnum.Web;

                        mRes.Add(box);
                    }
                    catch { }
                    foreach (SPList list in parentWeb.Lists)
                    {
                        SearchField(list);
                    }
                    foreach (SPWeb subWeb in parentWeb.Webs) 
                    {
                        SearchField(subWeb);
                    }
                    break;
                default:
                    break;
            }
            
        }

        public void SearchField(SPList list)
        {
            switch (mSearchInfo.mSearchBy)
            {
                case SearchByEumn.ID:
                    try
                    {

                        Guid id = new Guid((string)mSearchInfo.mSearchIndex);
                        SPField field = list.Fields[id];
                        SPDataBox box = new SPDataBox();
                        box.SPObj = field;
                        box.Type = SPObjTypeEnum.Field;
                        box.ParentType = SPObjTypeEnum.List;

                        mRes.Add(box);

                    }
                    catch { }
                    break;
                default:break;
            }
        }

        public void SearchContentType(SPWeb parentWeb) 
        {
            switch (mSearchInfo.mSearchBy)
            {
                case SearchByEumn.ID:
                    {
                        //byte[] byts = StringParser.ConvertHexStringToBytes(mSearchInfo.mSearchIndex);
                        SPContentTypeId id = new SPContentTypeId((string)mSearchInfo.mSearchIndex);
                        try
                        {
                            SPContentType ct = parentWeb.ContentTypes[id];
                            if (ct != null)
                            {

                                SPDataBox box = new SPDataBox();
                                box.SPObj = ct;
                                box.Type = SPObjTypeEnum.ContentType;
                                box.ParentType = SPObjTypeEnum.Web;

                                mRes.Add(box);
                            }
                        }
                        catch { }
                        foreach (SPList list in parentWeb.Lists)
                        {
                            
                        }
                        foreach (SPWeb subWeb in parentWeb.Webs)
                        {
                            SearchContentType(subWeb);
                        }
                        break;
                    }
                default:break;
            }
            
        }

        public void SearchContentType(SPList list) 
        {
            switch (mSearchInfo.mSearchBy)
            {
                case SearchByEumn.ID:
                    try
                    {
                        SPContentTypeId id = new SPContentTypeId((string)mSearchInfo.mSearchIndex);
                        SPContentType ct = list.ContentTypes[id];
                        if (ct != null)
                        {
                            SPDataBox box = new SPDataBox();
                            box.SPObj = ct;
                            box.Type = SPObjTypeEnum.ContentType;
                            box.ParentType = SPObjTypeEnum.List;
                            mRes.Add(box);
                        }
                    }
                    catch { }
                    break;
            }
        }

        public void SearchFeature(SPSite parentSite) 
        {
            switch (mSearchInfo.mSearchBy)
            {
                case SearchByEumn.ID:
                    Guid id = new Guid((string)mSearchInfo.mSearchIndex);
                    try
                    {
                        SPFeature feature = parentSite.Features[id];
                        if (feature == null) 
                        {
                            throw new Exception("G.Except.");
                        }
                        SPDataBox box = new SPDataBox();
                        box.SPObj = feature;
                        box.Type = SPObjTypeEnum.Feature;
                        box.ParentType = SPObjTypeEnum.Site;

                        mRes.Add(box);

                    }
                    catch 
                    {
                    }
                    SearchFeature(parentSite.RootWeb);
                    break;
                default: break;
            }
        }

        public void SearchFeature(SPWeb parentWeb)
        {
            switch (mSearchInfo.mSearchBy)
            {
                case SearchByEumn.ID:
                    Guid id = new Guid((string)mSearchInfo.mSearchIndex);
                    try
                    {
                        SPFeature feature = parentWeb.Features[id];
                        if (feature == null)
                        {
                            throw new Exception("G.Except.");
                        }
                        SPDataBox box = new SPDataBox();
                        box.SPObj = feature;
                        box.Type = SPObjTypeEnum.Feature;
                        box.ParentType = SPObjTypeEnum.Web;

                        mRes.Add(box);
                    }
                    catch
                    {
                    }
                    foreach (SPWeb subWeb in parentWeb.Webs)
                    {
                        SearchFeature(subWeb);
                    }
                    break;
                default: break;
            }
        }

        public void SearchView(SPWeb web) 
        {
            switch (mSearchInfo.mSearchBy)
            {
                case SearchByEumn.ID:
                    Guid id = new Guid((string)mSearchInfo.mSearchIndex);

                    foreach (SPList list in web.Lists)
                    {
                        try
                        {
                            SPView view = list.Views[id];

                            SPDataBox box = new SPDataBox();
                            box.SPObj = view;
                            box.Type = SPObjTypeEnum.View;
                            box.ParentType = SPObjTypeEnum.List;

                            mRes.Add(box);
                        }
                        catch { }
                    }
                    foreach (SPWeb subWeb in web.Webs)
                    {
                        SearchFeature(subWeb);
                    }
                    break;
                default: break;
            }
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
