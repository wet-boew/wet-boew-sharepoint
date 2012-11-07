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
using SPCLF3.Objects;

namespace SPCLF3.WebControls
{
    [ToolboxData("<{0}:Language runat=server></{0}:Language>")]
    public class Language : WebControl
    {

        protected override void  OnPreRender(EventArgs e)
        {
 	        base.OnPreRender(e);

            try
            {
                PublishingPage publishingPage;
                PublishingPage targetPage;

                // figure out what variation we are in and link back to the other language using the 

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
                        Master_Pages.CLF3PublishingMaster masterPage = (Master_Pages.CLF3PublishingMaster)this.Page.Master;
                        if (!String.IsNullOrEmpty(masterPage.LanguageFlipQueryString))
                            queryString = masterPage.LanguageFlipQueryString;
                    }

                    // check for variations... if we have variations then use the currentThreads culture to figure out our language rather than the webs 
                    if (label == null)
                    {
                        // no variations... use the current users local
                        string langlabel = HttpContext.GetGlobalResourceObject("CLF3", "OtherLanguageText", System.Threading.Thread.CurrentThread.CurrentUICulture).ToString();
                        string propLang = System.Threading.Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;
                        string Languagecontrol = (System.Threading.Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName == "en") ?
                            "<a href=\"" + publishingPage.Uri.AbsoluteUri + queryString + "\" lang=\"" + propLang + "\" xml:lang=\"" + propLang + "\" onclick=\"javascript:OnSelectionChange(1036); return false;\"><span>" + langlabel + "</span></a>" :
                            "<a href=\"" + publishingPage.Uri.AbsoluteUri + queryString + "\" lang=\"" + propLang + "\" xml:lang=\"" + propLang + "\" onclick=\"javascript:OnSelectionChange(1033); return false;\"><span>" + langlabel + "</span></a>";
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
                            if ((aPubWeb.Label.Title != publishingPage.PublishingWeb.Label.Title) &&
                                ((aPubWeb.Label.Title.ToLower() == "eng") || (aPubWeb.Label.Title.ToLower() == "fra")))
                            {
                                // then we have the pubweb from the other variation... 
                                label = aPubWeb.Label;
                                break;
                            }
                        }
                        if (label == null)
                        {
                            throw new System.ArgumentException(HttpContext.GetGlobalResourceObject("CLF3", "Error_CantFindPubWebLabel", SPContext.Current.Web.Locale).ToString(), "listItem");
                        }
                        else
                        {
                            targetPage = publishingPage.GetVariation(label);

                            string currenturl = HttpContext.Current.Request.Path.ToString().ToLower();

                            string reverseurl = targetPage.Uri.AbsoluteUri;
                            string langlabel = HttpContext.GetGlobalResourceObject("CLF3", "OtherLanguageText", SPContext.Current.Web.Locale).ToString();
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
                            string Languagecontrol = (publishingPage.PublishingWeb.Label.Language.Substring(0, 2) == "en") ?
                                "<a href=\"" + reverseurl + queryString + "\" lang=\"" + propLang + "\" xml:lang=\"" + propLang + "\" onclick=\"javascript:OnSelectionChange2(1036); return false;\"><span>" + langlabel + "</span></a>" :
                                "<a href=\"" + reverseurl + queryString + "\" lang=\"" + propLang + "\" xml:lang=\"" + propLang + "\" onclick=\"javascript:OnSelectionChange2(1033); return false;\"><span>" + langlabel + "</span></a>";
                            this.Controls.Add(new LiteralControl(Languagecontrol));
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
                        Master_Pages.CLF3PublishingMaster masterPage = (Master_Pages.CLF3PublishingMaster)this.Page.Master;
                        if (!String.IsNullOrEmpty(masterPage.LanguageFlipQueryString))
                            queryString = masterPage.LanguageFlipQueryString;
                    }

                    string url = SPContext.Current.Web.Url;
                    // no variations... use the current users local
                    string langlabel = HttpContext.GetGlobalResourceObject("CLF3", "OtherLanguageText", System.Threading.Thread.CurrentThread.CurrentUICulture).ToString();
                    string propLang = System.Threading.Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;
                    string Languagecontrol = (System.Threading.Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName == "en") ?
                            "<a href=\"" + url + queryString + "\" lang=\"" + propLang + "\" xml:lang=\"" + propLang + "\" onclick=\"javascript:OnSelectionChange(1036); return false;\"><span>" + langlabel + "</span></a>" :
                            "<a href=\"" + url + queryString + "\" lang=\"" + propLang + "\" xml:lang=\"" + propLang + "\" onclick=\"javascript:OnSelectionChange(1033); return false;\"><span>" + langlabel + "</span></a>";
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
