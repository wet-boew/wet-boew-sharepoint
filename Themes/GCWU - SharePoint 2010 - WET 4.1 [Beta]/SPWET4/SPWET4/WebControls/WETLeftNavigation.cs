using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Publishing;
using Microsoft.SharePoint.Publishing.Navigation;
using System.Web.SessionState;
using SPWET4.Objects;

namespace SPWET4.WebControls
{
    /*
     * This control requires Sharepoint List WET4LeftNavigation. CLF3LeftNavigation List should be renamed to WET4LeftNavigation
     */
    [ToolboxData("<{0}:WETLeftNavigation runat=\"server\" />")]
    public class WETLeftNavigation : WebControl
    {
        protected override void Render(HtmlTextWriter writer)
        {
            string currentNavClass = string.Empty;
            string selectedNav = string.Empty;

            try
            {
                // setup the outer wrappers
                string htmlOutput = string.Empty;

                // figure out our language of the current label
                if (SPContext.Current.ListItem != null && PublishingPage.IsPublishingPage(SPContext.Current.ListItem))
                {
                    PublishingPage publishingPage = PublishingPage.GetPublishingPage(SPContext.Current.ListItem);
                    string langWeb = string.Empty;
                    if (publishingPage.PublishingWeb.Label != null)
                        langWeb = (publishingPage.PublishingWeb.Label.Title.Substring(0, 2).ToLower() == "en") ? "eng" : "fra";
                    else
                        langWeb = "eng";

                    selectedNav = SPContext.Current.ListItemServerRelativeUrl;

                    SPList WET4LeftNavigation = null;
                    List<SPListItem> firstLevelCollItem;

                    htmlOutput += "<ul class=\"list-group menu list-unstyled\">";

                    try
                    {
                        WET4LeftNavigation = SPContext.Current.Web.Lists["CLF3LeftNavigation"];

                        firstLevelCollItem = (from SPListItem li in WET4LeftNavigation.Items
                                              where Convert.ToString(li["Level"]).IndexOf(".") == -1
                                              orderby li["SortOrder"]
                                              select li).ToList<SPListItem>();

                        foreach (SPListItem oItem_1 in firstLevelCollItem)
                        {
                            //Ensure that this is a first level link by checking number of dots in Level value. Must be 0
                            string level = oItem_1["Level"].ToString();
                            List<char> list = level.ToList<char>();
                            int numberOfDots = list.Count<char>(c => c == '.');

                            if ((numberOfDots == 0))
                            {
                                //renderfirstlevelLink
                                htmlOutput += renderTopLevelLink(WET4LeftNavigation, oItem_1, level, langWeb, selectedNav);
                            }
                        }
                    }
                    finally
                    {
                    }

                    htmlOutput += "</ul>";
                    htmlOutput += "<br /><br /><br />";

                    writer.Write(htmlOutput);
                }
                else
                {
                    //Nik20121026 - Handles the case where the site's template is a collaboration one;
                    string langWeb = string.Empty;
                    string cultISO = "";
                    if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("/eng/"))
                        cultISO = "en";
                    else
                        cultISO = "fr";

                    langWeb = (cultISO == "en") ? "eng" : "fra";
                    
                    selectedNav = SPContext.Current.ListItemServerRelativeUrl;

                    SPList WET4LeftNavigation = null;
                    List<SPListItem> firstLevelCollItem;

                    htmlOutput += "<ul class=\"list-group menu list-unstyled\">";

                    try
                    {
                        WET4LeftNavigation = SPContext.Current.Web.Lists["WET4LeftNavigation"];

                        firstLevelCollItem = (from SPListItem li in WET4LeftNavigation.Items
                                              where Convert.ToString(li["Level"]).IndexOf(".") == -1
                                              orderby li["SortOrder"]
                                              select li).ToList<SPListItem>();

                        foreach (SPListItem oItem_1 in firstLevelCollItem)
                        {
                            //Ensure that this is a first level link by checking number of dots in Level value. Must be 0
                            string level = oItem_1["Level"].ToString();
                            List<char> list = level.ToList<char>();
                            int numberOfDots = list.Count<char>(c => c == '.');

                            if ((numberOfDots == 0))
                            {
                                //renderfirstlevelLink
                                htmlOutput += renderTopLevelLink(WET4LeftNavigation, oItem_1, level, langWeb, selectedNav);
                            }
                        }
                    }
                    finally
                    {
                    }

                    writer.Write(htmlOutput);
                    htmlOutput += "</ul>";
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex.Message + " " + ex.StackTrace);
            }

        }

