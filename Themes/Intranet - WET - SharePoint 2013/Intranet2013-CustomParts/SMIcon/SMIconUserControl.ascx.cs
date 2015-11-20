using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


using System.ComponentModel;
using System.Web;
using Microsoft.SharePoint;
using System.Text;
using System.Linq;
using Microsoft.SharePoint.WebControls;
using System.Collections.Generic;

namespace Intranet2013_CustomParts.SMIcon
{
    public partial class SMIconUserControl : UserControl
    {

        private string smIConLinksHeaderEn = "Stay Connected";
        private string smIConLinksHeaderFr = "Restez branchés";

        [WebBrowsable(true)]
        [WebDisplayName("SMIConLinksHeaderEn")]
        [WebDescription("English Header Text")]
        [Personalizable(PersonalizationScope.User)]
        [Category("SMIconLink Setting")]

        public string SMIConLinksHeaderEn
        {
            get { return smIConLinksHeaderEn; }
            set { smIConLinksHeaderEn = value; }
        }


        [WebBrowsable(true)]
        [WebDisplayName("SMIConLinksHeaderFr")]
        [WebDescription("French Header Text")]
        [Personalizable(PersonalizationScope.User)]
        [Category("SMIconLink Setting")]
        public string SMIConLinksHeaderFr
        {
            get { return smIConLinksHeaderFr; }
            set { smIConLinksHeaderFr = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            int LCID = HttpContext.Current.Request.Url.Segments.Contains("fra/") ? 1036 : 1033;
            System.Text.StringBuilder sb = new StringBuilder();
            //Load the images and links from the List
            string smiconList = "SMIcon";
            //Shireeh Added run with elevated to avoid annonymous access issue
            SPSecurity.RunWithElevatedPrivileges(delegate()
                    {
                        using (SPSite site = new SPSite(SPContext.Current.Site.Url))
                        {
                            using (SPWeb web = site.RootWeb)
                            {
                                try
                                {
                                    SPList list = web.Lists.TryGetList(smiconList);
                                    SPQuery oQuery = new SPQuery();
                                    SPListItemCollection collListItems;
                                    oQuery.Query = "<Where><IsNotNull><FieldRef Name='ID'/></IsNotNull></Where>" +
                                                                "<OrderBy><FieldRef Name='ItemOrder' /></OrderBy>";
                                    collListItems = list.GetItems(oQuery);
                                    if (sb != null & list != null && list.Items.Count > 0)
                                    {
                                        sb.Append("<div class= \"SMIconTitleRow\"><h3 class=\"background-accent margin-bottom-medium\" style\"width:100% !important; vertical-align:middle;\">");
                                        sb.Append((LCID == 1033 ? this.SMIConLinksHeaderEn : this.SMIConLinksHeaderFr));
                                        sb.Append("</h3></div><div class=\"SMIconLinkRow\"><div class= \"float-left\">");
                                        sb.Append("&nbsp; &nbsp;");
                                        foreach (SPListItem item in collListItems)
                                        {
                                            string link = LCID == 1036 ? "FraLink" : "ENGLink";
                                            String img = LCID == 1036 ? "FraIcon" : "ENGIcon";
                                            sb.Append("<a id=\"" + item.Title + "\" href=\"" + item[link].ToString().Split(',')[0] + "\" style=\"" + "stylename" + "\">" + "<img ID=\"" + "img" + item.Title + "\" runat=\"server\" src=\"" + item[img].ToString().Split(',')[0] + "\" /></a> ");

                                        }
                                        sb.Append("</div></div>");
                                        sb.Append("<link rel=\"stylesheet\" type=\"text/css\" href=\"/Style Library/SideImageLink/SideImageLink.css\"/>");
                                    }
                                    Literal1.Text = sb.ToString();

                                }//end try
                                catch
                                {






                                }
                                finally
                                {
                                    site.Dispose();
                                    web.Dispose();
                                }

                            }//web
                        }//site
                    });
        }
    }
}
