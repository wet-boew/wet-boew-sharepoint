using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Publishing;
using Microsoft.SharePoint.Security;
using System.Globalization;
using System.Collections.Generic;
using System.Collections;
using WET.Theme.Intranet.Objects;

namespace WET.Theme.Intranet.Features
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("1d7f0f0e-448c-4098-99d3-e29cd250eb05")]
    public class EventReceiver : SPFeatureReceiver
    {
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            //This feature activation will make changes to the root site only. 
            //Any site built after will be taken care of by the web created event receiver.
            /*SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                CreateNavigationLibrary(((SPSite)properties.Feature.Parent).Url);
                /*DefaultMasterPageProcess(properties);
                DefaultPageLayoutProcess(properties);
                DefaultNavigation(properties);
            });*/
        }

        private void CreateNavigationLibrary(string url)
        {
            try
            {
                using (SPSite site = new SPSite(url))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        SPList list = web.Lists.TryGetList("TopNavigation");
                        if (list == null)
                        {
                            web.Lists.Add("TopNavigation", "", SPListTemplateType.DocumentLibrary);
                            web.Update();
                        }
                    }
                }
            }
            catch { }
        }

        /*private void DefaultMasterPageProcess(SPFeatureReceiverProperties properties)
        {
            try
            {
                using (SPSite site = new SPSite(((SPSite)properties.Feature.Parent).Url))
                {
                    using (SPWeb web = site.RootWeb)
                    {
                        if (CheckIfMasterPageExist(properties, defaultMasterPage))
                        {
                            web.CustomMasterUrl = "/_catalogs/masterpage/" + defaultMasterPage;
                            web.Update();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("WebProvisioning.cs - DefaultMasterPageProcess: " + ex.Message + " " + ex.StackTrace);
            }
        }

        /// <summary>
        /// Checks to see if the given master page exists in the site collections Master Page library
        /// </summary>
        /// <param name="properties"></param>
        /// <param name="_masterpage">Name of the master page to chech against</param>
        /// <returns></returns>
        private Boolean CheckIfMasterPageExist(SPFeatureReceiverProperties properties, String _masterpage)
        {
            Boolean _bln = false;
            try
            {
                SPSite site = (SPSite)properties.Feature.Parent;
                SPWeb web = site.RootWeb;

                SPList myList = web.Lists["Master Page Gallery"];
                SPQuery oQuery = new SPQuery();
                oQuery.Query = string.Format("<Where><Contains><FieldRef Name=\"FileLeafRef\" /><Value Type=\"File\">.master</Value></Contains></Where><OrderBy><FieldRef Name=\"FileLeafRef\" /></OrderBy>");
                SPListItemCollection colListItems = myList.GetItems(oQuery);
                foreach (SPListItem currentItem in colListItems)
                {
                    if (currentItem.Name.Trim().ToLower() == _masterpage.Trim().ToLower())
                    {
                        _bln = true;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("WebProvisioning.cs - CheckIfMasterPageExist: " + ex.Message + " " + ex.StackTrace);
            }

            return _bln;
        }


        private void DefaultPageLayoutProcess(SPFeatureReceiverProperties properties)
        {
            PageLayout _pageLayout = null;
            try
            {
                using(SPSite site = new SPSite(((SPSite)properties.Feature.Parent).Url))
                {
                    using (SPWeb web = site.RootWeb)
                    {
                        PublishingSite pubSiteCollection = new PublishingSite(site);
                        PublishingWeb publishingWeb = PublishingWeb.GetPublishingWeb(web);

                        //check if pagelayout to be defaulted already exists in AvailablePageLayouts
                        PageLayout[] availablePageLayouts = publishingWeb.GetAvailablePageLayouts();
                        foreach (PageLayout pageLayout in availablePageLayouts)
                        {
                            if (pageLayout.Name == defaultPageLayout)
                                _pageLayout = pageLayout;
                        }

                        //if exists
                        if (_pageLayout != null)
                        {
                            publishingWeb.SetDefaultPageLayout(_pageLayout, true);
                            publishingWeb.Update();
                        }
                        else  //if does not exist
                        {
                            //get all AvailablePageLayouts
                            PageLayout[] _allpageLayout = publishingWeb.GetAvailablePageLayouts();
                            PageLayout[] plarray = new PageLayout[_allpageLayout.Length + 1];
                            int ipl = -1;
                            //transfer existing pagelayouts in AvailablePageLayouts to PageLayout[]
                            foreach (PageLayout _itempl in _allpageLayout)
                            {
                                ipl++;
                                plarray[ipl] = _itempl;
                            }

                            //PageLayout to be defaulted to
                            _pageLayout = pubSiteCollection.PageLayouts["/_catalogs/masterpage/" + defaultPageLayout];
                            ipl++;
                            //add to the PageLayout array
                            plarray[ipl] = _pageLayout;
                            //reset AvailablePageLayouts
                            publishingWeb.SetAvailablePageLayouts(plarray, true);
                            publishingWeb.Update();
                            //set DefaultPageLayout
                            publishingWeb.SetDefaultPageLayout(_pageLayout, true);

                            publishingWeb.Update();
                            web.Update();
                        }

                        //Swap the page layout of the default.aspx page
                        //SwapPageLayout(publishingWeb, _pageLayout, web.Site.RootWeb.ContentTypes[_pageLayout.AssociatedContentType.Id]);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("WebProvisioning.cs - DefaultPageLayoutProcess: " + ex.Message + " " + ex.StackTrace);
            }
        }


        public static void SwapPageLayout(PublishingWeb publishingWeb, PageLayout defaultPageLayout, SPContentType ctype)
        {
            string checkInComment = "Automatic Event Handler Page Layout Fix";
            //
            // Validate the input parameters.
            if (null == publishingWeb)
            {
                throw new System.ArgumentNullException("publishingWeb");
            }
            if (null == defaultPageLayout)
            {
                throw new System.ArgumentNullException("defaultPageLayout");
            }

            SPList list = publishingWeb.PagesList;
            if (list.ContentTypes[defaultPageLayout.AssociatedContentType.Name] == null)
            {
                list.ContentTypes.Add(ctype);
            }
            SPContentType ct = list.ContentTypes[defaultPageLayout.AssociatedContentType.Name];

            PublishingPageCollection publishingPages = publishingWeb.GetPublishingPages();
            foreach (PublishingPage publishingPage in publishingPages)
            {
                if (publishingPage.ListItem.File.CheckOutType == SPFile.SPCheckOutType.None)
                {
                    publishingPage.CheckOut();
                }
                publishingPage.ListItem["ContentTypeId"] = ct.Id;

                switch (publishingPage.Url)
                {
                    default:
                        publishingPage.Layout = defaultPageLayout;
                        publishingPage.Title = publishingWeb.Title;
                        break;
                }
                publishingPage.Update();
                publishingPage.CheckIn(checkInComment);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="properties"></param>
        private void DefaultNavigation(SPFeatureReceiverProperties properties)
        {
            try
            {
                PublishingWeb pubWeb = null;

                if (PublishingWeb.IsPublishingWeb((SPWeb)properties.Feature.Parent))
                {
                    pubWeb = PublishingWeb.GetPublishingWeb((SPWeb)properties.Feature.Parent);
                    //Global Navigation Settings
                    pubWeb.Navigation.GlobalIncludeSubSites = true;
                    pubWeb.Navigation.GlobalIncludePages = false;
                    //Current Navigation Settings
                    pubWeb.Navigation.CurrentIncludeSubSites = true;
                    pubWeb.Navigation.CurrentIncludePages = false;
                    pubWeb.Update();
                    ((SPWeb)properties.Feature.Parent).Update();                    
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("WebProvisioning.cs - DefaultNavigation: " + ex.Message + " " + ex.StackTrace);
            }
        }


        // Uncomment the method below to handle the event raised before a feature is deactivated.

        //public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        //{
        //}


        // Uncomment the method below to handle the event raised after a feature has been installed.

        //public override void FeatureInstalled(SPFeatureReceiverProperties properties)
        //{
        //}


        // Uncomment the method below to handle the event raised before a feature is uninstalled.

        //public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
        //{
        //}

        // Uncomment the method below to handle the event raised when a feature is upgrading.

        //public override void FeatureUpgrading(SPFeatureReceiverProperties properties, string upgradeActionName, System.Collections.Generic.IDictionary<string, string> parameters)
        //{
        //}*/
    }
}
