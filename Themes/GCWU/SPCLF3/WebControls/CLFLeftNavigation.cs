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
using SPCLF3.Objects;

namespace SPCLF3.WebControls
{
    [ToolboxData("<{0}:CLFLeftNavigation runat=\"server\" />")]
    public class CLFLeftNavigation : WebControl
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

                    currentNavClass = "class=\"nav-current\"";
                    selectedNav = SPContext.Current.ListItemServerRelativeUrl;

                    SPList CLF3LeftNavigation = null;
                    List<SPListItem> firstLevelCollItem;

                    try
                    {
                        CLF3LeftNavigation = SPContext.Current.Web.Lists["CLF3LeftNavigation"];

                        firstLevelCollItem = (from SPListItem li in CLF3LeftNavigation.Items
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
                                htmlOutput += renderTopLevelLink(CLF3LeftNavigation, oItem_1, level, langWeb, selectedNav);
                            }
                        }
                    }
                    finally
                    {
                    }

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

                    currentNavClass = "class=\"nav-current\"";
                    selectedNav = SPContext.Current.ListItemServerRelativeUrl;

                    SPList CLF3LeftNavigation = null;
                    List<SPListItem> firstLevelCollItem;

                    try
                    {
                        CLF3LeftNavigation = SPContext.Current.Web.Lists["CLF3LeftNavigation"];

                        firstLevelCollItem = (from SPListItem li in CLF3LeftNavigation.Items
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
                                htmlOutput += renderTopLevelLink(CLF3LeftNavigation, oItem_1, level, langWeb, selectedNav);
                            }
                        }
                    }
                    finally
                    {
                    }

                    writer.Write(htmlOutput);
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
            string currentNavClass = string.Empty;

            List<SPListItem> secondLevelCollItem = (from SPListItem li in aList.Items
                                                    where Convert.ToString(li["Level"]).StartsWith(aLevel)
                                                    orderby li["SortOrder"]
                                                    select li).ToList<SPListItem>();

            title = Convert.ToString(aItem["Title"]);
            urlLink = Convert.ToString(aItem["UrlLink"]);

            if (selected == urlLink)
            {
                currentNavClass = " class=\"nav-current\"";
            }
            else
            {
                currentNavClass = "";
            }

            if (secondLevelCollItem.Count > 1)
            {
                // have children... render for expansion
                returnString = "<section><h3>" +
                                "<a href=\"" + urlLink + "\"" + currentNavClass + ">" + title + "</a></h3>";

                // render any children
                foreach (SPListItem oItem in secondLevelCollItem)
                {
                    returnString += renderSecondLevelLink(aList, oItem, Convert.ToString(aItem["Level"]), aLang, selected);
                }

                // close off the heading
                returnString += "</section>";
            }
            else
            {
                // top level heading with no children
                returnString = "<section><h3><a href=\"" + urlLink + "\"" + currentNavClass + ">" + title + "</a></h3></section>";
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
                    currentNavClass = " class=\"nav-current\"";
                }
                else
                {
                    currentNavClass = string.Empty;
                }

                returnString = "<ul>" +
                               "<li><a href=\"" + urlLink + "\"" + currentNavClass + ">" + title + "</a>";

                //Check if this level has any sub levels
                List<SPListItem> thirdLevelCollItem = (from SPListItem li in aList.Items
                                                       where Convert.ToString(li["Level"]).StartsWith(level)
                                                       orderby li["SortOrder"]
                                                       select li).ToList<SPListItem>();



                // render the second level heading
                if (thirdLevelCollItem.Count > 1)
                {
                    // start our child UL
                    returnString += "<ul>";

                    // render any children
                    foreach (SPItem item in thirdLevelCollItem)
                    {
                        string thirdLevel = item["Level"].ToString();
                        List<char> thirdList = thirdLevel.ToList<char>();
                        numberOfDots = thirdList.Count<char>(c => c == '.');
                        urlLink = Convert.ToString(item["UrlLink"]);
                        if (urlLink.ToLower().Contains(selected.ToLower()))
                        {
                            currentNavClass = " class=\"nav-current\"";
                        }
                        else
                        {
                            currentNavClass = string.Empty;
                        }


                        if (numberOfDots == 2)
                        {
                            title = Convert.ToString(item["Title"]);
                            returnString += "<li><a href=\"" + urlLink + "\"" + currentNavClass + ">" + title + "</a></li>";
                        }
                    }

                    // close off the list
                    returnString += "</ul>";
                }

                // close off the heading
                returnString += "</li></ul>";
            }

            return returnString;
        }
    }
}
