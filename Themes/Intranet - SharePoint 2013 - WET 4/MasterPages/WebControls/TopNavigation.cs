using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint.Publishing;
using Microsoft.SharePoint.Publishing.Navigation;
using WET.Theme.Intranet.Objects;

namespace WET.Theme.Intranet.WebControls
{
    [DefaultProperty("Text"),
   ToolboxData("<{0}:TopNavigation runat=\"server\"></{0}:TopNavigation>")]
    public class TopNavigation : WebControl
    {
        string langWeb;

        protected override void Render(HtmlTextWriter writer)
        {
            string htmlOutput = string.Empty;

            try
            {       
                langWeb = string.Empty;
                if (SPContext.Current.ListItem != null && PublishingPage.IsPublishingPage(SPContext.Current.ListItem))
                {
                    // figure out our language of the current label
                    PublishingPage publishingPage = PublishingPage.GetPublishingPage(SPContext.Current.ListItem);

                    if (publishingPage.PublishingWeb.Label != null)
                        langWeb = (publishingPage.PublishingWeb.Label.Title.Substring(0, 2).ToLower() == "en") ? "Eng" : "Fra";
                }
                else
                {
                    string cultISO = "";
                    if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("/eng/"))
                        cultISO = "en";
                    else
                        cultISO = "fr";
                    langWeb = (cultISO == "en") ? "Eng" : "Fra";
                }
                
                htmlOutput += "<nav class=\"wb-menu visible-md visible-lg wb-init wb-data-ajax-replace-inited wb-menu-inited wb-navcurr-inited\" id=\"wb-sm\" role=\"navigation\" typeof=\"SiteNavigationElement\" data-trgt=\"mb-pnl\">\r\n";
                htmlOutput += "<div class=\"pnl-strt container visible-md visible-lg nvbar\">\r\n";
                htmlOutput += "<h2>Topics menu</h2>\r\n";
                htmlOutput += "<div class=\"row\">\r\n";
                htmlOutput += "<ul class=\"list-inline menu\" role=\"menubar\">\r\n";
                htmlOutput += renderTopLevelLinks();
                htmlOutput += "</ul></div></div>";
                htmlOutput += "</nav>";

                writer.Write(htmlOutput);                
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex.Message + " " + ex.StackTrace);
            }
        }

        private string renderTopLevelLinks()
        {
            string topLinkContent = "";
            SPWeb currentSiteRoot = SPContext.Current.Web.Site.RootWeb;
            SPList lstTopNavigation = currentSiteRoot.Lists["WETTopNavigation"];            

            int tabIndex = 0;
            foreach(SPListItem item in lstTopNavigation.Items)
            {
                if(!item["Order" + langWeb].ToString().Contains('.'))
                {
                    topLinkContent += "<li><a tabindex=\"" + tabIndex.ToString() + "\" class=\"item\" role=\"menuitem\" aria-haspopup=\"true\" aria-posinset=\"1\" aria-setsize=\"3\" href=\"" + item["Url" + langWeb].ToString() + "\">";
                    topLinkContent += item["Title" + langWeb].ToString();
                    topLinkContent += "<span class=\"expicon glyphicon glyphicon-chevron-down\"></span></a>";
                    topLinkContent += renderSubLevelLinks(item["Order" + langWeb].ToString());
                    topLinkContent += "</li>";
                    tabIndex++;
                }                
            }
            return topLinkContent;
        }

        private string renderSubLevelLinks(string topLevelItemID)
        {
            string subLinkContent = "";
            SPWeb currentSiteRoot = SPContext.Current.Web.Site.RootWeb;
            SPList lstTopNavigation = currentSiteRoot.Lists["WETTopNavigation"];
            bool oneFound = false;

            int posinset = 1;
            foreach (SPListItem item in lstTopNavigation.Items)
            {
                if(item["Order" + langWeb].ToString().StartsWith(topLevelItemID + "."))
                {
                    if(!oneFound)
                    {
                        subLinkContent += "<ul class=\"sm list-unstyled\" id=\"project\" role=\"menu\" aria-expanded=\"false\" aria-hidden=\"true\">";
                        oneFound = true;
                    }
                    subLinkContent += "<li><a tabindex=\"-1\" role=\"menuitem\" aria-posinset=\"" + posinset.ToString() + "\" aria-setsize=\"9\" href=\"" + item["Url" + langWeb].ToString() + "\">" + item["Title" + langWeb].ToString() + "</a></li>";
                    posinset++;
                }                
            }
            if (oneFound)
                subLinkContent += "</ul>";

            return subLinkContent;
        }
    }
}