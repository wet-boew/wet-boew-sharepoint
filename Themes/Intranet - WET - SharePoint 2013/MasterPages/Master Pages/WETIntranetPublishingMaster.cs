using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using Microsoft.SharePoint;

namespace WET.Theme.Intranet.Master_Pages
{
    public class WETIntranetPublishingMaster : MasterPage
    {

        /// <summary>
        /// Override the query string in the URL of the language flip hyperlink.
        /// </summary>
        public string LanguageFlipQueryString = null;

        /// <summary>
        /// Override the title of the current page.
        /// </summary>
        public string PageTitle = null;

        /// <summary>
        /// Override the title of the page node in the breadcrumb.
        /// </summary>
        public string BreadcrumbPageNodeTitle = null;



        private List<string> Parameters = new List<string>();

        public WETIntranetPublishingMaster()
        {

        }

        private static void AddControlAdapterToType<T>(Type controlType) where T : Adapters.WebPartZoneAdapter, new()
        {
            try
            {
                var adapters = System.Web.HttpContext.Current.Request.Browser.Adapters;
                var key = controlType.AssemblyQualifiedName;
                if(!adapters.Contains(key))
                {
                    var adapter = typeof(T).AssemblyQualifiedName;
                    if(key !=null)
                        adapters.Add(key, adapter);
                }
            }
            catch { }
        }

        /// <summary>
        /// Register a new WET feature with the progressive enhancement initialization.
        /// </summary>
        /// <param name="parameter">The JavaScript object definition to add to the parameters array.</param>
        public void RegisterFeature(string parameter)
        {
            if (!this.Parameters.Contains(parameter))
                this.Parameters.Add(parameter);
        }

