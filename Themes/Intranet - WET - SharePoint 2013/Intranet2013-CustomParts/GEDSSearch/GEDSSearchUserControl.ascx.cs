
using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using System.Text;
using System.Linq;
using Microsoft.SharePoint.WebControls;
using System.Collections.Generic;



namespace Intranet2013_CustomParts.GEDSSearch
{
    public partial class GEDSSearchUserControl : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int LCID = HttpContext.Current.Request.Url.Segments.Contains("fra/") ? 1036 : 1033;

            string WebpartTitle = "";// "<h3> &nbsp;";

            External.Text = LCID == 1036 ? "SAGE" : "External";
            Internal.Text = LCID == 1036 ? "SAGE-BAC" : "Internal-LAC";
            btnGEDS.Text = LCID == 1036 ? "Allez-y" : "Go";
            WebpartTitle += LCID == 1036 ? " Sage - Trouver une personne" : " GEDS - Find a person ";
            //updateLink.HRef = LCID == 1036 ? "http://sharepoint.lac-bac.int/SiteDirectory/App/BC/SitePages/GEDS%20Modification%20Form.aspx" : "http://sharepoint.lac-bac.int/SiteDirectory/App/BC/SitePages/GEDS%20Modification%20Form.aspx";
            //updateLink.InnerText = LCID == 1036 ? "Demande de modification du SAGE" : "GEDS Modification Request";
            ///WebpartTitle += "</h3>";
            //gedstitle.InnerText = WebpartTitle;
            GEDSh3.InnerText = WebpartTitle;
        }


        protected void btnGEDS_Click(object sender, EventArgs e)

        {
            //Read the option selected to re direct two links that should help me build the link from the textbox
            //http://geds20-sage20.ssc-spc.gc.ca/fr/SAGE20/?pgid=011&sf=0&sc=1&cdn=ou=LAC-BAC,%20o=GC,%20c=CA&sv=
            //http://geds20-sage20.ssc-spc.gc.ca/en/GEDS20/?pgid=011&sf=0&sc=1&cdn=ou=LAC-BAC, o=GC, c=CA&sv=
            //http://geds20-sage20.ssc-spc.gc.ca/fr/SAGE20/?pgid=008&cdn=&sv=jan
            //Links Changed June 1st 2015 only for Departmental search
            //http://geds20-sage20.ssc-spc.gc.ca/en/GEDS20/?pgid=014&dn=ou%3DLAC-BAC%2C+o%3DGC%2C+c%3DCA&sv=
            //http://geds20-sage20.ssc-spc.gc.ca/fr/SAGE20/?pgid=014&dn=ou%3DLAC-BAC%2C+o%3DGC%2C+c%3DCA&sv=

            int LCID = (SPContext.Current.Web.Url.ToLower().Contains("/fra/") ? 1036 : 1033);
            String frweblink = "/fr/SAGE20/";
            String enweblink = "/en/GEDS20/";
            string qs = "";
            if (Internal.Checked == true)
            {
                qs = "?pgid=011&sf=0&sc=1&cdn=ou=LAC-BAC, o=GC, c=CA&sv=";
                
            }
            else
            {
                qs = "?pgid=008&cdn=&sv=";
            }
            qs = qs + Name.Text.ToString();

            String redirectLink = "http://geds20-sage20.ssc-spc.gc.ca" + (LCID == 1036 ? frweblink : enweblink) + qs;
            //this.Parent.Page.Response.Redirect(redirectLink); This is a better way of redirecting
            System.Web.HttpContext.Current.Response.Redirect(redirectLink, true);
            //
        }

        protected void GEDSRadio_CheckedChanged(object sender, EventArgs e)
        {

        }

        protected void Name_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
