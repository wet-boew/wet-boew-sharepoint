using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint.Publishing;
using System.Linq;
using WET.Theme.Intranet.Objects;

namespace WET.Theme.Intranet.WebControls
{
    [ToolboxData("<{0}:HeaderLink runat=\"server\" />")]
    public class HeaderLink : WebControl
    {
        protected override void Render(HtmlTextWriter writer)
        {
            try
            {
                System.Text.StringBuilder sb = new StringBuilder();
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    SPList headerList = SPContext.Current.Site.RootWeb.Lists.TryGetList("WETHeaderNavigation");
                    if (headerList != null)
                    {
                        int count = 1;
                        string fieldName = "Eng";
                        if (SPContext.Current.Web.Url.ToLower().Contains("/fra/"))
                            fieldName = "Fra";

                        foreach (SPListItem item in headerList.Items)
                        {
                            sb.AppendLine("<li id=\"gcwu-gcnb" + count.ToString() + "\"><a href=\"" + item["Url" + fieldName].ToString().Split(',')[0] + "\">" + item["Title" + fieldName].ToString() + "</a></li>");
                            count++;
                        }


                        writer.Write(sb.ToString());
                    }
                });
            }
            catch { }
        }
    }
}
