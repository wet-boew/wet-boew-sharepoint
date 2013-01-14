using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using Microsoft.SharePoint;

namespace LAC.SharePoint.Slider.SliderWebPart
{
    public partial class SliderWebPartUserControl : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected override void OnInit(EventArgs e)
        {
            SliderWebPart swp = (SliderWebPart)this.Parent;
            if (swp.ListID.Equals(Guid.Empty)) return;

            using (SPSite site = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb web = site.OpenWeb(swp.ListWebID))
                {

                    //SPList list = elevatedWeb.Lists.TryGetList("Slideshow");
                    SPList list = web.Lists[swp.ListID];
                    List<SPListItem> queryResults;
                    int count = swp.ItemLimit;

                    // Query for the first 'count' number of items in the list
                    IEnumerable<SPListItem> listItems = list.Items.OfType<SPListItem>();
                    queryResults = (from SPListItem item in listItems
                                    select item).ToList();

                    StringBuilder liItems = new StringBuilder();
                    StringBuilder tabItems = new StringBuilder();

                    for(int n = 0; n < count; n++)
                    {
                        if (n < queryResults.Count)
                        {
                            SPListItem itm = queryResults[n];
                            string title = itm.Title;
                            string src = MakeAbsoluteURL(web.Url, itm.Url);
                            string url = string.Empty;

                            if (null != itm["URL"])
                            {
                                string link = itm["URL"].ToString();
                                String[] urlArray = link.Split(',');
                                url = urlArray[0].ToString();
                            }
                            else
                            {
                                url = "javascript:void(0)";
                            }
                            string hRef = "#tab" + (n + 1).ToString();
                            liItems.AppendLine("<li><a class=\"swpTabsLink\" style=\"text-decoration: none !important;\" href=\"" + hRef + "\">" + title + "</a></li>");

                            string divId = "tab" + (n + 1).ToString();
                            tabItems.AppendLine("<div id=\"" + divId + "\" class=\"tabs-content-pad\">");
                            tabItems.AppendLine("<p><a href=\"" + url + "\"><img class=\"image-actual float-left\" src=\"" + src + "\" alt=\"" + title + "\"/></a></p>");
                            tabItems.AppendLine("</div>");
                        }
                    }
                    litTabs.Text = liItems.ToString();
                    litPanels.Text = tabItems.ToString() + "</div>";
                    sliderWPTabs.Attributes.Add("data-wet-boew", "cycle:" + swp.Speed.ToString());
                }
            }

            base.OnInit(e);
        }

        private string MakeAbsoluteURL(string webUrl, string itemUrl)
        {
            string siteURL = string.Empty;

            if (itemUrl.StartsWith("http://") || itemUrl.StartsWith("https://"))
            {
                siteURL = itemUrl;
            }
            else
            {
                siteURL = webUrl + "/" + itemUrl;
            }
            return siteURL;
        }

    }
}
