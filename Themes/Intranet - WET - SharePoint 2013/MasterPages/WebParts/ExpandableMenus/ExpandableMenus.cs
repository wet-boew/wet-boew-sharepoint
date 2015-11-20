using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using System.Text;
using System.Linq;
using Microsoft.SharePoint.WebControls;
using System.Collections.Generic;

namespace WET.Theme.WebParts.ExpandableMenus
{
    [ToolboxItemAttribute(false)]
    public class ExpandableMenus : WebPart
    {
        [WebBrowsable(true), Personalizable(PersonalizationScope.Shared), WebDisplayName("List's Name")]
        public string ListName { get; set; }

        protected override void Render(HtmlTextWriter writer)
        {

            int licdtmp = System.Threading.Thread.CurrentThread.CurrentUICulture.LCID;
        //Shireeh Added run with elevated to avoid annonymous access issue
            SPSecurity.RunWithElevatedPrivileges(delegate()
                    {
                        using (SPSite site = new SPSite(SPContext.Current.Site.Url))
                        {
                            using (SPWeb web = site.RootWeb)
                            {
                                try
                                {
                                    int LCID = HttpContext.Current.Request.Url.Segments.Contains("fra/") ? 1036 : 1033;// System.Threading.Thread.CurrentThread.CurrentUICulture.LCID;
                                    StringBuilder sb = new StringBuilder();
                                    SPList list = web.Lists.TryGetList(this.ListName);

                                    writer.Write(generateHTML(list, LCID));
                                    if (this.ListName.ToString().Length <= 0)
                                    {
                                        this.ListName = "LACexpandableMenu";
                                    }
                                }
                                catch (Exception ex)
                                {
                                    writer.Write(ex.Message + " -- " + ex.StackTrace);
                                }
                            }
                        }
                    });//elevated
        }

