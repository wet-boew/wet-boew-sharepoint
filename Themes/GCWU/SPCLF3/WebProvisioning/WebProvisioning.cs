using System;
using System.Collections.Generic;
using System.Collections;
using System.Globalization;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Publishing;
using System.Linq;
using Microsoft.SharePoint.Administration;
using SPCLF3.Objects;

namespace SPCLF3.WebProvisioning
{
    /// <summary>
    /// Web Events
    /// </summary>
    public class WebProvisioning : SPWebEventReceiver
    {
        private string defaultMasterPage = "CLF3Publishing.master";
        private string defaultPageLayout = "Layout2Col.aspx";
        private string footerListName = "CLF3FooterNavigation";
        private SPListTemplateType listtype = SPListTemplateType.GenericList;
        private SPList footerList = null;

        public override void WebProvisioned(SPWebEventProperties properties)
        {
            base.WebProvisioned(properties);
            using (SPSite site = new SPSite(properties.Web.Site.ID))
            {
                using (SPWeb web = site.AllWebs[properties.WebId])
                {
                    if (PublishingWeb.IsPublishingWeb(web))
                    {
                        SPSecurity.RunWithElevatedPrivileges(delegate()
                        {
                            DefaultMasterPageProcess(properties);
                            DefaultPageLayoutProcess(properties);
                            MUIProcess(properties);
                            DefaultNavigation(properties);
                            CreateFooterList(properties);
                        });
                    }
                }
            }
        }


