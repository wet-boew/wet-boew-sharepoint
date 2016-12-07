using Microsoft.SharePoint;
using Microsoft.SharePoint.Publishing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace SPWET4.WebControls
{
    [DefaultProperty("Text"),
    ToolboxData("<{0}:SearchBox runat=server></{0}:SearchBox>")]
    public class SearchBox : WebControl
    {

        private TextBox txtSearchBox;

        public SearchBox() : base()
        {
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            try
            {
                string langWeb = string.Empty;

                if (SPContext.Current.ListItem != null && PublishingPage.IsPublishingPage(SPContext.Current.ListItem))
                {
                    // figure out our language of the current label
                    PublishingPage publishingPage = PublishingPage.GetPublishingPage(SPContext.Current.ListItem);

                    if (publishingPage.PublishingWeb.Label != null)
                        langWeb = (publishingPage.PublishingWeb.Label.Title.Substring(0, 2).ToLower() == "en") ? "eng" : "fra";
                }
                else
                {
                    string cultISO = "";
                    if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("/eng/"))
                        cultISO = "en";
                    else
                        cultISO = "fr";
                    langWeb = (cultISO == "en") ? "eng" : "fra";
                }

                HtmlGenericControl pnlSearch = new HtmlGenericControl("div");
                pnlSearch.Attributes.Add("class", "form-group");

                txtSearchBox = new TextBox()
                {
                    CssClass = "form-control",
                };
                txtSearchBox.Attributes.Add("type", "search");
                txtSearchBox.Attributes.Add("size", "27");
                txtSearchBox.MaxLength = 150;
                txtSearchBox.Style.Add("margin-right", "5px");
                pnlSearch.Controls.Add(txtSearchBox);

                this.Controls.Add(pnlSearch);

                HtmlButton btnSearch = new HtmlButton();
                btnSearch.Attributes.Add("class", "btn btn-default");
                btnSearch.Attributes.Add("type", "submit");
                btnSearch.InnerText = (langWeb == "fra") ? "Recherche" : "Search";
                btnSearch.ServerClick += btnSearch_ServerClick;

                this.Controls.Add(btnSearch);
            }
            catch(Exception)
            {
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            EnsureChildControls();
        }

        void btnSearch_ServerClick(object sender, EventArgs e)
        {
            EnsureChildControls();

            PublishingPage publishingPage = PublishingPage.GetPublishingPage(SPContext.Current.ListItem);
            string langWeb = string.Empty;
            if (publishingPage != null)
                langWeb = (publishingPage.PublishingWeb.Label.Title.Substring(0, 2).ToLower() == "en") ? "eng" : "fra";
            else if (SPContext.Current.Web.Url.ToLower().Contains("/fra/"))
                langWeb = "fra";
            else
                langWeb = "eng";

            string searchCentreURL = string.Empty;
            if (SPContext.Current.Site.RootWeb.AllProperties["SRCH_ENH_FTR_URL"] != null)
                searchCentreURL = SPContext.Current.Site.RootWeb.AllProperties["SRCH_ENH_FTR_URL"].ToString();
            else
            {
                if (langWeb == "eng")
                    searchCentreURL = SPContext.Current.Site.RootWeb.Url + "/" + langWeb + "/Search/Pages/results.aspx?k=" + txtSearchBox.Text;
                else
                    searchCentreURL = SPContext.Current.Site.RootWeb.Url + "/" + langWeb + "/Recherche/Pages/resultats.aspx?k=" + txtSearchBox.Text;
            }
            this.Page.Response.Redirect(searchCentreURL, true);
        }

        protected override void Render(HtmlTextWriter writer)
        {
            RenderContents(writer);
        }

    }
}
