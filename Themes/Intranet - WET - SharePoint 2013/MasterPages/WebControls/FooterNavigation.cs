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
            //Shireeh Adding to avoid annonymous access issue 
            var siteId = SPContext.Current.Site.ID;
            var webId = SPContext.Current.Web.ID;
            // System.Threading.Thread.CurrentThread.CurrentUICulture.LCID;
            int lcid = HttpContext.Current.Request.Url.Segments.Contains("fra/") ? 1036 : 1033;
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                try
                {
                    using (SPSite site = new SPSite(siteId))
                    {
                        using (SPWeb web = site.OpenWeb(webId))
                        {
                            if (SPContext.Current.Web.ParentWeb.IsRootWeb) //only display on landing page for language variations, one level down from rootweb.
                            {
                                //Using the new site to get the root instead of the Context. 
                                if (SPContext.Current.ListItemServerRelativeUrl.ToLower() == (web.ServerRelativeUrl.TrimEnd('/') + "/" + web.RootFolder.WelcomePage).ToLower())
                                {
                                    string webUrl = string.Empty;
                                    SPList footerNavigation = null;
                                    SPListItemCollection collListItems;

                                    try
                                    {

                                        webUrl = site.RootWeb.Url;
                                        using (SPWeb rootWeb = site.OpenWeb())
                                        {
                                            if (rootWeb.Lists.TryGetList("LACFooterImages") != null)
                                            {
                                                footerNavigation = rootWeb.Lists["LACFooterImages"];
                                                SPQuery oQuery = new SPQuery();
                                                if (lcid == 1036)
                                                {
                                                    oQuery.Query = "<Where><Eq><FieldRef Name='FrenchImage' /><Value Type='Choice'>YES</Value></Eq></Where>" +
                                                        "<OrderBy><FieldRef Name='ItemOrder' /></OrderBy>";
                                                }
                                                else
                                                {
                                                    oQuery.Query = "<Where><Eq><FieldRef Name='EnglishImage' /><Value Type='Choice'>YES</Value></Eq></Where>" +
                                                        "<OrderBy><FieldRef Name='ItemOrder' /></OrderBy>";
                                                }
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

                                                            if (counter == 1)
                                                            {
                                                                linkClass = " class=\"span-2 align-center row-start\" ";
                                                            }
                                                            if (counter == 2)
                                                            {
                                                                linkClass = " class=\"span-2 align-center\" ";
                                                            }

                                                            if (counter == 3)
                                                            {
                                                                linkClass = " class=\"span-2 align-center row-end\" ";
                                                            }

                                                            if (lcid == 1036)
                                                                topLinksHTML += "<div" + linkClass + " style=\"margin-right:130px;\"><a href=\"" + item["French Url"].ToString() + "\">" + "<img class= \"image-actual\" src='" + item.File.ServerRelativeUrl + "'>" + "</a></div>";
                                                            else
                                                                topLinksHTML += "<div" + linkClass + " style=\"margin-right:130px;\"><a href=\"" + item["English Url"].ToString() + "\">" + "<img class= \"image-actual\" src='" + item.File.ServerRelativeUrl + "'>" + "</a></div>";

                                                        }
                                                    }

                                                    // now do the sections

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
                                        //output.Write("<div class=\"wet-boew-responsiveimg\" data-picture=\"data-picture\" style=\"padding-left:50px\">" + topLinksHTML + "</div>");
                                        output.Write("<div style=\"padding-left:0px\">" + topLinksHTML + "</div>");
                                    }

                                    // write out the sectionsHTML to output as is..
                                    output.Write(sectionsHTML);
                                }
                            }
                        }
                    }




                }
                catch (Exception ex)
                {
                    output.Write("oTech.Lac.Intranet.WebControls: FooterImageNav exception message: " + ex.ToString());
                }
            });
        }
    }
}
