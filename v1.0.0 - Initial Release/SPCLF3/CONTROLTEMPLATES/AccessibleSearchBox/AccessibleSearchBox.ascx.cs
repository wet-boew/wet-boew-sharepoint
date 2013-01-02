using System;
using System.Web.UI;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Publishing;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace SPCLF3.CONTROLTEMPLATES.AccessibleSearchBox
{
    public partial class AccessibleSearchBox : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string url = SPContext.Current.Web.Url;
            string parentUrl = SPContext.Current.Site.RootWeb.Url.ToLower();
            string langWeb = "eng";
            if (url.ToLower().Contains(parentUrl + "/fra"))
                langWeb = "fra";
            else 
                langWeb = "eng";

            if (langWeb == "fra")
                btnSearch.Text = "Recherche";
            else
                btnSearch.Text = "Search";            
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            PublishingPage publishingPage = PublishingPage.GetPublishingPage(SPContext.Current.ListItem);
            string langWeb = string.Empty;
            if(publishingPage != null)
                langWeb = (publishingPage.PublishingWeb.Label.Title.Substring(0, 2).ToLower() == "en") ? "eng" : "fra";
            else if(SPContext.Current.Web.Url.ToLower().Contains("/fra/"))
                langWeb = "fra";
            else
                langWeb = "eng";

            string searchCentreURL = string.Empty;
            if (SPContext.Current.Site.RootWeb.AllProperties["SRCH_ENH_FTR_URL"] != null)
                searchCentreURL = SPContext.Current.Site.RootWeb.AllProperties["SRCH_ENH_FTR_URL"].ToString();
            else
            {
                if (langWeb == "eng")
                    searchCentreURL = SPContext.Current.Site.RootWeb.Url + "/" + langWeb + "/Search/Pages/results.aspx?k=" + txtSearch.Text;
                else
                    searchCentreURL = SPContext.Current.Site.RootWeb.Url + "/" + langWeb + "/recherche/Pages/resultats.aspx?k=" + txtSearch.Text;
            }
            Response.Redirect(searchCentreURL, true);
        }
    }
}
