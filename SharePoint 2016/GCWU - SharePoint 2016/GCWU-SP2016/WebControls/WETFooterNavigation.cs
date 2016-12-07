using System;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint.Publishing;
using System.Linq;
using SPWET4.Objects;

namespace SPWET4.WebControls
{
    /*
     *This control requires a CLF3FooterNavigation Sharepoint list that needs to be renamed to WET4FooterNavigation to be name consistent
     */
    [ToolboxData("<{0}:WETFooterNavigation runat=\"server\" />")]
    public class WETFooterNavigation : WebControl
    {
        // Creates content in the following manner
        // 1:  all links not in folders will be rendered in the first div in the order added to the list
        //<div id="cn-ft-tctr">
        //    <ul>
        //        <li class="terms"><a href="#" rel="license">Terms and conditions</a></li>
        //        <li class="trans"><a href="#">Transparency</a></li>
        //    </ul>
        //</div>
        //<div class="clear"></div>
        // 2:  all folders will be rendered with the link with the title matching the folder name being the link for the folder
        // 3:  if no link matches the folder name, it will be rendered as text and not a link
        // 4:  all other links in folders will be rendered in the order they are added to the folder as <LI> items
        //<section>
        //    <div class="span-2">
        //        <h4 class="col-head"><a href="#">About us</a></h4>
        //        <ul>
        //            <li><a href="#">Our Mandate</a></li>
        //            <li><a href="#">Our Minister</a></li>
        //        </ul>
        //    </div>
        //</section>
        /// <summary> 
        /// Render this control to the output parameter specified.
        /// </summary>
        /// <param name="output"> The HTML writer to write out to </param>
        protected override void Render(HtmlTextWriter output)
        {
            string topLinksHTML = string.Empty;
            string sectionsHTML = string.Empty;
            //Create a link back to the root of the variation
            if (SPContext.Current.ListItem != null && PublishingPage.IsPublishingPage(SPContext.Current.ListItem))
            {
                try
                {
                    if (PublishingPage.IsPublishingPage(SPContext.Current.ListItem))
                    {
                        PublishingPage publishingPage = PublishingPage.GetPublishingPage(SPContext.Current.ListItem);
                        // Check to see if Variations are enabled or not.... if the label is null then no variations
                        string langWeb = (publishingPage.PublishingWeb.Label == null) ?
                                                    string.Empty :
                                                    (publishingPage.PublishingWeb.Label.Language.Substring(0, 2).ToLower() == "en") ? "eng" : "fra";
                        string webUrl = string.Empty;
                        SPList cLFFooterNavigation = null;
                        SPListItemCollection collListItems;

                        try
                        {
                            if (langWeb != string.Empty)
                            {
                                SPWebCollection webs = SPContext.Current.Site.RootWeb.Webs;
                                SPWeb tempWeb = webs[langWeb];
                                webUrl = tempWeb.Url;
                                tempWeb.Dispose();
                            }
                            else
                            {
                                webUrl = SPContext.Current.Site.RootWeb.Url;
                            }

                            using (SPSite site = new SPSite(webUrl))
                            {
                                using (SPWeb web = site.OpenWeb())
                                {
                                    cLFFooterNavigation = web.Lists.TryGetList("CLF3FooterNavigation");

                                    SPQuery oQuery = new SPQuery();
                                    oQuery.Query = "<Where><IsNotNull><FieldRef Name='NavURL'/></IsNotNull></Where>" +
                                            "<OrderBy><FieldRef Name='RowOrder' /></OrderBy>";

                                    if (cLFFooterNavigation != null)
                                    {
                                        collListItems = cLFFooterNavigation.GetItems(oQuery);

                                        int counter = 0;
                                        foreach (SPListItem item in collListItems)
                                        {
                                            counter += 1;

                                            string linkRel = string.Empty;
                                            // set the rel = license for terms and conditions being the title of the link

                                            if (item.Title.ToLower() == HttpContext.GetGlobalResourceObject("WET4", "TermsAndConditionsText", SPContext.Current.Web.Locale).ToString().ToLower())
                                            {
                                                linkRel = " rel=\"license\"";
                                            }
                                            else
                                            {
                                                linkRel = string.Empty;
                                            }

                                            // then this is one of the top links... add it to the 
                                            // <li class="terms"><a href="#" rel="license">Terms and conditions</a></li>
                                            topLinksHTML += "<li><a href=\"" + item["NavURL"].ToString() + "\"" + linkRel + ">" + item.Title + "</a></li>";
                                        }

                                        // now do the sections
                                        foreach (SPListItem aFolderItem in cLFFooterNavigation.Folders)
                                        {
                                            // its one of the folders... render it and add it to sectionsHTML
                                            sectionsHTML += renderSection(aFolderItem.Folder, cLFFooterNavigation);
                                        }
                                    }
                                }
                            }
                        }
                        finally
                        {
                        }
                        // write out the topLinksHTML to output
                        if (!string.IsNullOrEmpty(topLinksHTML))
                        {
                            output.Write("<ul id=\"gc-tctr\" class=\"list-inline\">" + topLinksHTML + "</ul><div class=\"row\">");
                        }

                        // write out the sectionsHTML to output as is..
                        output.Write(sectionsHTML);

                        if (!string.IsNullOrEmpty(topLinksHTML))
                        {
                            output.Write("</div>");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteLog(ex.Message + " " + ex.StackTrace);
                }
            }
            else
            {
                //Nik20121026 - The current site's template is a collaboration one;
                // Check to see if Variations are enabled or not.... if the label is null then no variations
                SPList cLFFooterNavigation = null;
                SPListItemCollection collListItems;

                try
                {
                    cLFFooterNavigation = SPContext.Current.Web.Lists["CLF3FooterNavigation"];
                    SPQuery oQuery = new SPQuery();
                    oQuery.Query = "<Where><IsNotNull><FieldRef Name='NavURL'/></IsNotNull></Where>" +
                                    "<OrderBy><FieldRef Name='RowOrder' /></OrderBy>";

                    if (cLFFooterNavigation != null)
                    {
                        collListItems = cLFFooterNavigation.GetItems(oQuery);

                        int counter = 0;
                        foreach (SPListItem item in collListItems)
                        {
                            counter += 1;

                            string linkRel = string.Empty;

                            if (item.Title.ToLower() == HttpContext.GetGlobalResourceObject("WET4", "TermsAndConditionsText", SPContext.Current.Web.Locale).ToString().ToLower())
                            {
                                linkRel = " rel=\"license\"";
                            }
                            else
                            {
                                linkRel = string.Empty;
                            }

                            // then this is one of the top links... add it to the 
                            // <li class="terms"><a href="#" rel="license">Terms and conditions</a></li>
                            topLinksHTML += "<li><a href=\"" + item["NavURL"].ToString() + "\"" + linkRel + ">" + item.Title + "</a></li>";
                        }

                        // now do the sections
                        foreach (SPListItem aFolderItem in cLFFooterNavigation.Folders)
                        {
                            // its one of the folders... render it and add it to sectionsHTML
                            sectionsHTML += renderSection(aFolderItem.Folder, cLFFooterNavigation);
                        }
                    }
                }
                catch { }
                // write out the topLinksHTML to output
                if (!string.IsNullOrEmpty(topLinksHTML))
                {
                    output.Write("<ul id=\"gc-tctr\" class=\"list-inline\">" + topLinksHTML + "</ul><div class=\"row\">");
                }

                // write out the sectionsHTML to output as is..
                output.Write(sectionsHTML);

                if (!string.IsNullOrEmpty(topLinksHTML))
                {
                    output.Write("</div>");
                }
            }

        }

        private string renderSection(SPFolder folderItem, SPList cLFFooterNavigation)
        {
            string sectionHTML = string.Empty;
            string sectionHeaderHTML = string.Empty;
            SPQuery oQuery = new SPQuery();
            oQuery.Folder = folderItem;
            // get only those links with a NavURL
            oQuery.Query = "<Where><IsNotNull><FieldRef Name='NavURL'/></IsNotNull></Where>" +
                           "<OrderBy><FieldRef Name='RowOrder' /></OrderBy>";
            SPListItemCollection collListItems;

            try
            {
                collListItems = cLFFooterNavigation.GetItems(oQuery);

                foreach (SPListItem childItem in collListItems)
                {
                    // if the childItem has the same title as the folderItem then this is the
                    // link associated with the section
                    // if no child item has the same title then the section is rendered with only a title
                    if (childItem["NavURL"] != null)
                    {
                        if (childItem.Title == folderItem.Name)
                        {
                            sectionHeaderHTML = childItem["NavURL"].ToString();
                        }
                        else
                        {
                            // then it is a link... add it to the section URL
                            sectionHTML += "<li><a href=\"" + childItem["NavURL"].ToString() + "\">" + childItem.Title + "</a></li>";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex.Message + " " + ex.StackTrace);
            }

            if (string.IsNullOrEmpty(sectionHeaderHTML))
            {
                // then there is no link for this header... just put out the text
                sectionHeaderHTML = "<h3>" + folderItem.Name + "</h3>";
            }
            else
            {
                // add in the link
                sectionHeaderHTML = "<h3><a href=\"" + sectionHeaderHTML + "\">" + folderItem.Name + "</a></h3>";

            }

            // wrap the list of child items if there are any
            sectionHTML = (string.IsNullOrEmpty(sectionHTML)) ? string.Empty : "<ul class=\"list-unstyled\">" + sectionHTML + "</ul>";

            return "<section class=\"col-sm-3\">" + sectionHeaderHTML + sectionHTML + "</section>";
        }
    }
}