        private void DefaultMasterPageProcess(SPWebEventProperties properties)
        {
            try
            {
                using (SPSite site = new SPSite(properties.Web.Site.ID))
                {
                    using (SPWeb web = site.AllWebs[properties.WebId])
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
        private Boolean CheckIfMasterPageExist(SPWebEventProperties properties, String _masterpage)
        {
            Boolean _bln = false;
            try
            {
                using (SPSite site = new SPSite(properties.SiteId))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
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
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("WebProvisioning.cs - CheckIfMasterPageExist: " + ex.Message + " " + ex.StackTrace);
            }

            return _bln;
        }


        private void DefaultPageLayoutProcess(SPWebEventProperties properties)
        {
            PageLayout _pageLayout;
            try
            {
                using (SPSite site = new SPSite(properties.SiteId))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        PublishingSite pubSiteCollection = new PublishingSite(site);
                        PublishingWeb publishingWeb = PublishingWeb.GetPublishingWeb(properties.Web);

                        //check if pagelayout to be defaulted already exists in AvailablePageLayouts
                        _pageLayout = (from _pl in publishingWeb.GetAvailablePageLayouts()
                                       where _pl.Name == defaultPageLayout
                                       select _pl).FirstOrDefault();

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
                        SwapPageLayout(publishingWeb, _pageLayout, web.Site.RootWeb.ContentTypes[_pageLayout.AssociatedContentType.Id]);
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
            string checkInComment = "CLF3 Automatic Event Handler Page Layout Fix";
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


        private void MUIProcess(SPWebEventProperties properties)
        {
            try
            {
                using (SPSite site = new SPSite(properties.Web.Site.ID))
                {
                    using (SPWeb web = site.AllWebs[properties.WebId])
                    {
                        web.IsMultilingual = true;
                        // Add support for any installed language currently not supported.            
                        SPLanguageCollection installed = SPRegionalSettings.GlobalInstalledLanguages;
                        IEnumerable<CultureInfo> cultures = web.SupportedUICultures;
                        foreach (SPLanguage language in installed)
                        {
                            CultureInfo culture = new CultureInfo(language.LCID);
                            if (!cultures.Contains(culture))
                            {
                                web.AddSupportedUICulture(culture);
                            }
                        }
                        web.Update();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("WebProvisioning.cs - MUIProcess: " + ex.Message + " " + ex.StackTrace);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="properties"></param>
        private void DefaultNavigation(SPWebEventProperties properties)
        {
            int level;
            try
            {
                using (SPSite site = new SPSite(properties.Web.Site.ID))
                {
                    using (SPWeb web = site.AllWebs[properties.WebId])
                    {
                        PublishingSite pubsite = new PublishingSite(site);
                        PublishingWeb pubWeb = this.GetPublishingSiteFromWeb(web);
                        level = web.ServerRelativeUrl.Split('/').Length - 1;

                        switch (level)
                        {
                            case 0:
                                //Global Navigation Settings
                                pubWeb.Navigation.GlobalIncludeSubSites = true;
                                pubWeb.Navigation.GlobalIncludePages = false;
                                //Current Navigation Settings
                                pubWeb.Navigation.CurrentIncludeSubSites = true;
                                pubWeb.Navigation.CurrentIncludePages = false;
                                web.Update();
                                break;
                            case 1:
                                //Global Navigation Settings
                                pubWeb.Navigation.InheritGlobal = true;
                                pubWeb.Navigation.GlobalIncludeSubSites = true;
                                pubWeb.Navigation.GlobalIncludePages = false;
                                //Current Navigation Settings
                                pubWeb.Navigation.ShowSiblings = true;
                                pubWeb.Navigation.CurrentIncludeSubSites = true;
                                pubWeb.Navigation.CurrentIncludePages = false;
                                pubWeb.Update();
                                break;
                            default:
                                //Global Navigation Settings
                                pubWeb.Navigation.InheritGlobal = true;
                                pubWeb.Navigation.GlobalIncludeSubSites = true;
                                pubWeb.Navigation.GlobalIncludePages = false;
                                //Current Navigation Settings
                                pubWeb.Navigation.InheritCurrent = true;
                                pubWeb.Navigation.CurrentIncludeSubSites = true;
                                pubWeb.Navigation.CurrentIncludePages = false;
                                pubWeb.Update();
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("WebProvisioning.cs - DefaultNavigation: " + ex.Message + " " + ex.StackTrace);
            }
        }

        private PublishingWeb GetPublishingSiteFromWeb(SPWeb web)
        {
            PublishingWeb result = null;

            if (web == null)
                throw new ArgumentNullException("web");

            var pubSite = new PublishingSite(web.Site);

            if (PublishingWeb.IsPublishingWeb(web))
            {
                result = PublishingWeb.GetPublishingWeb(web);
            }

            return result;
        }


        private void CreateFooterList(SPWebEventProperties properties)
        {
            int level;
            try
            {
                using (SPSite site = new SPSite(properties.Web.Site.ID))
                {
                    using (SPWeb web = site.AllWebs[properties.WebId])
                    {
                        level = web.ServerRelativeUrl.Split('/').Length - 1;

                        if (level == 1)
                        {
                            //Check if list already exists, if not, create one. 
                            footerList = web.Lists.TryGetList(footerListName);
                            if (footerList == null)
                                web.Lists.Add(footerListName, "This SharePoint list is used for the CLF 3.0 Footer", listtype);

                            SPList list = web.Lists[footerListName];
                            list.EnableFolderCreation = true;

                            // create columns
                            //assume that column "Title" is already created
                            list.Fields.Add("NavURL", SPFieldType.Text, false);
                            list.Fields.Add("RowOrder", SPFieldType.Number, true);

                            // make new column visible in default view
                            SPView view = list.DefaultView;
                            view.ViewFields.Add("Type");
                            view.ViewFields.Add("NavURL");
                            view.ViewFields.Add("RowOrder");
                            view.Update();

                            #region Create About Us
                            //Create Folders
                            SPListItem folderItem = list.Items.Add(list.RootFolder.ServerRelativeUrl, SPFileSystemObjectType.Folder);
                            folderItem["Title"] = "About Us";
                            folderItem.Update();

                            //create a listitem object to add item in the foler
                            SPListItem listItem = list.Items.Add(folderItem.Folder.ServerRelativeUrl, SPFileSystemObjectType.File, null);
                            //Set the values for other fields in the list
                            listItem["Title"] = "About Us";
                            listItem["NavURL"] = "/eng/Pages/about.aspx";
                            listItem["RowOrder"] = 1;
                            listItem.Update();

                            SPListItem listItem2 = list.Items.Add(folderItem.Folder.ServerRelativeUrl, SPFileSystemObjectType.File, null);
                            //Set the values for other fields in the list
                            listItem2["Title"] = "Our Mandate";
                            listItem2["NavURL"] = "/eng/Pages/mandate.aspx";
                            listItem2["RowOrder"] = 2;
                            listItem2.Update();

                            SPListItem listItem3 = list.Items.Add(folderItem.Folder.ServerRelativeUrl, SPFileSystemObjectType.File, null);
                            //Set the values for other fields in the list
                            listItem3["Title"] = "Our Minister";
                            listItem3["NavURL"] = "/eng/Pages/minister.aspx";
                            listItem3["RowOrder"] = 3;
                            listItem3.Update();

                            list.Update();
                            #endregion

                            #region Create Contact Us
                            SPListItem folderItem2 = list.Items.Add(list.RootFolder.ServerRelativeUrl, SPFileSystemObjectType.Folder);
                            folderItem2["Title"] = "Contact Us";
                            folderItem2.Update();

                            SPListItem listItem4 = list.Items.Add(folderItem2.Folder.ServerRelativeUrl, SPFileSystemObjectType.File, null);
                            //Set the values for other fields in the list
                            listItem4["Title"] = "Contact Us";
                            listItem4["NavURL"] = "/eng/Pages/contact.aspx";
                            listItem4["RowOrder"] = 1;
                            listItem4.Update();

                            SPListItem listItem5 = list.Items.Add(folderItem2.Folder.ServerRelativeUrl, SPFileSystemObjectType.File, null);
                            //Set the values for other fields in the list
                            listItem5["Title"] = "Phone numbers";
                            listItem5["NavURL"] = "/eng/Pages/phone.aspx";
                            listItem5["RowOrder"] = 2;
                            listItem5.Update();

                            SPListItem listItem6 = list.Items.Add(folderItem2.Folder.ServerRelativeUrl, SPFileSystemObjectType.File, null);
                            //Set the values for other fields in the list
                            listItem6["Title"] = "Office locations";
                            listItem6["NavURL"] = "/eng/Pages/office.aspx";
                            listItem6["RowOrder"] = 3;
                            listItem6.Update();

                            list.Update();
                            #endregion

                            #region Create News
                            SPListItem folderItem3 = list.Items.Add(list.RootFolder.ServerRelativeUrl, SPFileSystemObjectType.Folder);
                            folderItem3["Title"] = "News";
                            folderItem3.Update();

                            SPListItem listItem7 = list.Items.Add(folderItem3.Folder.ServerRelativeUrl, SPFileSystemObjectType.File, null);
                            //Set the values for other fields in the list
                            listItem7["Title"] = "News";
                            listItem7["NavURL"] = "/eng/Pages/news.aspx";
                            listItem7["RowOrder"] = 1;
                            listItem7.Update();
                            list.Update();

                            SPListItem listItem8 = list.Items.Add(folderItem3.Folder.ServerRelativeUrl, SPFileSystemObjectType.File, null);
                            //Set the values for other fields in the list
                            listItem8["Title"] = "News releases";
                            listItem8["NavURL"] = "/eng/Pages/releases.aspx";
                            listItem8["RowOrder"] = 2;
                            listItem8.Update();
                            list.Update();

                            SPListItem listItem9 = list.Items.Add(folderItem3.Folder.ServerRelativeUrl, SPFileSystemObjectType.File, null);
                            //Set the values for other fields in the list
                            listItem9["Title"] = "Multimedia";
                            listItem9["NavURL"] = "/eng/Pages/multimedia.aspx";
                            listItem9["RowOrder"] = 3;
                            listItem9.Update();
                            list.Update();

                            SPListItem listItem10 = list.Items.Add(folderItem3.Folder.ServerRelativeUrl, SPFileSystemObjectType.File, null);
                            //Set the values for other fields in the list
                            listItem10["Title"] = "Media advisories";
                            listItem10["NavURL"] = "/eng/Pages/advisories.aspx";
                            listItem10["RowOrder"] = 4;
                            listItem10.Update();

                            list.Update();
                            #endregion

                            #region Create Stay Connected
                            SPListItem folderItem4 = list.Items.Add(list.RootFolder.ServerRelativeUrl, SPFileSystemObjectType.Folder);
                            folderItem4["Title"] = "Stay Connected";
                            folderItem4.Update();

                            SPListItem listItem11 = list.Items.Add(folderItem4.Folder.ServerRelativeUrl, SPFileSystemObjectType.File, null);
                            //Set the values for other fields in the list
                            listItem11["Title"] = "Stay Connected";
                            listItem11["NavURL"] = "";
                            listItem11["RowOrder"] = 0;
                            listItem11.Update();

                            SPListItem listItem12 = list.Items.Add(folderItem4.Folder.ServerRelativeUrl, SPFileSystemObjectType.File, null);
                            //Set the values for other fields in the list
                            listItem12["Title"] = "You Tube";
                            listItem12["NavURL"] = "/eng/Pages/youtube.aspx";
                            listItem12["RowOrder"] = 1;
                            listItem12.Update();

                            SPListItem listItem13 = list.Items.Add(folderItem4.Folder.ServerRelativeUrl, SPFileSystemObjectType.File, null);
                            //Set the values for other fields in the list
                            listItem13["Title"] = "Twitter";
                            listItem13["NavURL"] = "/eng/Pages/twitter.aspx";
                            listItem13["RowOrder"] = 2;
                            listItem13.Update();

                            SPListItem listItem14 = list.Items.Add(folderItem4.Folder.ServerRelativeUrl, SPFileSystemObjectType.File, null);
                            //Set the values for other fields in the list
                            listItem14["Title"] = "Feeds";
                            listItem14["NavURL"] = "/eng/Pages/Feeds.aspx";
                            listItem14["RowOrder"] = 3;
                            listItem14.Update();

                            list.Update();
                            #endregion

                            //adding two item that are not in a folder
                            SPListItem item = list.Items.Add();
                            item["Title"] = "Terms and Conditions";
                            item["NavURL"] = "/eng/Pages/terms.aspx";
                            item["RowOrder"] = 1;
                            item.Update();

                            SPListItem item2 = list.Items.Add();
                            item2["Title"] = "Transparency";
                            item2["NavURL"] = "/eng/Pages/transparency.aspx";
                            item2["RowOrder"] = 2;
                            item2.Update();

                            list.Update();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("WebProvisioning.cs - DefaultMasterPageProcess: " + ex.Message + " " + ex.StackTrace);
            }
        }


    }
}
