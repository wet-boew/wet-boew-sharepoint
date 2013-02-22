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
using WET.Theme.Intranet.Objects;

namespace WET.Theme.Intranet.WebControls
{
    [ToolboxData("<{0}:FooterNavigation runat=\"server\" />")]
    public class FooterNavigation : WebControl
    {        
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
                        SPList footerNavigation = null;
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
                                    if (web.Lists.TryGetList("WETFooterNavigation") != null)
                                    {
                                        footerNavigation = web.Lists["WETFooterNavigation"];
                                        SPQuery oQuery = new SPQuery();
                                        oQuery.Query = "<Where><IsNotNull><FieldRef Name='NavURL'/></IsNotNull></Where>" +
                                                "<OrderBy><FieldRef Name='RowOrder' /></OrderBy>";

                                        if (footerNavigation != null)
                                        {
                                            collListItems = footerNavigation.GetItems(oQuery);                                            
                                            int counter = 0;
                                            if (collListItems != null)
                                            {
                                                foreach (SPListItem item in collListItems)
                                                {
                                                    counter += 1;

                                                    string linkClass = string.Empty;
                                                    // set the rel = license for terms and conditions being the title of the link

                                                    

                                                    linkClass = " class=\"ui-link\"";
                                                    
                                                    // then this is one of the top links... add it to the 
                                                    // <li class="terms"><a href="#" rel="license">Terms and conditions</a></li>
                                                    topLinksHTML += "<li><a href=\"" + item["NavURL"].ToString() + "\"" + linkClass + ">" + item.Title + "</a></li>";
                                                }
                                            }

                                            // now do the sections
                                            foreach (SPListItem aFolderItem in footerNavigation.Folders)
                                            {
                                                // its one of the folders... render it and add it to sectionsHTML
                                                sectionsHTML += renderSection(aFolderItem.Folder, footerNavigation);
                                            }
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
                            // wrap the html in a <div id="cn-ft-tctr"><ul> ... </ul></div>
                            output.Write("<div id=\"gcwu-tctr\"><ul>" + topLinksHTML + "</ul></div><div class=\"clear\"></div>");
                        }

                        // write out the sectionsHTML to output as is..
                        output.Write(sectionsHTML);
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
               SPList footerNavigation = null;
               SPListItemCollection collListItems;

               try
               {
                   if (SPContext.Current.Web.Lists.TryGetList("WETFooterNavigation") != null)
                   {
                       footerNavigation = SPContext.Current.Web.Lists["WETFooterNavigation"];
                       SPQuery oQuery = new SPQuery();
                       oQuery.Query = "<Where><IsNotNull><FieldRef Name='NavURL'/></IsNotNull></Where>" +
                                       "<OrderBy><FieldRef Name='RowOrder' /></OrderBy>";

                       if (footerNavigation != null)
                       {
                           collListItems = footerNavigation.GetItems(oQuery);

                           int counter = 0;
                           foreach (SPListItem item in collListItems)
                           {
                               counter += 1;

                               string linkClass = string.Empty;
                               string linkRel = string.Empty;

                               if (item.Title.ToLower() == HttpContext.GetGlobalResourceObject("WET", "TermsAndConditionsText", SPContext.Current.Web.Locale).ToString().ToLower())
                               {
                                   linkRel = " rel=\"license\"";
                               }
                               else
                               {
                                   linkRel = string.Empty;
                               }

                               // set the class based on the counter
                               if (counter != collListItems.Count)
                               {
                                   linkClass = " class=\"gcwu-tc\"";
                               }
                               else
                               {
                                   linkClass = " class=\"gcwu-tr\"";
                               }
                               // then this is one of the top links... add it to the 
                               // <li class="terms"><a href="#" rel="license">Terms and conditions</a></li>
                               topLinksHTML += "<li" + linkClass + "><a href=\"" + item["NavURL"].ToString() + "\"" + linkRel + ">" + item.Title + "</a></li>";
                           }

                           // now do the sections
                           foreach (SPListItem aFolderItem in footerNavigation.Folders)
                           {
                               // its one of the folders... render it and add it to sectionsHTML
                               sectionsHTML += renderSection(aFolderItem.Folder, footerNavigation);
                           }
                       }
                   }
                }
                catch (Exception ex)
                {
                    Logger.WriteLog(ex.Message + " " + ex.StackTrace);
                }  
                // write out the topLinksHTML to output
                if (!string.IsNullOrEmpty(topLinksHTML))
                {
                    // wrap the html in a <div id="cn-ft-tctr"><ul> ... </ul></div>
                    output.Write("<div id=\"gcwu-tctr\"><ul>" + topLinksHTML + "</ul></div><div class=\"clear\"></div>");
                }

                // write out the sectionsHTML to output as is..
                output.Write(sectionsHTML);
            }
            
        }

        private string renderSection(SPFolder folderItem, SPList footerNavigation)
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
                collListItems = footerNavigation.GetItems(oQuery);

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
                sectionHeaderHTML = "<h4 class=\"gcwu-col-head\">" + folderItem.Name + "</h4>";
            }
            else
            {
                // add in the link
                sectionHeaderHTML = "<h4 class=\"gcwu-col-head\"><a href=\"" + sectionHeaderHTML + "\">" + folderItem.Name + "</a></h4>";

            }

            // wrap the list of child items if there are any
            sectionHTML = (string.IsNullOrEmpty(sectionHTML)) ? string.Empty : "<ul>" + sectionHTML + "</ul>";

            return "<section><div class=\"span-2\">" + sectionHeaderHTML + sectionHTML + "</div></section>";
        }
    }
}