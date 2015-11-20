using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace webparts.Feed.ascx
{
    [ToolboxItemAttribute(false)]
    public class Feed : WebPart
    {
        // Visual Studio might automatically update this path when you change the Visual Web Part project item.
        private const string _ascxPath = @"~/_CONTROLTEMPLATES/15/webparts/Feed/FeedUserControl.ascx";
        private string newsListName = "NewsFeed";
        private string webpartHeaderEn = "News";
        private string webpartHeaderFr = "Nouvelles";

        private string strUpEn = "Up";
        private string strUpFr = "en haut";

        private string strDownEn = "Down";
        private string strDownFr = "en bas";

        private string strAllNewsLinkEn = "/eng/Pages/News";
        private string strAllNewsLinkFr = "/fra/Pages/Nouvelles";

        [WebBrowsable(true)]
        [WebDisplayName("WebpartHeaderEn")]
        [WebDescription("Header English")]
        [Personalizable(PersonalizationScope.Shared)]
        [Category("News Setting")]
        public string WebpartHeaderEn
        {
            get { return webpartHeaderEn; }
            set { webpartHeaderEn = value; }
        }


        [WebBrowsable(true)]
        [WebDisplayName("WebpartHeaderFr")]
        [WebDescription("Header French")]
        [Personalizable(PersonalizationScope.Shared)]
        [Category("News Setting")]
        public string WebpartHeaderFr
        {
            get { return webpartHeaderFr; }
            set { webpartHeaderFr = value; }
        }

        [WebBrowsable(true)]
        [WebDisplayName("NewsListName")]
        [WebDescription("List name that stores all news")]
        [Personalizable(PersonalizationScope.Shared)]
        [Category("News Setting")]

        public string NewsListName
        {
            get { return newsListName; }
            set { newsListName = value; }
        }




        [WebBrowsable(true)]
        [WebDisplayName("StrUpEn")]
        [WebDescription("Up button English text")]
        [Personalizable(PersonalizationScope.Shared)]
        [Category("News Setting")]
        public string StrUpEn
        {
            get { return strUpEn; }
            set { strUpEn = value; }
        }
        [WebBrowsable(true)]
        [WebDisplayName("StrUpFr")]
        [WebDescription("Up button French text")]
        [Personalizable(PersonalizationScope.Shared)]
        [Category("News Setting")]
        public string StrUpFr
        {
            get { return strUpFr; }
            set { strUpFr = value; }
        }


        [WebBrowsable(true)]
        [WebDisplayName("StrDownEn")]
        [WebDescription("Down button English text")]
        [Personalizable(PersonalizationScope.Shared)]
        [Category("News Setting")]
        public string StrDownEn
        {
            get { return strDownEn; }
            set { strDownEn = value; }
        }
        [WebBrowsable(true)]
        [WebDisplayName("StrUpFr")]
        [WebDescription("Down button French text")]
        [Personalizable(PersonalizationScope.Shared)]
        [Category("News Setting")]
        public string StrDownFr
        {
            get { return strDownFr; }
            set { strDownFr = value; }
        }



        [WebBrowsable(true)]
        [WebDisplayName("StrAllNewsLinkEn")]
        [WebDescription("All News Link English Url")]
        [Personalizable(PersonalizationScope.Shared)]
        [Category("News Setting")]
        public string StrAllNewsLinkEn
        {
            get { return strAllNewsLinkEn; }
            set { strAllNewsLinkEn = value; }
        }
        [WebBrowsable(true)]
        [WebDisplayName("StrAllNewsLinkFr")]
        [WebDescription("All News Link French Url")]
        [Personalizable(PersonalizationScope.Shared)]
        [Category("News Setting")]
        public string StrAllNewsLinkFr
        {
            get { return strAllNewsLinkFr; }
            set { strAllNewsLinkFr = value; }
        }


        protected override void CreateChildControls()
        {
            try 
            {
               // Control control = Page.LoadControl(_ascxPath);
                //Controls.Add(control);
                //base.CreateChildControls();
                //var webPart = (WebPart)Parent;
                //Control control = Page.LoadControl(_ascxPath);
                //have to add parent control so I could use the properties
                FeedUserControl control = Page.LoadControl(_ascxPath) as FeedUserControl;
                control.ParentWebPartControl = this;

                if (control != null)
                {
                    Controls.Add(control);
                }
                
            }
            catch
            {

            }
        }
    }
}
