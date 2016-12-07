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

namespace WET.Theme.GCWU.WebControls
{
    [DefaultProperty("Text"),
    ToolboxData("<{0}:LastModifiedDate runat=server></{0}:LastModifiedDate>")]
    public class LastModifiedDate : WebControl
    {
        /// <summary> 
        /// Render this control to the output parameter specified.
        /// </summary>
        /// <param name="output"> The HTML writer to write out to </param>
        protected override void Render(HtmlTextWriter output)
        {
            //get the placeholder that holds the meta tag content
            if (SPContext.Current.ListItem != null && PublishingPage.IsPublishingPage(SPContext.Current.ListItem))
            {
                PublishingPage page = PublishingPage.GetPublishingPage(SPContext.Current.ListItem);
                if (page != null)
                {
                    string sLastModifiedDate = page.LastModifiedDate.ToString("yyyy-MM-dd");
                    output.Write(sLastModifiedDate);
                }
            }
            else
            {
                if (SPContext.Current.Web.LastItemModifiedDate != null)
                {
                    string sLastModifiedDate = SPContext.Current.Web.LastItemModifiedDate.ToString("yyyy-MM-dd");
                    output.Write(sLastModifiedDate);
                }
            }
        }

    }
}
