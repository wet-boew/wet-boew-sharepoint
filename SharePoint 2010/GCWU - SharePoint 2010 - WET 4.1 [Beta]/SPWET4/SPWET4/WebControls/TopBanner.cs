using Microsoft.SharePoint;
using Microsoft.SharePoint.Publishing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SPWET4.WebControls
{
    [DefaultProperty("Text"),
    ToolboxData("<{0}:TopBanner runat=server></{0}:TopBanner>"),
    ]
    public class TopBanner : WebControl
    {

        public TopBanner()
            : base(HtmlTextWriterTag.Div)
        {
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            EnsureChildControls();

            this.Visible = false; // Control is assumed to be invisible

            try
            {
                // Look for the image inside the banner library
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    SPWeb web = site.RootWeb;

                    // Determine current language
                    PublishingWeb pWeb = PublishingWeb.GetPublishingWeb(SPContext.Current.Web);
                    string language = (pWeb.Label.Title.ToLower() == "fra") ? "French" : "English";

                    SPList list = web.Lists["TopBanner"];

                    SPQuery query = new SPQuery();
                    query.Query = "<Where><Eq><FieldRef Name=\"Language\" /><Value Type=\"Text\">" + language + "</Value></Eq></Where>";

                    SPListItemCollection items = list.GetItems(query);
                    if (items.Count >= 1)
                    {
                        this.Visible = true;

                        SPListItem item = items[0];
                        SPFieldUrlValue urlValue = new SPFieldUrlValue((string)item["URL"]);

                        this.CssClass = "span-8 row-start row-end";
                        this.Style.Add("max-height", "165px");
                        this.Style.Add("margin", "0 auto !important");
                        this.Style.Add("padding", "5px 10px 0 10px");
                        this.Style.Add("float", "none !important");
                        this.Style.Add("overflow", "hidden");
                        this.Style.Add("background-image", "url(data:image/gif;base64,R0lGODlhAQABAIAAAMzMzAAAACH5BAAAAAAALAAAAAABAAEAAAICRAEAOw==),url(data:image/gif;base64,R0lGODlhAQABAIAAAMzMzAAAACH5BAAAAAAALAAAAAABAAEAAAICRAEAOw==)");
                        this.Style.Add("background-position", "left top,right top");
                        this.Style.Add("background-repeat", "repeat-y");
                        this.Style.Add("background-color", "#FFF");

                        Image image = new Image()
                        {
                            ImageUrl = item.File.ServerRelativeUrl,
                            AlternateText = (string)item["Description"],
                        };

                        // Create a hyperlink is a URL was provided
                        if (!String.IsNullOrEmpty((string)item["URL"]))
                        {
                            HyperLink link = new HyperLink()
                            {
                                NavigateUrl = urlValue.Url,
                            };
                            link.Controls.Add(image);
                            this.Controls.Add(link);
                        }
                        else
                            this.Controls.Add(image);
                    }
                }
            }
            catch (Exception)
            {
            }

        }

    }
}