        private string renderTopLevelLink(SPList aList, SPListItem aItem, string aLevel, string aLang, string selected)
        {
            string returnString = string.Empty;
            string isFirstClass = string.Empty;
            string urlLink = string.Empty;
            string title = string.Empty;

            List<SPListItem> secondLevelCollItem = (from SPListItem li in aList.Items
                                                    where Convert.ToString(li["Level"]).StartsWith(aLevel)
                                                    orderby li["SortOrder"]
                                                    select li).ToList<SPListItem>();

            title = Convert.ToString(aItem["Title"]);
            urlLink = Convert.ToString(aItem["UrlLink"]);

            if (secondLevelCollItem.Count > 1)
            {
                // have children... render for expansion
                if (urlLink == "#" || string.IsNullOrEmpty(urlLink))
                    returnString = "<li><h3 class=\"wb-navcurr\">" + title + "</h3><ul class=\"list-group menu list-unstyled\">";
                else
                    returnString = "<li><h3 class=\"wb-navcurr\">" +
                                "<a href=\"" + urlLink + "\">" + title + "</a></h3><ul class=\"list-group menu list-unstyled\">";

                // render any children
                foreach (SPListItem oItem in secondLevelCollItem)
                {
                    returnString += renderSecondLevelLink(aList, oItem, Convert.ToString(aItem["Level"]), aLang, selected);
                }

                // close off the heading
                returnString += "</ul></li>";
            }
            else
            {
                // top level heading with no children
                if (urlLink == "#" || string.IsNullOrEmpty(urlLink))
                    returnString = "<li><h3 class=\"wb-navcurr\">" + title + "</h3></li>";
                else
                    returnString = "<li><h3 class=\"wb-navcurr\"><a href=\"" + urlLink + "\">" + title + "</a></h3></li>";
            }
            return returnString;
        }

        private string renderSecondLevelLink(SPList aList, SPListItem aItem, string aLevel, string aLang, string selected)
        {
            string returnString = string.Empty;
            string currentNavClass = string.Empty;

            //Ensure that this is a second level link by checking number of dots in Level value. Must be 1
            string level = aItem["Level"].ToString();
            List<char> list = level.ToList<char>();
            int numberOfDots = list.Count<char>(c => c == '.');

            if (numberOfDots == 1)
            {
                string urlLink = string.Empty;
                string title = string.Empty;

                title = Convert.ToString(aItem["Title"]);
                urlLink = Convert.ToString(aItem["UrlLink"]);

                if (selected == urlLink)
                {
                    currentNavClass = "list-group-item wb-navcurr";
                }
                else
                {
                    currentNavClass = "list-group-item";
                }

                returnString = "<li><a href=\"" + urlLink + "\" class=\"" + currentNavClass + "\">" + title + "</a>";

                //Check if this level has any sub levels
                List<SPListItem> thirdLevelCollItem = (from SPListItem li in aList.Items
                                                       where Convert.ToString(li["Level"]).StartsWith(level)
                                                       orderby li["SortOrder"]
                                                       select li).ToList<SPListItem>();



                // render the second level heading
                if (thirdLevelCollItem.Count > 1)
                {
                    // start our child UL
                    returnString += "<ul class=\"list-group list-unstyled\">";

                    // render any children
                    foreach (SPItem item in thirdLevelCollItem)
                    {
                        string thirdLevel = item["Level"].ToString();
                        List<char> thirdList = thirdLevel.ToList<char>();
                        numberOfDots = thirdList.Count<char>(c => c == '.');
                        urlLink = Convert.ToString(item["UrlLink"]);
                        if (urlLink.ToLower().Contains(selected.ToLower()))
                        {
                            currentNavClass = "list-group-item wb-navcurr";
                        }
                        else
                        {
                            currentNavClass = "list-group-item";
                        }


                        if (numberOfDots == 2)
                        {
                            title = Convert.ToString(item["Title"]);
                            returnString += "<li><a href=\"" + urlLink + "\" class=\"" + currentNavClass + "\">" + title + "</a></li>";
                        }
                    }

                    // close off the list
                    returnString += "</ul>";
                }

                // close off the heading
                returnString += "</li>";
            }

            return returnString;
        }
    }
}
