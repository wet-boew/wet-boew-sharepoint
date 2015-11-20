using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using System.Text;
using Microsoft.SharePoint.WebControls;
using System.Text.RegularExpressions;
using System.Web;
using System.Linq;

namespace WET.Theme.WebParts.QuickLinks
{
    [ToolboxItemAttribute(false)]
    public partial class QuickLinks : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]


        private string quickLinkHeaderEn = "QUICK LINKS";
        private string quickLinkHeaderFr = "QUICK LINKS Fr";

        [WebBrowsable(true)]
        [WebDisplayName("QuickLinksHeaderEn")]
        [WebDescription("English Header Text")]
        [Personalizable(PersonalizationScope.User)]
        [Category("QuickLink Setting")]
        public string QuickLinksHeaderEn
        {
            get { return quickLinkHeaderEn; }
            set { quickLinkHeaderEn = value; }
        }


        [WebBrowsable(true)]
        [WebDisplayName("QuickLinksHeaderFr")]
        [WebDescription("French Header Text")]
        [Personalizable(PersonalizationScope.User)]
        [Category("QuickLink Setting")]
        public string QuickLinksHeaderFr
        {
            get { return quickLinkHeaderFr; }
            set { quickLinkHeaderFr = value; }
        }

        public QuickLinks()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            using (SPSite site = new SPSite(SPContext.Current.Site.Url))
            {
                using (SPWeb web = site.RootWeb)
                {
                    try
                    {
                        StringBuilder sb = new StringBuilder();
                        int LCID = HttpContext.Current.Request.Url.Segments.Contains("fra/") ? 1036 : 1033;// System.Threading.Thread.CurrentThread.CurrentUICulture.LCID;


                        registerClientScirptCSS(sb);

                        SPList list = web.Lists.TryGetList("LACQuickLinks");
                        generataSliderHTML(sb, list, LCID);

                        Literal1.Text = sb.ToString();
                    }
                    catch (Exception ex)
                    {
                        Literal1.Text = ex.ToString();
                    }
                }
            }
        }

        private void generataSliderHTML(StringBuilder sb, SPList list, int lcid)
        {
            if (sb != null & list != null && list.Items.Count > 0)
            {
                sb.Append("<div class=\"span-2\">");
                sb.Append("<h3 class=\"background-accent margin-bottom-medium\">");
                if (lcid == 1036)
                    sb.Append(this.QuickLinksHeaderFr);
                else
                    sb.Append(this.QuickLinksHeaderEn);
                sb.Append("</h3>");
                sb.Append("<ul class=\"list-bullet-none\">");
                foreach (SPListItem item in list.Items)
                {
                    sb.Append("<li class=\"quickLinkLi\">");

                    if (lcid == 1033)
                    {
                        sb.Append("<a href=\"" + item["English Url"] + "\">");
                        sb.Append(item["English Link Text"] + "</a>");
                    }
                    else
                    {
                        sb.Append("<a href=\"" + item["French Url"] + "\">");
                        sb.Append(item["French Link Text"] + "</a>");
                    }
                    sb.Append("</li>");
                }
                sb.Append("</ul>");
                sb.Append("</div>");

            }

        }


        private void registerClientScirptCSS(StringBuilder sb)
        {
            if (sb != null)
            {

            }

        }
    }
}

