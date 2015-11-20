using Microsoft.SharePoint;
using System;
using System.ComponentModel;
using System.Text;
using System.Web.UI.WebControls.WebParts;
using System.Web;
using System.Linq;

namespace WET.Theme.WebParts.ImagesSlider
{
    [ToolboxItemAttribute(false)]
    public partial class ImagesSlider : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]

        private int displayTime = 5;
        private string imageLibraryName = "LACImagesSlider";


        [WebBrowsable(true)]
        [WebDisplayName("ImageLibraryName")]
        [WebDescription("Library name that stores all images for slider to use")]
        [Personalizable(PersonalizationScope.User)]
        [Category("Image Slider Setting")]
        public string ImageLibraryName
        {
            get { return imageLibraryName; }
            set { imageLibraryName = value; }
        }


        [WebBrowsable(true)]
        [WebDisplayName("DisplayTime")]
        [WebDescription("Display time between images")]
        [Personalizable(PersonalizationScope.User)]
        [Category("Image Slider Setting")]
        public int DisplayTime
        {
            get { return displayTime; }
            set { displayTime = value; }
        }


        public enum SlideEffect
        {
            slideHori = 0,
            slideVert,
            fade,
            resize,
            none
        };

        protected SlideEffect effect;

        [WebBrowsable(true)]
        [WebDisplayName("SlideEffect")]
        [WebDescription("Slide Effect ")]
        [Personalizable(PersonalizationScope.User)]
        [Category("Image Slider Setting")]

        public SlideEffect Effect
        {
            get { return effect; }
            set { effect = value; }
        }


        public ImagesSlider()
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
                            int LCID = HttpContext.Current.Request.Url.Segments.Contains("fra/") ? 1036 : 1033;// System.Threading.Thread.CurrentThread.CurrentUICulture.LCID;

                            StringBuilder sb = new StringBuilder();

                            SPList list = web.Lists.TryGetList(this.ImageLibraryName);
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

                SPQuery oQuery = new SPQuery();
                int i = 1;
                int j = 1;
                SPListItemCollection collListItems;
                string LANG = lcid == 1033 ? "ENG" : "FRA";
                oQuery.Query = "<Where><And><Eq><FieldRef Name='ShowInSlider' /><Value Type='Choice'>YES</Value></Eq><Eq><FieldRef Name='SlideLanguage' /><Value Type='Choice'>"+LANG+"</Value></Eq></And></Where><OrderBy><FieldRef Name='ItemOrder' Ascending='True' /></OrderBy></Query>";

                collListItems = list.GetItems(oQuery);

                if (collListItems.Count > 0)
                {
                    sb.Append("<section>");
                    sb.Append("<div class=\"span-6\">");
                    sb.Append("<div class=\"wet-boew-tabbedinterface auto-play embedded-grid tabs-style-6 animate cycle slide-horz\">");
                    sb.Append("<div class=\"tabs-panel\">");

                    foreach (SPListItem item in collListItems)
                    {
                        sb.Append("<div id=\"tab" + i.ToString() + "\" class=\"span-6 margin-bottom-none\"><section>");
                        string url = new SPFieldUrlValue(item["URL"].ToString()).Url.ToString();
                        sb.Append("<p><a href=\""+url+"\"><img imgCustomclass class=\"imgCustomclass margin-bottom-none\" src=\"" + item.File.ServerRelativeUrl + "\"/></a></p>");
                        //if (lcid == 1036)
                        //{
                        //    sb.Append("<div class=\"span-6 position-left position-bottom opacity-90 background-dark\"><p>");
                        //    sb.Append(item["French Description"].ToString());
                        //    sb.Append("</p></div>");
                        //}
                        //else
                        //{
                            sb.Append("<div class=\"span-6 position-left position-bottom opacity-90 background-dark\"><p>");
                            sb.Append(item["Description"].ToString());
                            sb.Append("</p></div>");

                       // }
                        i++;
                        sb.Append("</section></div>");
                    }

                    sb.Append("</div><ul class=\"tabs\">");

                    foreach (SPListItem item in collListItems)
                    {
                        sb.Append("<li class=\"img\"><a href=\"" + "#tab" + j.ToString() + "\"><img style=\"width:100px !important;\" class= \"image-actual\" src=\"" + item.File.ServerRelativeUrl + "\" /></a></li>");
                        j++;
                    }

                    sb.Append("</ul></div>");// div class wb-tabs
                    sb.Append("</div></section>");

                }
            }

        }



    }
}

