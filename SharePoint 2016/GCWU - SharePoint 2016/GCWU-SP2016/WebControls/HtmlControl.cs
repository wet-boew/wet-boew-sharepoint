using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Publishing;
using System.Security;
using System.Web;
using SPWET4.Objects;
using System.Web.UI.HtmlControls;

namespace SPWET4.WebControls
{
    [DefaultProperty("Text"),
    ToolboxData("<{0}:HtmlControl runat=server></{0}:HtmlControl>")]
    public class HtmlControl : HtmlGenericControl
    {

        public HtmlControl()
            : base()
        {
            this.TagName = "html";
        }

        public HtmlControl(string tag)
            : base(tag)
        {
            this.TagName = tag;
        }

        protected override void Render(HtmlTextWriter writer)
        {
            string lang = HttpContext.Current.Request.Url.AbsolutePath.ToLower().StartsWith("/fra") ? "fr" : "en";
            writer.WriteLine("<!--[if lt IE 9]><html class=\"no-js lt-ie9\" lang=\"" + lang + "\" dir=\"ltr\"><![endif]-->");
            writer.WriteLine("<!--[if gt IE 8]><!--><html class=\"no-js\" lang=\"" + lang + "\" dir=\"ltr\"><!--<![endif]-->");
            foreach (Control c in this.Controls)
            {
                c.RenderControl(writer);
            }
            writer.WriteLine("</html>");
        }

    }
}
