using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Text;
using Microsoft.SharePoint;
using System.ComponentModel;
using System.Web;
using System.Linq;

namespace WET.Theme.WebParts.CollabLink
{
    public partial class CollabLinkUserControl : UserControl
    {
       

        
        protected void Page_Load(object sender, EventArgs e)
        {
            //Shireeh Added to avoid issues with Annonymous access
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
                               SPList list = web.Lists.TryGetList("LACPortalLink");
                               generataSliderHTML(sb, list, LCID);

                               Literal1.Text = sb.ToString();
                           }
                           catch (Exception ex)
                           {
                               Literal1.Text = ex.ToString();
                           }
                       }
                   }
               });//run with elevated
        }//pageload
        
         private void generataSliderHTML(StringBuilder sb, SPList list, int lcid)
        {


            SPQuery oQuery = new SPQuery();
            SPListItemCollection collListItems;

            oQuery.Query = "<Where><IsNotNull><FieldRef Name='ID'/></IsNotNull></Where>" +
                                        "<OrderBy><FieldRef Name='ItemOrder' /></OrderBy>";
            collListItems = list.GetItems(oQuery);






            if (sb != null & list != null && list.Items.Count > 0)
            {
                sb.Append("<div class=\"span-2\" style=\"margin: 0 0 0 0;padding: 0 0 0 0;\"> ");
                int i = 1;

                foreach (SPListItem item in collListItems)
                {
                    if (i <= 1)
                    {
                        if (i == 1 && lcid == 1033)
                            //sb.Append(" <div class=\"personLinkRow\" style=\"focus:block; display:block;  cursor:pointer; margin: 0 0 0 0;padding: 0 0 0 0; \"><a style=\"color: white !important;\" href=\"" + item["English Url"] + "\"><div class=\"align-right\" onclick=\"location.href='" + item["English Url"] + "';\" style=\"focus:block; display:block;  cursor:pointer; margin: 0 0 0 0;padding: 0 0 0 0; \" >" + "<img class=\"float-left\"  src=\"" + item.File.ServerRelativeUrl + "\" />");
                            sb.Append(" <div class=\"personLinkRow\" style=\"focus:block; display:block;  cursor:pointer; margin: 0 0 0 0;padding: 0 0 0 0; \" onclick=\"location.href='" + item["English Url"] + "';\"  ><a style=\"color: white !important;\" href=\"" + item["English Url"] + "\">");
                        else
                        {
                            if (i == 1 && lcid == 1036)
                               // sb.Append("<li class=\"personLinkRow\"><div class= \"float-left\" onclick=\"location.href='" + item["French Url"] + "';\" >" + item["French Link Text"] + "</br><a style=\"color: white !important;\" href=\"" + item["French Url"] + "\"> </div><div class=\"align-right\" onclick=\"location.href='" + item["French Url"] + "';\" >" + "<img class=\"float-right\"  height: 45px;\" src=\"" + item.File.ServerRelativeUrl + "\" /></a>");
                                //sb.Append("<div class=\"personLinkRow\" style=\"focus:block; display:block;  cursor:pointer; margin: 0 0 0 0;padding: 0 0 0 0; \" ><a style=\"color: white !important;\" href=\"" + item["French Url"] + "\"><div class=\"align-right\" onclick=\"location.href='" + item["French Url"] + "';\" style=\"focus:block; display:block;  cursor:pointer; margin: 0 0 0 0;padding: 0 0 0 0; \" >" + "<img class=\"float-left\"  src=\"" + item.File.ServerRelativeUrl + "\" /> ");                         
                                sb.Append(" <div class=\"personLinkRow\" style=\"focus:block; display:block;  cursor:pointer; margin: 0 0 0 0;padding: 0 0 0 0; \" onclick=\"location.href='" + item["French Url"] + "';\"  ><a style=\"color: white !important;\" href=\"" + item["French Url"] + "\">");
                            }

                        sb.Append("</a></div><div class=\"clear\" class=\"sideImageLinksBottomClear;\"></div>");
                        i++;
                    }
                    else
                        break;
                }
                
                sb.Append("</div>");


            }
        }


        private void registerClientScirptCSS(StringBuilder sb)
        {
            if (sb != null)
            {
                sb.Append("<link rel=\"stylesheet\" type=\"text/css\" href=\"/Style Library/SideImageLink/SideImageLink.css\"/>");

            }

        }
        
        }

    }

