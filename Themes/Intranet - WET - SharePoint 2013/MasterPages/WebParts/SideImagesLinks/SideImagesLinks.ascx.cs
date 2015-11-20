using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Text;
using Microsoft.SharePoint;
using System.ComponentModel;
using System.Web;
using System.Linq;

namespace WET.Theme.WebParts.SideImagesLinks
{
    [ToolboxItemAttribute(false)]
    public partial class SideImagesLinks : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public SideImagesLinks()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //Shireeh Added run with elevated to avoid annonymous access issue
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.Url))
                {
                    using (SPWeb web = site.RootWeb)
                    {
                        try
                        {
                            //int LCID = (int)web.Language;
                            int LCID = HttpContext.Current.Request.Url.Segments.Contains("fra/") ? 1036 : 1033;// System.Threading.Thread.CurrentThread.CurrentUICulture.LCID;
                            StringBuilder sb = new StringBuilder();
                            SPList list = web.Lists.TryGetList("LACSideImagesLinks");
                            generataSliderHTML(sb, list, LCID);

                            Literal1.Text = sb.ToString();
                        }
                        catch (Exception ex)
                        {
                            Literal1.Text = ex.ToString();
                        }
                    }
                }
            });
        }

        private void generataSliderHTML(StringBuilder sb, SPList list, int lcid)
        {


            SPQuery oQuery = new SPQuery();
            SPListItemCollection collListItems;

            oQuery.Query = "<Where><IsNotNull><FieldRef Name='ID'/></IsNotNull></Where>" +
                                        "<OrderBy><FieldRef Name='ItemOrder' /></OrderBy>";
            collListItems = list.GetItems(oQuery);






            if (sb != null & list != null && list.Items.Count > 0)
            {
                sb.Append("<div class=\"span-2\"> ");
                int i = 1;
                sb.Append("<ul class=\"list-bullet-none\">");
                foreach (SPListItem item in collListItems)
                {
                    if (i <= 3)
                    {
                        if (i == 1 && lcid == 1033)
                            //--sb.Append("<li class=\"personLinkRow\"><div class= \"float-left\">" + item["English Link Text"] + "</br><a style=\"color: white !important;\" href=\"" + item["English Url"] + "\"> </div><div class=\"align-right\">" + "<img class=\"float-right\"  height: 45px;\" src=\"" + item.File.ServerRelativeUrl + "\" /> </a>");
                        //if (i == 2 && lcid == 1033)
                            sb.Append("<a style=\"color: white !important;\" href=\"" + item["English Url"] + "\"> <li class=\"webLinkRow\"><div class= \"float-left\">" + item["English Link Text"] + "</br><b>" + item["English Description"] + "</b></div><div class=\"align-right\">" + "<img class=\"float-right\"   src=\"" + item.File.ServerRelativeUrl + "\" />");
                        if (i == 2 && lcid == 1033)
                            sb.Append("<a style=\"color: white !important;\" href=\"" + item["English Url"] + "\"> <li class=\"phoneLinkRow\"><div class= \"float-left\">" + item["English Link Text"] + "</br><b>" + item["English Description"] + "</b></div><div class=\"align-right\">" + "<img class=\"float-right sideImageLinksSizes\" src=\"" + item.File.ServerRelativeUrl + "\" />");
                        if (i == 3 && lcid == 1033)
                            sb.Append("<a style=\"color: white !important;\" href=\"" + item["English Url"] + "\"> <li class=\"notepadLinkRow\"><div class= \"float-left\">" + item["English Link Text"] + "</br><b>" + item["English Description"] + "</b></div><div class=\"align-right\">" + "<img class=\"float-right\" src=\"" + item.File.ServerRelativeUrl + "\" />");

                        else
                        {
                            if (i == 1 && lcid == 1036)
                                //--sb.Append("<li class=\"personLinkRow\"><div class= \"float-left\">" + item["French Link Text"] + "</br><a style=\"color: white !important;\" href=\"" + item["French Url"] + "\"> </div><div class=\"align-right\">" + "<img class=\"float-right\"  height: 45px;\" src=\"" + item.File.ServerRelativeUrl + "\" /></a>");
                            //if (i == 2 && lcid == 1036)
                                sb.Append("<a style=\"color: white !important;\" href=\"" + item["French Url"] + "\"> <li class=\"webLinkRow\"><div class= \"float-left\">" + item["French Link Text"] + "</br><b>" + item["French Description"] + "</b></div><div class=\"align-right\">" + "<img class=\"float-right\"   src=\"" + item.File.ServerRelativeUrl + "\" />");
                            if (i == 2 && lcid == 1036)
                                sb.Append("<a style=\"color: white !important;\" href=\"" + item["French Url"] + "\"> <li class=\"phoneLinkRow\"><div class= \"float-left\">" + item["French Link Text"] + "</br><b>" + item["French Description"] + "</b></div><div class=\"align-right\">" + "<img class=\"float-right sideImageLinksSizes\" src=\"" + item.File.ServerRelativeUrl + "\" /><");
                            if (i == 3 && lcid == 1036)
                                sb.Append("<a style=\"color: white !important;\" href=\"" + item["French Url"] + "\"> <li class=\"notepadLinkRow\"><div class= \"float-left\">" + item["French Link Text"] + "</br><b>" + item["French Description"] + "</b></div><div class=\"align-right\">" + "<img class=\"float-right\" src=\"" + item.File.ServerRelativeUrl + "\" />");

                        }

                        sb.Append("</li> </a> <div class=\"clear\" class=\"sideImageLinksBottomClear;\"></div>");
                        i++;
                    }
                    else
                        break;
                }
                sb.Append("</ul>");
                sb.Append("</div>");


            }
        }


        private void registerClientScirptCSS(StringBuilder sb)
        {
            if (sb != null)
            {
                sb.Append("<link rel=\"stylesheet\" type=\"text/css\" href=\"/Style Library/SideImageLink/SideImageLink.css\"/>");

            }

        }
    }
}

