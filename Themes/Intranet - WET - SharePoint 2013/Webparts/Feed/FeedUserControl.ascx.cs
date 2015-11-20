using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using System.Web;
using System.Text;
using Microsoft.SharePoint.WebControls;
using System.Text.RegularExpressions;
using System.ComponentModel;

namespace webparts.Feed.ascx
{
    public partial class FeedUserControl : UserControl
    {

        //Define the parent webpart
        public Feed ParentWebPartControl { get; set; }

        //declare variables for webpart properties
        private string newsListName = "NewsFeed";
        private string webpartHeaderEn = "News";
        private string webpartHeaderFr = "Nouvelles";

        private string strUpEn = "Up";
        private string strUpFr = "en haut";

        private string strDownEn = "Down";
        private string strDownFr = "en bas";

        private string strAllNewsLinkEn = "/eng/Pages/News";
        private string strAllNewsLinkFr = "/fra/Pages/Nouvelles";
        
        //Declare webpart property items here
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



        protected void Page_Load(object sender, EventArgs e)
        {
            using (SPSite site = new SPSite(SPContext.Current.Site.Url))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    try
                    {
                        int LCID = (HttpContext.Current.Request.Url.ToString().Contains("fra/") ? 1036 : 1033);
                        StringBuilder sb = new StringBuilder();
                        string listName = this.ParentWebPartControl.NewsListName.ToString();//"MyNewsList";
                        //SPList list = web.Lists.TryGetList(listName);// web.Lists[this.ParentWebPartControl.NewsListName]; //web.Lists.TryGetList();
                        //************************************
                        SPList list = web.Lists.TryGetList(listName);// web.Lists[this.ParentWebPartControl.NewsListName]; //web.Lists.TryGetList();
                       
                        if (list != null && list.ItemCount > 0)
                        {
                            //SPQuery myquery = new SPQuery();
                            SPListItemCollection lic;
                            //myquery.Query="<Where><IsNotNull><FieldRef Name='Title'/></IsNotNull></Where><OrderBy><FieldRef Name='Publish Date' Ascending='False' /></OrderBy>";
                            //myquery.RowLimit = 20;
                            //lic = list.GetItems(myquery);
                            //Only Add title if there are any items.
                            // if (lic.Count>0)

                            string titleText = "";
                                titleText = (LCID == 1036) ? this.ParentWebPartControl.WebpartHeaderFr.ToString() : this.ParentWebPartControl.WebpartHeaderEn.ToString();
                            string titleUrl = "";
                                titleUrl = (LCID == 1036) ? this.ParentWebPartControl.StrAllNewsLinkFr.ToString() : this.ParentWebPartControl.StrAllNewsLinkEn.ToString();
                            sb.Append("<h2 class='NewsFeedTitle'>");
                            sb.AppendFormat("<a href='{0}'>{1}</a>", titleUrl, titleText);
                            sb.Append("</h2>");
                            lic = list.Items;
                            if (list.ItemCount > 0)
                            {


                                sb.Append("<div class=\"NewsFeed\">");
                                sb.Append("<ul id=\"Feeder\">");

                                foreach (SPListItem li in lic)
                                {
                                    sb.Append("<li>");
                                    sb.Append("<div class=\"NewsFeedItem\">");
                                    sb.Append("<a href=\"" + (LCID == 1036 ? li["French Url"] : li["English Url"]) + "\">");
                                    sb.Append(LCID == 1036 ? li["French Link Text"].ToString() : li["English Link Text"].ToString());
                                    sb.Append("</a>");
                                    sb.Append("</div>");
                                    sb.Append("<div class=\"NewsFeedDate\">" + String.Format("{0:d MMMM, yyyy}", Convert.ToDateTime(li["Publish Date"].ToString())) + "</div>");
                                    sb.Append("</li>");

                                }//end for list item loop

                                sb.Append("</ul>");
                                sb.Append("</div>");

                            }
                        }

                        //************************************
                        Literal1.Text = sb.ToString();
                    }
                    catch
                    {

                    }
                }//using spweb
            }//using spsite
        }
    }
}
