using System.Text;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Publishing;
using System.Web.UI;
using System.Web;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Security;
using SPWET4.Objects;

namespace SPWET4.WebControls
{
    [ToolboxData("<{0}:ScriptManager runat=\"server\" />")]
    public class ScriptManager : Control
    {
        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);

            try
            {
                HttpBrowserCapabilities browserInfo = HttpContext.Current.Request.Browser;
                int browserVersion = browserInfo.MajorVersion;

                switch (Condition.ToLower())
                {
                    case ("lte"):
                        if (browserVersion <= int.Parse(Threshold))
                            writer.WriteLine("<script src=\"" + Source + "\" type=\"text/javascript\">" + "<" + "/" + "script>");
                        break;

                    case ("gt"):
                        if (browserVersion > int.Parse(Threshold))
                            writer.WriteLine("<script src=\"" + Source + "\" type=\"text/javascript\">" + "<" + "/" + "script>");
                        break;

                    case ("gte"):
                        if (browserVersion >= int.Parse(Threshold))
                            writer.WriteLine("<script src=\"" + Source + "\" type=\"text/javascript\">" + "<" + "/" + "script>");
                        break;

                    default:
                        if (browserVersion < int.Parse(Threshold))
                            writer.WriteLine("<script src=\"" + Source + "\" type=\"text/javascript\">" + "<" + "/" + "script>");
                        break;
                }

            }
            catch { }
        }

        public string Source
        {
            get;
            set;
        }

        public string Threshold
        { get; set; }

        public string Condition
        { get; set; }
    }
}
