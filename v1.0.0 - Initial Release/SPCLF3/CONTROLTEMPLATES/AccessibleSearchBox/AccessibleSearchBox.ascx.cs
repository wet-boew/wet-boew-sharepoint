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
            btnSearch.Attributes.Add("data-icon", "search");

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            PublishingPage publishingPage = PublishingPage.GetPublishingPage(SPContext.Current.ListItem);
            string langWeb = (publishingPage.PublishingWeb.Label.Title.Substring(0, 2).ToLower() == "en") ? "eng" : "fra";

            string searchCentreURL = SPContext.Current.Site.RootWeb.AllProperties["SRCH_ENH_FTR_URL"].ToString();                

            Response.Redirect(searchCentreURL + "/results.aspx?k=" + txtSearch.Text, true);
        }
    }
}
