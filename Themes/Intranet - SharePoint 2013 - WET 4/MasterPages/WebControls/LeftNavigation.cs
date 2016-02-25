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
using WET.Theme.Intranet.Objects;

namespace WET.Theme.Intranet.WebControls
{
    [ToolboxData("<{0}:LeftNavigation runat=\"server\" />")]
    public class LeftNavigation : WebControl
    {
        protected override void Render(HtmlTextWriter writer)
        {
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

                    SPList WETLeftNavigation = null;
                    List<SPListItem> firstLevelCollItem;

                    try
                    {
                        WETLeftNavigation = SPContext.Current.Web.Lists["WETLeftNavigation"];

                        firstLevelCollItem = (from SPListItem li in WETLeftNavigation.Items
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
                                htmlOutput += renderTopLevelLink(WETLeftNavigation, oItem_1, level, langWeb);
                            }
                        }
                    }
                    finally
                    {
                    }
                    htmlOutput += "<br /><br /><br />";
                    writer.Write(htmlOutput);

                    // Nik20131114 - Need to use javascript to highlight the current section in the left menu, because with anchors ('#') we cannot
                    //               make a decision server side;
                    string selectorScript = "<script type=\"text/javascript\" src=\"/Style Library/js/leftmenuselector.js\"><" + "/" + "script>";

                    writer.Write(selectorScript);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex.Message + " " + ex.StackTrace);
            }

        }

        private string renderTopLevelLink(SPList aList, SPListItem aItem, string aLevel, string aLang)
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

            if (secondLevelCollItem.Count > 1)
            {
                // have children... render for expansion
                returnString = "<section><h3>" +
                                "<a href=\"" + urlLink + "\"  onclick=\"checkLeftMenuLinks();\">" + title + "</a></h3>";

                // render any children
                foreach (SPListItem oItem in secondLevelCollItem)
                {
                    returnString += renderSecondLevelLink(aList, oItem, Convert.ToString(aItem["Level"]), aLang);
                }

                // close off the heading
                returnString += "</section>";
            }
            else
            {
                // top level heading with no children
                returnString = "<section><h3><a href=\"" + urlLink + "\">" + title + "</a></h3></section>";
            }
            return returnString;
        }

        private string renderSecondLevelLink(SPList aList, SPListItem aItem, string aLevel, string aLang)
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

                returnString = "<ul>" +
                               "<li><a href=\"" + urlLink + "\" onclick=\"checkLeftMenuLinks();\">" + title + "</a>";

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

                        if (numberOfDots == 2)
                        {
                            title = Convert.ToString(item["Title"]);
                            returnString += "<li><a href=\"" + urlLink + "\"  onclick=\"checkLeftMenuLinks();\">" + title + "</a></li>";
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