        public string getEditModeCSS()
        {
            string returnString = string.Empty;
            if (Microsoft.SharePoint.SPContext.Current.Web.CurrentUser != null)
            {
                /* for the edit screens  only*/
                returnString = "h2,h3,h4,h5,h6,p,blockquote,table,form,img,pre,details{margin-left:0px !important;margin-right:0px !important;}\n"
                                + @".ms-cui-ribbon
                                    {   
                                        line-height:1em !important;
                                    }
                                    .ms-toolpanefooter input
                                    {
                                        display:inline !important;
                                    }
                                    .UserControlGroup input
                                    {
                                        display:inline !important;
                                    }
                                    .ms-wpadder-items
                                    {
                                        padding-top:0px !important;
                                        padding-bottom:0px !important;
                                        padding-left:0px !important;
                                        padding-right:0px !important;
                                        line-height:auto !important;
                                    }
                                    body
                                    {
                                        height:98% !important;
                                        overflow:hidden !important;
                                    }
                                    .ms-alignleft
                                    {
                                        border:0px !important;
                                    }
                                    .ms-alignright
                                    {
                                        border:0px !important;
                                    }
                                    .ms-wpadder-itemTable td
                                    {
                                        border:0px !important;
                                    }
                                    .ms-wpadder-selected
                                    {
                                        padding-top:0px !important;
                                        padding-bottom:0px !important;
                                    }
                                    .ms-wpadder-selected img
                                    {
                                        margin:0px !important;
                                    }
                                    .ms-wpadder-items img
                                    {
                                        margin-bottom:0px !important;
                                    }
                                    .ms-wpadder-upload td
                                    {
                                        border:0px !important;
                                    }
                                    /* Nik - Dialog Buttons */
                                    .ms-dlgFrameContainer input
                                    {
                                        display:inline !important;
                                    }
                                    .ms-dlgFrameContainer
                                    {
                                        height:100% !important;
                                    }
                                    ";
            }
            else
            {
                returnString = @"/*Needed to fix the scroll bar*/
                                body #s4-workspace {
                                    overflow: visible !important;
                                }";
            }
            return returnString;
        }
       
        
        /// <summary>
        /// Render cleaned up code.
        /// </summary>
        /// <param name="writer"></param>
        /*protected override void Render(HtmlTextWriter writer)
        {
            try
            {
                //base.Render(writer);
                // extract all html
                if (SPContext.Current.Web.CurrentUser != null)
                {
                    base.Render(writer);
                }
                else
                {
                    System.IO.StringWriter str = new System.IO.StringWriter();
                    HtmlTextWriter wrt = new HtmlTextWriter(str);

                    // render html 
                    base.Render(wrt);
                    wrt.Close();
                    string html = str.ToString();

                    // find all script tags
                    Regex scriptRegex = new Regex("<script[^>]*");
                    MatchCollection scriptMatches = scriptRegex.Matches(html);

                    // go through matches in reverse 
                    for (int i = scriptMatches.Count - 1; i >= 0; i--)
                    {
                        // identify script tags with no type attribute  
                        if (scriptMatches[i].ToString().IndexOf("type") < 0)
                        {
                            // add type attribute after script opening tag 
                            html = html.Insert(scriptMatches[i].Index + 7, " type=\"text/javascript\"");
                        }
                    }

                    // find all uppercase script tags
                    Regex scriptRegex1 = new Regex("<script[^>]*");
                    MatchCollection scriptMatches1 = scriptRegex1.Matches(html);

                    // go through matches in reverse 
                    for (int i = scriptMatches1.Count - 1; i >= 0; i--)
                    {
                        html = html.Replace("<SCRIPT LANGUAGE='JavaScript' >", "<script type=\"text/javascript\">");
                    }

                    // find all language=javascript tags
                    Regex scriptRegex2 = new Regex("<script[^>]*");
                    MatchCollection scriptMatches2 = scriptRegex2.Matches(html);

                    // go through matches in reverse 
                    for (int i = scriptMatches2.Count - 1; i >= 0; i--)
                    {
                        html = html.Replace(" language=\"javascript\"", "");
                    }

                    // find all language=JavaScript tags
                    Regex scriptRegex3 = new Regex("<script[^>]*");
                    MatchCollection scriptMatches3 = scriptRegex3.Matches(html);

                    // go through matches in reverse 
                    for (int i = scriptMatches3.Count - 1; i >= 0; i--)
                    {
                        html = html.Replace(" language=\"JavaScript\"", "");
                    }

                    // find all defer tags
                    // DEFER version 1
                    Regex scriptRegex4 = new Regex("<script[^>]*");
                    MatchCollection scriptMatches4 = scriptRegex4.Matches(html);

                    // go through matches in reverse 
                    for (int i = scriptMatches4.Count - 1; i >= 0; i--)
                    {
                        html = html.Replace(" defer", "");
                        html = html.Replace("=\"defer\"", "");
                    }

                    // find form name tag
                    Regex scriptRegex5 = new Regex("<form[^>]*");
                    MatchCollection scriptMatches5 = scriptRegex5.Matches(html);

                    // go through matches in reverse 
                    for (int i = scriptMatches5.Count - 1; i >= 0; i--)
                    {
                        html = html.Replace(" name=\"aspnetForm\"", "");
                    }


                    // find all uppercase script tags
                    Regex scriptRegex7 = new Regex("</SCRIPT[^>]*");
                    MatchCollection scriptMatches7 = scriptRegex7.Matches(html);

                    // go through matches in reverse 
                    for (int i = scriptMatches7.Count - 1; i >= 0; i--)
                    {
                        html = html.Replace("SCRIPT", "script");
                    }

                    // find __expr-val-dir tags
                    Regex scriptRegex8 = new Regex("<html[^>]*");
                    MatchCollection scriptMatches8 = scriptRegex8.Matches(html);

                    // go through matches in reverse 
                    for (int i = scriptMatches8.Count - 1; i >= 0; i--)
                    {
                        html = html.Replace(" __expr-val-dir=\"ltr\"", "");
                        html = html.Replace("lang=\"en\"", "xml:lang=\"en\"");
                    }

                    // find tables tags
                    Regex scriptRegex9 = new Regex("<table[^>]*");
                    MatchCollection scriptMatches9 = scriptRegex9.Matches(html);

                    // go through matches in reverse 
                    for (int i = scriptMatches9.Count - 1; i >= 0; i--)
                    {
                        html = html.Replace(" TOPLEVEL", "");
                        html = html.Replace("vAlign", "valign");
                        html = html.Replace("width=\"100%\"", "");
                    }

                    // find tables tags
                    Regex scriptRegex10 = new Regex("<td[^>]*");
                    MatchCollection scriptMatches10 = scriptRegex10.Matches(html);

                    // go through matches in reverse 
                    for (int i = scriptMatches10.Count - 1; i >= 0; i--)
                    {
                        html = html.Replace("Modified By", "ModifiedBy");
                    }

                    // find hidden div
                    Regex scriptRegex11 = new Regex("<menu[^>]*");
                    MatchCollection scriptMatches11 = scriptRegex11.Matches(html);

                    // go through matches in reverse 
                    for (int i = scriptMatches11.Count - 1; i >= 0; i--)
                    {
                        html = html.Replace("<ie:menuitem", "<li");
                        html = html.Replace("</ie:menuitem>", "</li>");
                        html = html.Replace("onmenuclick", "onclick");
                        //html = html.Replace("text","Title");
                        html = html.Replace("iconsrc=", "style='background:url(");
                        html = html.Replace("style=\"display:none\"", "");
                    }

                    Regex scriptRegex12 = new Regex("<menu[^>]*");
                    MatchCollection scriptMatches12 = scriptRegex12.Matches(html);

                    // go through matches in reverse 
                    for (int i = scriptMatches12.Count - 1; i >= 0; i--)
                    {
                        html = html.Replace("<menu", "<ul");
                        html = html.Replace("</menu>", "</ul>");
                        html = html.Replace("HelpIcon.gif\"", "HelpIcon.gif\");display:none;'");
                        html = html.Replace("text=\"Help\"", "");
                        html = html.Replace("type=\"option\"", "");
                    }

                    Regex scriptRegex13 = new Regex("<input type='hidden' id='_wpcmWpid' name='_wpcmWpid' value=''[^>]*");
                    MatchCollection scriptMatches13 = scriptRegex13.Matches(html);

                    // go through matches in reverse 
                    for (int i = scriptMatches13.Count - 1; i >= 0; i--)
                    {
                        html = html.Replace("<input type='hidden' id='_wpcmWpid' name='_wpcmWpid' value='' />", "<p><input type='hidden' id='_wpcmWpid' name='_wpcmWpid' value='' /></p>");
                    }

                    Regex scriptRegex14 = new Regex("<input type='hidden' id='wpcmVal' name='wpcmVal' value=''[^>]*");
                    MatchCollection scriptMatches14 = scriptRegex14.Matches(html);

                    // go through matches in reverse 
                    for (int i = scriptMatches14.Count - 1; i >= 0; i--)
                    {
                        html = html.Replace("<input type='hidden' id='wpcmVal' name='wpcmVal' value=''/>", "<p><input type='hidden' id='wpcmVal' name='wpcmVal' value='' /></p>");
                    }

                    Regex scriptRegex15 = new Regex("<div[^>]*");
                    MatchCollection scriptMatches15 = scriptRegex15.Matches(html);

                    // go through matches in reverse 
                    for (int i = scriptMatches15.Count - 1; i >= 0; i--)
                    {
                        // the following line has been commented out because it prevents postbacks when anonymous browsing
                        //html = html.Replace("ID", "id");
                        html = html.Replace("aria-labelledby=\"ctl00_PlaceHolderMain_ctl00_label\"", "");
                        html = html.Replace("aria-labelledby=\"ctl00_PlaceHolderMainRight_ContentRight_label\"", "");
                        html = html.Replace("aria-labelledby=\"ctl00_PlaceHolderMain_label\"", "");
                    }

                    Regex scriptRegex16 = new Regex("<div[^>]*");
                    MatchCollection scriptMatches16 = scriptRegex16.Matches(html);

                    // go through matches in reverse 
                    for (int i = scriptMatches16.Count - 1; i >= 0; i--)
                    {
                        html = html.Replace("aria-labelledby=\"ctl00_PlaceHolderMain_ctl00_label\"", "");
                    }

                    // find and remove the alt tag from input search box
                    // for all matches... replace with class=\"ms-sbplain\"
                    Regex scriptRegex17 = new Regex("class=\"ms-sbplain\" alt=\"[^\"]*\"");
                    MatchCollection scriptMatches17 = scriptRegex17.Matches(html);
                    // go through matches in reverse 
                    for (int i = scriptMatches17.Count - 1; i >= 0; i--)
                    {
                        html = html.Replace(scriptMatches17[i].Value, "class=\"ms-sbplain\"");
                    }

                    // fix the onmouseover onmouseout to include onblur and onfocus for the small search box
                    // Richard Dufour - 4/24/2012
                    //  Not Required... corected the link for the search buttons... this will put the magnifined glass back in
                    string scriptRegex18String = "onmouseover=\"[^\"]*\" onmouseout=\"[^\"]*\" class=\"srch-gosearchimg\"";
                    Regex scriptRegex18 = new Regex(scriptRegex18String);
                    MatchCollection scriptMatches18 = scriptRegex18.Matches(html);
                    // go through matches in reverse 
                    for (int i = scriptMatches18.Count - 1; i >= 0; i--)
                    {
                        string scriptRegex18ReplaceString = scriptMatches18[i].Value + " " + scriptMatches18[i].Value.Replace("class=\"srch-gosearchimg\"", "").Replace("onmouseover", "onfocus").Replace("onmouseout", "onblur");

                        html = html.Replace(scriptMatches18[i].Value, scriptRegex18ReplaceString);
                    }

                    // convert ccedil to &#231;
                    Regex scriptRegex19 = new Regex("&ccedil;");
                    MatchCollection scriptMatches19 = scriptRegex19.Matches(html);
                    // go through matches in reverse 
                    for (int i = scriptMatches19.Count - 1; i >= 0; i--)
                    {
                        html = html.Replace(scriptMatches19[i].Value, "&#231;");
                    }

                    // strip out the class="s4-wpTopTable" border="0" cellpadding="0" cellspacing="0" from the searchbox
                    Regex scriptRegex20 = new Regex("class=\"s4-wpTopTable\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\"");
                    MatchCollection scriptMatches20 = scriptRegex20.Matches(html);
                    // go through matches in reverse 
                    for (int i = scriptMatches20.Count - 1; i >= 0; i--)
                    {
                        html = html.Replace(scriptMatches20[i].Value, "class=\"s4-wpTopTable\"");
                    }

                    // strip out the s4-search" border="0" cellpadding="0" cellspacing="0" from the searchbox
                    Regex scriptRegex21 = new Regex("s4-search\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\"");
                    MatchCollection scriptMatches21 = scriptRegex21.Matches(html);
                    // go through matches in reverse 
                    for (int i = scriptMatches21.Count - 1; i >= 0; i--)
                    {
                        html = html.Replace(scriptMatches21[i].Value, "s4-search\"");
                    }

                    // convert <td valign="top"> to <td style="vertical-align: top !important;">
                    Regex scriptRegex22 = new Regex("<td valign=\"top\">");
                    MatchCollection scriptMatches22 = scriptRegex22.Matches(html);
                    // go through matches in reverse 
                    for (int i = scriptMatches22.Count - 1; i >= 0; i--)
                    {
                        html = html.Replace(scriptMatches22[i].Value, "<td style=\"vertical-align: top !important;\">");
                    }

                    // strip out the other format of s4-search" cellpadding="0" cellspacing="0" border="0" from the searchbox
                    Regex scriptRegex23 = new Regex("s4-search\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\"");
                    MatchCollection scriptMatches23 = scriptRegex23.Matches(html);
                    // go through matches in reverse 
                    for (int i = scriptMatches23.Count - 1; i >= 0; i--)
                    {
                        html = html.Replace(scriptMatches23[i].Value, "s4-search\"");
                    }

                    // clean the span outputted by SP when adding breadcrumbs <div id="cn-bc-inner"><span><ol> and </span></div><!-- cn-bc-inner -->
                    Regex scriptRegex24 = new Regex("<div id=\"gcwu-bc-in\"><span><ol>");
                    MatchCollection scriptMatches24 = scriptRegex24.Matches(html);
                    // go through matches in reverse 
                    for (int i = scriptMatches24.Count - 1; i >= 0; i--)
                    {
                        html = html.Replace(scriptMatches24[i].Value, "<div id=\"gcwu-bc-in\"><ol>");
                    }

                    // clean the span outputted by SP when adding breadcrumbs <div id="cn-bc-inner"><span><ol> and </span></div><!-- cn-bc-inner -->
                    Regex scriptRegex25 = new Regex("</span></div>");
                    MatchCollection scriptMatches25 = scriptRegex25.Matches(html);
                    // go through matches in reverse 
                    for (int i = scriptMatches25.Count - 1; i >= 0; i--)
                    {
                        html = html.Replace(scriptMatches25[i].Value, "</div>");
                    }

                    // small search web part extras
                    // clean out WebPartid="00000000-0000-0000-0000-000000000000" HasPers="true" id="WebPartWPQ1"  class="noindex" OnlyForMePart="true" allowDelete="false"
                    Regex scriptRegex26 = new Regex("WebPartid=\"00000000-0000-0000-0000-000000000000\" HasPers=\"true\"", RegexOptions.IgnoreCase);
                    MatchCollection scriptMatches26 = scriptRegex26.Matches(html);
                    // go through matches in reverse 
                    for (int i = scriptMatches26.Count - 1; i >= 0; i--)
                    {
                        html = html.Replace(scriptMatches26[i].Value, "");
                    }
                    Regex scriptRegex27 = new Regex("OnlyForMePart=\"true\" allowDelete=\"false\"", RegexOptions.IgnoreCase);
                    MatchCollection scriptMatches27 = scriptRegex27.Matches(html);
                    // go through matches in reverse 
                    for (int i = scriptMatches27.Count - 1; i >= 0; i--)
                    {
                        html = html.Replace(scriptMatches27[i].Value, "");
                    }

                    Regex scriptRegex28 = new Regex("<div id=\"ECBItems[^>]*", RegexOptions.IgnoreCase);
                    MatchCollection scriptMatches28 = scriptRegex28.Matches(html);
                    // go through matches in reverse 
                    for (int i = scriptMatches28.Count - 1; i >= 0; i--)
                    {
                        html = html.Replace(scriptMatches28[i].Value, scriptMatches28[i].Value + " style=\"display:none;\"");
                    }

                    // uncomment this to have the tables removed
                    // </?table[^>]*>|</?tr[^>]*>|</?td[^>]*>|</?thead[^>]*>|</?th[^>]*>|</?tfoot[^>]*>|</?tbody[^>]*>
                    //Regex scriptRegex28 = new Regex("</?table[^>]*>|</?tr[^>]*>|</?td[^>]*>|</?thead[^>]*>|</?th[^>]*>|</?tfoot[^>]*>|</?tbody[^>]*>");
                    //MatchCollection scriptMatches28 = scriptRegex28.Matches(html);
                    //// go through matches in reverse 
                    //for (int i = scriptMatches28.Count - 1; i >= 0; i--)
                    //{
                    //    html = html.Replace(scriptMatches28[i].Value, "");
                    //}

                    // write the 'clean' html to the page
                    writer.Write(html);
                }
            }
            catch (Exception ex)
            {
                WET.Theme.Intranet.Objects.Logger.WriteLog("WETPublishingMasterPage.cs error in Render method: " + ex.Message);
            }
        }*/


        protected void Page_Load(object sender, EventArgs e)
        {

        }

    }
}
