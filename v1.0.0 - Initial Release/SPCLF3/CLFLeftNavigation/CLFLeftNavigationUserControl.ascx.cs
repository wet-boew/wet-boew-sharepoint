using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SharePoint.Publishing;
using Microsoft.SharePoint.Publishing.Navigation;
using SPCLF3.Objects;


namespace SPCLF3.CLFLeftNavigation
{
    public partial class CLFLeftNavigationUserControl : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
             /// <summary>
        /// Ceates content in this manner - all outer divs are in the layout.
        /// Non selected section
        /// <section>
        ///    <h3><a href="#">Section 1</a></h3>
        ///    <ul>
        ///        <li><a href="#">Item 1a</a>
        ///            <ul>
        ///                <li><a href="#">Item 1ai</a></li>
        ///                <li><a href="#">Item 1aii</a></li>
        ///            </ul>
        ///        </li>
        ///        <li><a href="#">Item 1b</a></li>
        ///        <li><a href="#">Item 1c</a></li>
        ///    </ul>
        /// </section>
        /// Selected Section
        /// <section>
        ///    <h3><a href="#" class="nav-current">Current section (4) example </a></h3>
        ///    <ul>
        ///        <li><a href="#">Item 4a</a></li>
        ///        <li><a href="#" class="nav-current">Current item (4b) example</a>
        ///            <ul>
        ///                <li><a href="#">Item 4bi</a></li>
        ///                <li><a href="#" class="nav-current">Current sub (4bii) item example</a></li>
        ///                <li><a href="#">Item 4biii</a></li>
        ///            </ul>
        ///        </li>
        ///        <li><a href="#">Item 4c</a></li>
        ///    </ul>
        /// </section>
        /// </summary>

     string htmlOutput = string.Empty;
     string currentNavClass = string.Empty;
     string selectedNav = string.Empty;

            try
            {
                // setup the outer wrappers
                htmlOutput += "";

                // current nav css
                if (!String.IsNullOrEmpty(Request.QueryString["selected"]))
                {
                    currentNavClass = "class=\"nav-current\"";
                    selectedNav = Request.QueryString["selected"];
                }


                // figure out our language of the current label
                PublishingPage publishingPage = PublishingPage.GetPublishingPage(SPContext.Current.ListItem);
                string langWeb = (publishingPage.PublishingWeb.Label.Title.Substring(0, 2).ToLower() == "en") ? "eng" : "fra";
                
                SPWeb web = null;
                SPList CLF3LeftNavigation = null;
                List<SPListItem> firstLevelCollItem;

                try
                {

                    web = SPContext.Current.Site.RootWeb.Webs[langWeb];
                    CLF3LeftNavigation = web.Lists["CLF3LeftNavigation"];
                    
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
                    if (web != null)
                    {
                        web.Dispose();
                    }
                }

                // setup the outer wrappers                
                TableRow itemRow = new TableRow();
                itemRow.Height = 20;
                TableCell itemcell = new TableCell();
                Label lbl = new Label();
                lbl.Text = "<div class='cn-left-col-wp'>" + htmlOutput + "</div>";
                itemcell.Controls.Add(lbl);
                itemRow.Cells.Add(itemcell);
                CLF3LeftNavigationTable.Rows.Add(itemRow);

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

            if (selected == aLevel)
            {
                currentNavClass = " class=\"nav-current\"";
            }

            if (secondLevelCollItem.Count > 1)
            {
                // have children... render for expansion
                returnString = "<section><h3>" +
                                "<a href=\"" + urlLink + "?selected=" + aLevel + "\"" + currentNavClass + ">" + title + "</a></h3>";

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
                returnString = "<section><h3><a href=\"" + urlLink + "?selected=" + aLevel + "\"" + currentNavClass + ">" + title + "</a></h3></section>";
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

                    if (selected == level)
                    {
                        currentNavClass = " class=\"nav-current\"";
                    }
                    else {
                        currentNavClass = string.Empty;
                    }

                    returnString = "<ul>" +
                                   "<li><a href=\"" + urlLink + "?selected=" + level + "\"" + currentNavClass + ">" + title + "</a>";

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

                            if (selected == thirdLevel)
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
                                urlLink = Convert.ToString(item["UrlLink"]);

                                returnString += "<li><a href=\"" + urlLink + "?selected=" + thirdLevel + "\"" + currentNavClass + ">" + title + "</a></li>";
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

                          