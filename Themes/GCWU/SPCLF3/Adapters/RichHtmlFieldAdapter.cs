using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.WebPartPages;
using System.Web.UI.WebControls.WebParts;
using System.Text.RegularExpressions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.WebControls;
using System.IO;
using System.Web.UI;

namespace SPCLF3.Adapters
{
    public class RichHtmlFieldAdapter:System.Web.UI.Adapters.ControlAdapter
    {
        public RichHtmlFieldAdapter() { }

        
        protected override void Render(HtmlTextWriter writer)
        {
            using (new SPMonitoredScope("RichHtmlFieldAdapter"))
            {
                if (SPContext.Current != null &&  SPContext.Current.FormContext.FormMode == SPControlMode.Display)
                {
                    StringBuilder sb = new StringBuilder();
                    using (new SPMonitoredScope("Render original content"))
                    {
                        using (StringWriter sw = new StringWriter(sb))
                        {
                            using (HtmlTextWriter htw = new HtmlTextWriter(sw))
                            {
                                base.Render(htw);
                            }
                        }
                    }

                    string content = sb.ToString();

                    int start = 0, end = 0;
                    bool foundOne = true;
                    while (foundOne)
                    {
                        foundOne = false;
                        // Nik20130115 - Remove the HasPers attribute;
                        start = content.IndexOf("HasPers=");
                        end = content.IndexOf("\"", start + 9);

                        if (end > start && start > 0)
                        {
                            foundOne = true;
                            content = content.Remove(start, end - start);
                        }

                        // Nik20130115 - Remove the WebPartID attribute;
                        start = content.IndexOf("WebPartID=");
                        end = content.IndexOf("\"", start + 11);

                        if (end > start && start > 0)
                        {
                            foundOne = true;
                            content = content.Remove(start, end - start);
                        }

                        // Nik20130115 - Remove the AllowDelete attribute;
                        start = content.IndexOf("AllowDelete=");
                        end = content.IndexOf("\"", start + 13);

                        if (end > start && start > 0)
                        {
                            foundOne = true;
                            content = content.Remove(start, end - start);
                        }

                        // Nik20130115 - Remove the AllowExport attribute;
                        start = content.IndexOf("AllowExport=");
                        end = content.IndexOf("\"", start + 13);

                        if (end > start && start > 0)
                        {
                            foundOne = true;
                            content = content.Remove(start, end - start);
                        }
                    }

                    writer.Write(content);
                }
                else
                {
                    base.Render(writer);
                }
            }
        }
    }
}
