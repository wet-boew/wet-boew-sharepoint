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
using SPWET4.Objects;

namespace SPWET4.WebControls
{
    [DefaultProperty("Text"),
   ToolboxData("<{0}:WETTopNavigation runat=\"server\"></{0}:WETTopNavigation>")]
    public class WETTopNavigation : WebControl
    {
        #region Properties

        PortalSiteMapProvider map = PortalSiteMapProvider.GlobalNavSiteMapProvider;
        const string MegaMenuClass = " mb-megamenu";
        const string TabbedMenuClass = "";

        public enum ControlModes { MegaMenu, TabbedMenu }

        [DefaultValue(ControlModes.MegaMenu)]
        [Description("Select the type of Menu from the dropdown list.")]
        [Browsable(true)]
        [Category("Appearance")]
        public ControlModes ControlMode { get; set; }

        #endregion Properties

        char[] dot = { '.' };

        protected override void Render(HtmlTextWriter writer)
        {
            string htmlOutput = string.Empty;

            try
            {
                string langWeb = string.Empty;
                if (SPContext.Current.ListItem != null && PublishingPage.IsPublishingPage(SPContext.Current.ListItem))
                {
                    // figure out our language of the current label
                    PublishingPage publishingPage = PublishingPage.GetPublishingPage(SPContext.Current.ListItem);

                    if (publishingPage.PublishingWeb.Label != null)
                        langWeb = (publishingPage.PublishingWeb.Label.Title.Substring(0, 2).ToLower() == "en") ? "eng" : "fra";
                }
                else
                {
                    string cultISO = "";
                    if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("/eng/"))
                        cultISO = "en";
                    else
                        cultISO = "fr";
                    langWeb = (cultISO == "en") ? "eng" : "fra";
                }

                string dataAjaxFetch = "";
                if (SPContext.Current.Web.Locale.TwoLetterISOLanguageName == "en")
                    dataAjaxFetch += "/TopNavigationFiles/menu-include-en.html";
                else
                    dataAjaxFetch += "/TopNavigationFiles/menu-include-fr.html";

                htmlOutput += "<nav role=\"navigation\" id=\"wb-sm\" data-ajax-fetch=\"" + dataAjaxFetch + "\" data-trgt=\"mb-pnl\" class=\"wb-menu visible-md visible-lg\" typeof=\"SiteNavigationElement\">";
                htmlOutput += "<div class=\"container nvbar\">";
                htmlOutput += "<h2>Topics menu</h2>";
                htmlOutput += "<ul class=\"list-inline menu\">";
                htmlOutput += renderTopLevelLink(langWeb);
                htmlOutput += "</ul>";
                htmlOutput += "</div>";
                htmlOutput += "</nav>";

                string webUrl = SPContext.Current.Web.Url;
                using (SPSite site = new SPSite(webUrl))
                {
                    writer.Write(htmlOutput);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex.Message + " " + ex.StackTrace);
            }
        }

        private string renderTopLevelLink(string aLang)
        {
            System.Text.StringBuilder sb = new StringBuilder();
            string urlLink = string.Empty;
            string title = string.Empty;

            //TODO - Replace by static top nav.
            if (aLang == "eng")
            {
                sb.Append("<li><div><a href=\"/eng/pages/mega-menu.aspx?lvl=1\">Discover the Collection</a></div></li>");
                sb.Append("<li><div><a href=\"/eng/pages/mega-menu.aspx?lvl=2\">Online research</a></div></li>");
                sb.Append("<li><div><a href=\"/eng/pages/mega-menu.aspx?lvl=3\">Copies & Visiting</a></div></li>");
                sb.Append("<li><div><a href=\"/eng/pages/mega-menu.aspx?lvl=4\">Services for Professionals</a></div></li>");
            }
            else
            {
                sb.Append("<li><div><a href=\"/fra/pages/mega-menu.aspx?lvl=1\">Découvrez la collection</a></div></li>");
                sb.Append("<li><div><a href=\"/fra/pages/mega-menu.aspx?lvl=2\">Recherche en ligne</a></div></li>");
                sb.Append("<li><div><a href=\"/fra/pages/mega-menu.aspx?lvl=3\">Reproductions et Visites</a></div></li>");
                sb.Append("<li><div><a href=\"/fra/pages/mega-menu.aspx?lvl=4\">Services pour les professionnels</a></div></li>");
            }

            return sb.ToString();
        }

    }
}
