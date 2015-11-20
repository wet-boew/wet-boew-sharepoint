using Microsoft.SharePoint;
using System;
using System.ComponentModel;
using System.Text;
using System.Web.UI.WebControls.WebParts;
using System.Web;
using System.Linq;

namespace WET.Theme.WebParts.SideSlidesShow
{
    [ToolboxItemAttribute(false)]
    public partial class SideSlidesShow : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]

        private string slideShowHeaderEn = "Newly Digitized Images";
        private string slideShowHeaderFr = "Images Nouvellement Numérisés";
        private string[] tabValue;

        [WebBrowsable(true)]
        [WebDisplayName("SlideShowHeaderEn")]
        [WebDescription("English Header Text")]
        [Personalizable(PersonalizationScope.User)]
        [Category("SlideShow Setting")]
        public string SlideShowHeaderEn
        {
            get { return slideShowHeaderEn; }
            set { slideShowHeaderEn = value; }
        }

        [WebBrowsable(true)]
        [WebDisplayName("SlideShowHeaderFr")]
        [WebDescription("French Header Text")]
        [Personalizable(PersonalizationScope.User)]
        [Category("SlideShow Setting")]
        public string SlideShowHeaderFr
        {
            get { return slideShowHeaderFr; }
            set { slideShowHeaderFr = value; }
        }





        public SideSlidesShow()
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
                                    StringBuilder sb = new StringBuilder();
                                    int LCID = HttpContext.Current.Request.Url.Segments.Contains("fra/") ? 1036 : 1033;// System.Threading.Thread.CurrentThread.CurrentUICulture.LCID;


                                    //registerClientScirptCSS(sb);

                                    SPList list = web.Lists.TryGetList("LACNewDigitalsImagesInstance");
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
            if (sb != null & list != null && list.Items.Count > 0)
            {
                sb.Append("<section><div class=\"span-2\">");
                sb.Append("<h3 class=\"background-accent margin-bottom-medium\">");
                if (lcid == 1036)
                    sb.Append(this.SlideShowHeaderFr);
                else
                    sb.Append(this.SlideShowHeaderEn);
                sb.Append("</h3><div class=\"span-2\">");
                sb.Append("<div class=\"wet-boew-tabbedinterface tabs-style-5 cycle  auto-play span-2 row-start wet-boew-responsiveimg\" data-picture=\"data-picture\">");
                sb.Append("<div class=\"tabs-panel\">");
                int i = 1;
                foreach (SPListItem item in list.Items)
                {

                    sb.Append("<div class=\"align-center module-poster\" id=\"tab" + i + "\">");

                    sb.Append("<a href=\"" + item["English Url"] + "\" rel=\"external\">");
                    sb.Append("<img style=\"width:270px !important;\" src=\"" + item.File.ServerRelativeUrl + "\" alt=\"");


                    if (lcid == 1036)
                    {
                        sb.Append(item["French Description"] + "\"></a><p>" + item["French Description"] + "<p>");
                    }
                    else
                    {
                        sb.Append(item["English Description"] + "\"></a><div class=\"image-caption\"><p>" + item["English Description"] + "<p></div>");
                    }
                   //imagedescription
                    sb.Append("</div>");//slide
                    i++;
                }
                sb.Append("</div>");
                sb.Append("<ul class=\"tabs\">");
                int j = 1;

                foreach (SPListItem item in list.Items)
                {


                    if (lcid == 1036)
                    {

                        sb.Append("<li><a href=\"#tab" + j.ToString() + "\">" + item["French Description"] + "</a></li>");
                    }
                    else
                    {
                        sb.Append("<li><a href=\"#tab" + j.ToString() + "\">" + item["English Description"] + "</a></li>");
                    }
                    j++;
                }
                sb.Append("</ul>");
                //slides container
                sb.Append("</div>");//slides
                sb.Append("</div></div></section>"); //span-4
                sb.Append("<div class=\"clear\"></div>");
            }

        }
    }
}

