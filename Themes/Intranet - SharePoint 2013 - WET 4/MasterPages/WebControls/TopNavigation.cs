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
        #region Properties

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
                // setup the outer wrappers
                htmlOutput += "<div class=\"wet-boew-menubar mb-mega\"><div><ul class=\"mb-menu\" data-ajax-replace=\"";

                if (SPContext.Current.Web.Locale.TwoLetterISOLanguageName == "en")
                    htmlOutput += "/TopNavigation/menu-eng.txt\">";
                else
                    htmlOutput += "/TopNavigation/menu-fra.txt\">";

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

                string webUrl = SPContext.Current.Web.Url;
                using (SPSite site = new SPSite(webUrl))
                {
                    htmlOutput += renderTopLevelLink(langWeb);

                    // setup the outer wrappers
                    htmlOutput += "</ul></div></div>";

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
                sb.Append("<li><div><a href=\"/eng/pages/mega-menu.aspx?lvl=1\">Acquisition</a></div></li>");
                sb.Append("<li><div><a href=\"/eng/pages/mega-menu.aspx?lvl=2\">Stewardship</a></div></li>");
                sb.Append("<li><div><a href=\"/eng/pages/mega-menu.aspx?lvl=3\">Services for the public</a></div></li>");
                sb.Append("<li><div><a href=\"/eng/pages/mega-menu.aspx?lvl=4\">Services for LAC</a></div></li>");
            }

            return sb.ToString();
        }
    }//end class
}