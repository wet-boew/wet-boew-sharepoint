using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Publishing;
using System.Security;
using System.Web;
using SPCLF3.Objects;

namespace SPCLF3.WebControls
{
    [DefaultProperty("Text"),
    ToolboxData("<{0}:HomeLink runat=server></{0}:HomeLink>")]
    public class HomeLink : WebControl
    {
        /// <summary> 
        /// Render this control to the output parameter specified.
        /// </summary>
        /// <param name="output"> The HTML writer to write out to </param>
        protected override void Render(HtmlTextWriter output)
        {
            //Create a link back to the root of the variation
            if (SPContext.Current.ListItem != null && PublishingPage.IsPublishingPage(SPContext.Current.ListItem))
            {
                PublishingPage page = PublishingPage.GetPublishingPage(SPContext.Current.ListItem);
                if (page != null)
                {
                    try
                    {
                        // handle the homelink when variations are enabled
                        if (page.PublishingWeb.Label != null)
                        {
                            output.Write(
                                "<a href=\"" +
                                page.PublishingWeb.Label.TopWebUrl +
                                "\" style=\"font-size:1.5em;\">" +
                                HttpContext.GetGlobalResourceObject("CLF3", "SiteTitleText", SPContext.Current.Web.Locale).ToString() +
                                "</a>"
                            );
                        }
                        else
                        {
                            // when variations are not enabled
                            output.Write(
                                "<a href=\"" + SPContext.Current.Site.RootWeb.Url + "\" style=\"font-size:1.5em;\">" +
                                HttpContext.GetGlobalResourceObject("CLF3", "SiteTitleText", SPContext.Current.Web.Locale).ToString() +
                                "</a>"
                            );
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteLog(ex.Message + " " + ex.StackTrace);
                    }
                }
            }
            else
            {
                // when variations are not enabled
                output.Write(
                    "<a href=\"" + SPContext.Current.Site.RootWeb.Url + "\" style=\"font-size:1.5em;\">" +
                    HttpContext.GetGlobalResourceObject("CLF3", "SiteTitleText", SPContext.Current.Web.Locale).ToString() +
                    "</a>"
                );
            }
        }
    }
}
