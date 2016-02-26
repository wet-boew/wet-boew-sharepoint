using System;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Publishing;
using Microsoft.SharePoint.Publishing.Navigation;
using Microsoft.SharePoint.WebControls;
using WET.Theme.Intranet.Objects;

namespace WET.Theme.Intranet.WebControls
{
    [ToolboxData("<{0}:LanguageToggle runat=server></{0}:LanguageToggle>")]
    public class LanguageToggle : WebControl
    {

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            try
            {
                PublishingPage publishingPage;
                PublishingPage targetPage;
                string currentLang = "";
                if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("/eng/"))
                    currentLang = "en";
                else
                    currentLang = "fr";

                // Figure out what variation we are in and link back to the other language using the 
                if (SPContext.Current.ListItem != null && PublishingPage.IsPublishingPage(SPContext.Current.ListItem))
                {
                    publishingPage = PublishingPage.GetPublishingPage(SPContext.Current.ListItem);
                    VariationLabel label = publishingPage.PublishingWeb.Label;

                    //repass the querystring
                    string queryString = "";
                    if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString.ToString()))
                    {
                        queryString = "?" + HttpContext.Current.Request.QueryString.ToString();
                    }

                    // controls on the page can override the query string parameter if needed
                    if (this.Page.Master != null)
                    {
                        Master_Pages.WETIntranetPublishingMaster masterPage = (Master_Pages.WETIntranetPublishingMaster)this.Page.Master;
                        if (!String.IsNullOrEmpty(masterPage.LanguageFlipQueryString))
                            queryString = masterPage.LanguageFlipQueryString;
                    }
                    if (label == null)
                    {
                        // no variations... use the current users local
                        string langlabel = HttpContext.GetGlobalResourceObject("WET", "OtherLanguageText", SPContext.Current.Web.Locale).ToString();
                        string Languagecontrol = "";

                        if (currentLang.Equals("en"))
                            Languagecontrol = "<a href=\"" + publishingPage.Uri.AbsoluteUri + queryString + "\" lang=\"" + currentLang + "\" xml:lang=\"" + currentLang + "\" onclick=\"javascript:OnSelectionChange(1036); return false;\"><span>" + langlabel + "</span></a>";
                        else
                            Languagecontrol = "<a href=\"" + publishingPage.Uri.AbsoluteUri + queryString + "\" lang=\"" + currentLang + "\" xml:lang=\"" + currentLang + "\" onclick=\"javascript:OnSelectionChange(1033); return false;\"><span>" + langlabel + "</span></a>";
                        this.Controls.Add(new LiteralControl(Languagecontrol));
                    }
                    else
                    {
                        // handle the variations
                        // find our root publishing web

                        PublishingWeb topPubWeb = publishingPage.PublishingWeb;
                        while (topPubWeb.IsRoot == false)
                        {
                            topPubWeb = topPubWeb.ParentPublishingWeb;
                        }

                        // iterate through the variation urls and find the lable from the other language
                        foreach (PublishingWeb aPubWeb in topPubWeb.GetPublishingWebs())
                        {
                            if (aPubWeb.Label != null && (aPubWeb.Label.Title != publishingPage.PublishingWeb.Label.Title) &&
                                ((aPubWeb.Label.Title.ToLower() == "eng") || (aPubWeb.Label.Title.ToLower() == "fra")))
                            {
                                // then we have the pubweb from the other variation... 
                                label = aPubWeb.Label;
                                break;
                            }
                        }
                        if (label == null)
                        {
                            throw new System.ArgumentException(HttpContext.GetGlobalResourceObject("WET", "Error_CantFindPubWebLabel", SPContext.Current.Web.Locale).ToString(), "listItem");
                        }
                        else
                        {
                            targetPage = publishingPage.GetVariation(label);
                            if (targetPage != null)
                            {
                                string currenturl = HttpContext.Current.Request.Path.ToString().ToLower();

                                string reverseurl = targetPage.Uri.AbsoluteUri;
                                string langlabel = HttpContext.GetGlobalResourceObject("WET", "OtherLanguageText", SPContext.Current.Web.Locale).ToString();
                                string propLang = (publishingPage.PublishingWeb.Label.Language.Substring(0, 2) == "en") ? "fr" : "en";

                                this.Controls.Add(new LiteralControl("<script type=\"text/javascript\">" + System.Environment.NewLine +
                                   "function OnSelectionChange2(value){" +
                                   System.Environment.NewLine + "var today = new Date();" +
                                   System.Environment.NewLine + "var oneYear = new Date(today.getTime() + 365 * 24 * 60 * 60 * 1000);" +
                                   System.Environment.NewLine + "var url = \"" + reverseurl + queryString + "\";" +
                                   System.Environment.NewLine + "document.cookie = \"lcid=\" + value + \";path=/;expires=\" + oneYear.toGMTString();" +
                                   System.Environment.NewLine + "window.location.href = url;" +
                                   System.Environment.NewLine + "}" +
                                   System.Environment.NewLine +
                                   "</script>"));
                                string lang = publishingPage.PublishingWeb.Label.Language.Substring(0, 2);
                                string controlContent = "";
                                if (lang == "en")
                                {
                                    controlContent = @"<section id=""wb-lng"">
                                      <h2>Language selection</h2>     
                                         <ul class=""list-inline"">
                                        <li><a lang=""fr"" href=""" + reverseurl + queryString + @""" onclick=""javascript: OnSelectionChange2(1036); return false;"">Fran&ccedil;ais</a></li>
                                         </ul>
                                    </section>";
                                }
                                else
                                {
                                    {
                                        controlContent = @"<section id=""wb-lng"">
                                        <h2>S&eacute;lection de langue</h2>     
                                         <ul class=""list-inline"">
                                        <li><a lang=""en"" href=""" + reverseurl + queryString + @""" onclick=""javascript: OnSelectionChange2(1033); return false;"">English</a></li>
                                        </ul>
                                    </section>";
                                    }
                                }
                                this.Controls.Add(new LiteralControl(controlContent));
                            }
                        }
                    }
                }
                else
                {
                    //repass the querystring
                    string queryString = "";
                    if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString.ToString()))
                    {
                        queryString = "?" + HttpContext.Current.Request.QueryString.ToString();
                    }

                    // controls on the page can override the query string parameter if needed
                    if (this.Page.Master != null)
                    {
                        Master_Pages.WETIntranetPublishingMaster masterPage = (Master_Pages.WETIntranetPublishingMaster)this.Page.Master;
                        if (!String.IsNullOrEmpty(masterPage.LanguageFlipQueryString))
                            queryString = masterPage.LanguageFlipQueryString;
                    }

                    string url = SPContext.Current.Web.Url;
                    // no variations... use the current users local
                    string langlabel = HttpContext.GetGlobalResourceObject("WET", "OtherLanguageText", SPContext.Current.Web.Locale).ToString();

                    string Languagecontrol = "";
                    if (currentLang.Equals("en"))
                        Languagecontrol = "<a href=\"" + url + queryString + "\" lang=\"" + currentLang + "\" xml:lang=\"" + currentLang + "\" onclick=\"javascript:OnSelectionChange(1036); return false;\"><span>" + langlabel + "</span></a>";
                    else
                        Languagecontrol = "<a href=\"" + url + queryString + "\" lang=\"" + currentLang + "\" xml:lang=\"" + currentLang + "\" onclick=\"javascript:OnSelectionChange(1033); return false;\"><span>" + langlabel + "</span></a>";
                    this.Controls.Add(new LiteralControl(Languagecontrol));
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex.Message + " " + ex.StackTrace);
            }
        }
    }
}
