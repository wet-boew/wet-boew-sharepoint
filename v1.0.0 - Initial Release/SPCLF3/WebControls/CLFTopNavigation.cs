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
using SPCLF3.Objects;

namespace SPCLF3.WebControls
{
    [DefaultProperty("Text"),
   ToolboxData("<{0}:CLFTopNavigation runat=\"server\"></{0}:CLFTopNavigation>")]
    public class CLFTopNavigation : WebControl
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
                // setup the outer wrappers
                htmlOutput += "<div class=\"wet-boew-menubar mb-mega\"><div><ul class=\"mb-menu\" data-ajax-replace=\"";
                
                if(SPContext.Current.Web.Locale.TwoLetterISOLanguageName == "en")
                    htmlOutput += "/Navigation/menu-eng.txt\">";
                else
                    htmlOutput += "/Navigation/menu-fra.txt\">";

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
                    string cultISO = System.Threading.Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;
                    langWeb = (cultISO == "en") ? "eng" : "fra";    
                }
                
                string webUrl = SPContext.Current.Web.Url;

                //TODO - Remove this debug logic;
#if DEBUG
                if (webUrl.Contains("l41-106306"))
                    webUrl = webUrl.Replace("l41-106306", "localhost");
#endif

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
            //string isFirstClass = string.Empty;
            string urlLink = string.Empty;
            string title = string.Empty;
            //string level = string.Empty;
            //string colPosition = string.Empty;

            /*List<SPListItem> secondLevelCollItem = (from SPListItem li in aList.Items
                                                    where Convert.ToString(li["Level"]).StartsWith(aLevel)
                                                    && Convert.ToString(li["Level"]).Split(dot[0]).Length - 1 == 1
                                                    orderby li["SortOrder"]
                                                    select li).ToList<SPListItem>();*/

            //TODO - Replace by static top nav.
            if (aLang == "eng")
            {
                sb.Append("<li><div><a href=\"http://win-1elcne6ohdq/eng/pages/mega-menu.aspx?lvl=1\">Discover the Collection</a></div></li>");
                sb.Append("<li><div><a href=\"http://win-1elcne6ohdq/eng/pages/mega-menu.aspx?lvl=2\">Online research by Topic</a></div></li>");
                sb.Append("<li><div><a href=\"http://win-1elcne6ohdq/eng/pages/mega-menu.aspx?lvl=3\">Copies & Visiting</a></div></li>");
                sb.Append("<li><div><a href=\"http://win-1elcne6ohdq/eng/pages/mega-menu.aspx?lvl=4\">Services for Professionals</a></div></li>");
            }

            
            return sb.ToString();
        }

    }
}
