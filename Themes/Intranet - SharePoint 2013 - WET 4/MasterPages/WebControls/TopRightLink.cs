using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using System.Web;

namespace WET.Theme.Intranet.WebControls
{
    [ToolboxData("<{0}:TopRightLink runat=\"server\" />")]
    public class TopRightLink : WebControl
    {
        /// <summary> 
        /// Render this control to the output parameter specified.
        /// </summary>
        /// <param name="output"> The HTML writer to write out to </param>
        protected override void Render(HtmlTextWriter output)
        {
            using (SPSite site = new SPSite(SPContext.Current.Site.Url))
            {
                using (SPWeb web = site.RootWeb)
                {
                    try
                    {
                        SPListItemCollection collListItems;

                        int LCID = HttpContext.Current.Request.Url.Segments.Contains("fra/") ? 1036 : 1033;// System.Threading.Thread.CurrentThread.CurrentUICulture.LCID;
                        StringBuilder sb = new StringBuilder();
                        SPList list = web.Lists["LACRightHeaderLink"];

                        SPQuery oQuery = new SPQuery();
                        if (LCID == 1036)
                            oQuery.Query = "<Where><IsNotNull><FieldRef Name='ID'/></IsNotNull></Where>" +
                                            "<OrderBy><FieldRef Name='EnglishOrder' /></OrderBy>";

                        else
                            oQuery.Query = "<Where><IsNotNull><FieldRef Name='ID'/></IsNotNull></Where>" +
                                             "<OrderBy><FieldRef Name='FrenchOrder' /></OrderBy>";

                        collListItems = list.GetItems(oQuery);

                        if (list.Items.Count > 0)
                        {
                            int i = 1;
                            sb.Append("<div class=\"toprightlinks\">");
                            foreach (SPListItem item in collListItems)
                            {
                                if (LCID == 1036)
                                    sb.Append("&nbsp;&nbsp;<a href=\"" + item["French Url"] + "\">" + item["French Link Text"] + "</a>");
                                else
                                    sb.Append("&nbsp;&nbsp;<a href=\"" + item["English Url"] + "\">" + item["English Link Text"] + "</a>");
                                if (i != list.Items.Count)
                                {
                                    sb.Append("&nbsp;&nbsp|");
                                }
                                i++;
                            }
                            sb.Append("</div>");
                        }

                        output.Write(sb);
                    }
                    catch (Exception ex)
                    {
                        output.Write("oTech.Lac.Intranet.WebControls: HomeLink exception message: " + ex.ToString());
                    }
                }
            }

        }
    }
}