        private string generateHTML(SPList list, int lcid)
        {
            System.Text.StringBuilder sb = new StringBuilder();
            try
            {
                SPQuery spQuery = new SPQuery();
                List<SPListItem> collListItems = new List<SPListItem>();
                System.Collections.Generic.Dictionary<string, string> dicGroups = new Dictionary<string, string>();

                foreach (SPListItem item in list.Items)
                {
                    try
                    {
                        dicGroups.Add(item["English Group"].ToString(), item["French Group"].ToString());
                    }
                    catch
                    { }
                }

                string query = "";

                string urlLink, urlText;
                int i = 0;
                SPListItemCollection itemsCol;
                foreach (string entry in dicGroups.Keys)
                {
                    string bgColor = "";
                    switch (i % 5)
                    {
                        case (0):
                            bgColor = "#598DB2";
                            break;
                        case (1):
                            bgColor = "#1B7768";
                            break;
                        case (2):
                            bgColor = "#004A7F";
                            break;
                        case (3):
                            bgColor = "#0069B5";
                            break;
                        case (4):
                        default:
                            bgColor = "#598DB2";
                            break;
                    }
                    string value = entry;
                    string styleValue = "";
                    string displayValue = "display:none;";
                    if (i == 0)
                    {
                        styleValue = " open='open'";
                        displayValue = "display:inline-block;";
                    }
                    if (lcid == 1036)
                        value = dicGroups[entry];
                    sb.Append("<div style=\"padding-left:10px;\"><details class=\"wet-boew-prettify all-pre linenums polyfill print-open\" id=\"details_" + i.ToString() + "\" data-load=\"prettify\"" + styleValue + " onclick=\"ToggleExpandable('" + i.ToString() + "');\" style=\"padding-bottom:10px;\">" +
                            "<summary tabindex=\"0\" role=\"button\" aria-expanded=\"false\" style=\"-ms-user-select: none;padding-top:5px;height:25px;color:white;background-color:" + bgColor + ";\">" + value + "</summary>" +
                            "<span class=\"grid-12 polyfill print-open\" style=\"overflow:auto;background-color:#FFFFFF; padding-bottom: 0px;" + displayValue + "\" id=\"content" + i.ToString() + "\">");

                    spQuery = new SPQuery();
                    query = "<Where><Eq><FieldRef Name=\"English_x0020_Group\"/><Value Type=\"Text\">" + entry + "</Value></Eq></Where>";
                    spQuery.Query = query;

                    itemsCol = list.GetItems(spQuery);
                    int j = 0;
                    sb.Append("<div class=\"span-12 equalize\" style=\"margin-left:0px; margin-right:0px;background-color:#FFFFFF; padding: 0 0 0 0; margin: 0 0 0 0; line-height: 1em; \"><div class=\"span-6 row-start child \" style=\" margin: 0 0 0 0;padding: 0 0 0 0;  line-height: 1em; \">");
                   // sb.Append("<div class=\"span-12 equalize\" style=\"margin: 0 0 0 0;padding: 0 0 0 0; background-color:#FFFFFF;\"><div class=\"span-6 row-start child\" style=\" margin: 0 0 0 0;padding: 0 0 0 0;\">");
                    foreach (SPListItem item in itemsCol)
                    {
                        urlLink = item["English URL"].ToString().Split(',')[0];
                        urlText = item["Title"].ToString();
                        if (lcid == 1036)
                        {
                            urlLink = item["French URL"].ToString().Split(',')[0];
                            urlText = item["Title-FR"].ToString();
                        }

                        string breakContent = "";
                        //if (j > 0)
                        if (j != 0)
                        {
                           /* if (j % 2 == 0) breakContent = "</div></div><div class=\"span-12 equalize\" style=\"margin-left:0px; margin-right:0px;background-color:#FFFFFF;focus:block;\"><div class=\"span-6 row-start child\" style= \" margin: 0 0 0 0;padding: 0 0 0 0; \">";
                            //if (j % 2 == 0) breakContent = "</div></div><div class=\"span-12 equalize\" style=\"margin: 0 0 0 0;padding: 0 0 0 0; background-color:#FFFFFF;focus:block;\"><div class=\"span-6 row-start child\" style= \" margin: 0 0 0 0;padding: 0 0 0 0; \">";
                           else breakContent = "</div><div class=\"span-6 row-end child\" style= \" margin: 0 0 0 0;padding: 0 0 0 0; \">";
                           // else breakContent = "</div><div class=\"span-6 row-end child\" style=\"focus:block; cursor:pointer; \" onclick=\"location.href='" + urlLink + "';\">";
                            */
                            if (j % 2 == 0) breakContent = "</div></div><div class=\"span-12 equalize\" style=\"margin-left:0px; margin-right:0px;background-color:#FFFFFF;focus:block; padding: 0 0 0 0; margin: 0 0 0 0; line-height: 1em; \"><div class=\"span-6 row-start child\" style= \" padding-bottom: 0px; padding-top:0px; margin: 0 0 0 0; line-height: 1em; \">";
                            else breakContent = "</div><div class=\"span-6 row-end child\" style= \" padding-bottom: 0px; padding-top:0px; margin: 0 0 0 0; line-height: 1em; \" >";
                        }
                        string textDescription = "";
                        string moreLink = "";
                        if (lcid == 1033)
                        {
                            if (item["EnglishMoreText"] != null)
                                moreLink = "<a href=\"" + urlLink + "\">" + item["EnglishMoreText"] + "</a>";
                            textDescription = item["Description-EN"].ToString();
                        }
                        else
                        {
                            if (item["FrenchMoreText"] != null)
                                moreLink = "<a href=\"" + urlLink + "\">" + item["FrenchMoreText"] + "</a>";
                            textDescription = item["Description-FR"].ToString();
                        }

                        string ImageURL = lcid == 1033 ? item["ImageLink-EN"].ToString().Split(',')[0] : item["ImageLink-FR"].ToString().Split(',')[0];
                       //This is good works, But too much white space- Modifyinf the div within the if blocks.  
                        sb.Append(breakContent + "<div class=\"span-2 indent-none margin-top-medium margin-bottom-medium\" style=\"focus:block; cursor:pointer;  color:black; padding-right: 10px; padding-bottom:0px; padding-top:0px; margin: 0 0 0 0; line-height: 1em; \" onclick=\"location.href='" + urlLink + "';\"> <img src=\"" + ImageURL + "\" style=\"width:100% !important; padding: 0 0 0 0; margin: 0 0 0 0; line-height: 1em;\"> </div> <div style=\"focus:block; padding-top: 10px; padding-bottom: 0px; margin: 0 0 0 0; cursor:pointer; color:black; line-height: 1em; \" onclick=\"location.href='" + urlLink + "';\"> <b>" + urlText.Trim() + "</b><br />" + textDescription + moreLink + "</div>");
                        j++;
                    }
                    if (j % 2 != 0) sb.Append("</div><div class=\"span-6 row-end child\" style= \" padding-bottom: 0px; margin: 0 0 0 0; line-height: 1em; \"  >&nbsp;"); //sb.Append("</div><div class=\"span-6 row-end child\" style= \" margin: 0 0 0 0;padding: 0 0 0 0; \" >&nbsp;");
                    sb.Append("</div></span></details></div>");
                    i++;
                }

                sb.Append("<script type='text/javascript'>" +
                    "function ToggleExpandable(id){" +
                        "for(var i = 0; i < " + dicGroups.Keys.Count.ToString() + ";i++){" +
                            "var elementToToggle = document.getElementById('content' + i);" +
                            "elementToToggle.style.backgroundColor = '#9A0E11';" +
                            "var detailsElem = document.getElementById('details_' + i);" +
                            "if(id == i) {" +
                                "elementToToggle.style.display = \"block\";" +
                                "detailsElem.className = detailsElem.className + ' open';" +
                                "" +
                            "}" +
                            "else {" +
                                "elementToToggle.style.display = \"none\";" +
                                "detailsElem.className = detailsElem.className.replace(' open', '');" +
                             "}" +
                        "}" +
                    "}" +
                    "</script>");
                return sb.ToString();
            }
            catch (Exception ex)
            {
                sb.Append(ex.Message + " -- " + ex.StackTrace);
                return sb.ToString();
            }
        }
    }
}
