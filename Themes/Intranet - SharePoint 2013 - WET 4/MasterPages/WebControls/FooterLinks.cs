using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;

namespace WET.Theme.Intranet.WebControls
{
    [ToolboxData("<{0}:FooterLinks runat=\"server\" />")]
    public class FooterLinks : WebControl
    {
        protected override void Render(HtmlTextWriter writer)
        {
            try
            {
                int LCID = HttpContext.Current.Request.Url.Segments.Contains("fra/") ? 1036 : 1033;// System.Threading.Thread.CurrentThread.CurrentUICulture.LCID;
                System.Text.StringBuilder sb = new StringBuilder();
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    SPListItemCollection collListItems;
                    SPList footerList = SPContext.Current.Site.RootWeb.Lists.TryGetList("LACFooterLinks");

                    SPQuery oQuery = new SPQuery();
                    if (LCID == 1036)
                        oQuery.Query = "<Where><IsNotNull><FieldRef Name='ID'/></IsNotNull></Where>" +
                                        "<OrderBy><FieldRef Name='EnglishOrder' /></OrderBy>";

                    else
                        oQuery.Query = "<Where><IsNotNull><FieldRef Name='ID'/></IsNotNull></Where>" +
                                         "<OrderBy><FieldRef Name='FrenchOrder' /></OrderBy>";



                    if (footerList != null)
                    {
                        collListItems = footerList.GetItems(oQuery);
                        if (collListItems != null)
                        {
                            int i = 1;
                            sb.Append("<ul class=\"float-right\">");
                            foreach (SPListItem item in collListItems)
                            {
                                sb.Append("<li>");
                                if (LCID == 1033)
                                    sb.Append("<a class=\"footerFontClass\" href=\"" + item["English Url"] + "\">" + item["English Link Text"] + "</a>");
                                else
                                    sb.Append("<a class=\"footerFontClass\" href=\"" + item["French Url"] + "\">" + item["French Link Text"] + "</a>");
                                if (i != collListItems.Count)
                                    sb.Append("&nbsp;&nbsp;");
                                sb.Append("</li>");
                            }
                            sb.Append("</ul>");
                        }
                        writer.Write(sb.ToString());
                    }
                });
            }
            catch (Exception ex)
            {
                writer.Write("oTech.Lac.Intranet.WebControls: FooterLinks exception message: " + ex.ToString());
            }
        }
    }
}
