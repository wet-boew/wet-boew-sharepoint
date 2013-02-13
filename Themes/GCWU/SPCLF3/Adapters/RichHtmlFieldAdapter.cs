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

namespace WET.Theme.GCWU.Adapters
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
                        start = content.ToLower().IndexOf("haspers=");
                        end = content.IndexOf("\"", start + 9);

                        if (end > start && start > 0)
                        {
                            foundOne = true;
                            string line = content.Substring(start - 1, end - start + 2);
                            content = content.Remove(start - 1, end - start + 2);
                        }

                        // Nik20130115 - Remove the WebPartID attribute;
                        start = content.ToLower().IndexOf("webpartid=");
                        end = content.IndexOf("\"", start + 11);

                        if (end > start && start > 0)
                        {
                            foundOne = true;
                            string line = content.Substring(start - 1, end - start + 2);
                            content = content.Remove(start - 1, end - start+2);
                        }

                        // Nik20130125 - Remove the WebPartID2 attribute;
                        start = content.ToLower().IndexOf("webpartid2=");
                        end = content.IndexOf("\"", start + 12);

                        if (end > start && start > 0)
                        {
                            foundOne = true;
                            string line = content.Substring(start - 1, end - start + 2);
                            content = content.Remove(start - 1, end - start + 2);
                        }

                        // Nik20130115 - Remove the AllowDelete attribute;
                        start = content.ToLower().IndexOf("allowdelete=");
                        end = content.IndexOf("\"", start + 13);

                        if (end > start && start > 0)
                        {
                            foundOne = true;
                            string line = content.Substring(start - 1, end - start + 2);
                            content = content.Remove(start - 1, end - start + 2);
                        }

                        // Nik20130115 - Remove the AllowExport attribute;
                        start = content.ToLower().IndexOf("allowexport=");
                        end = content.IndexOf("\"", start + 13);

                        if (end > start && start > 0)
                        {
                            foundOne = true;
                            string line = content.Substring(start - 1, end - start + 2);
                            content = content.Remove(start - 1, end - start + 2);
                        }

                        // Nik20130125 - Remove the AllowRemove attribute;
                        start = content.ToLower().IndexOf("allowremove=");
                        end = content.IndexOf("\"", start + 13);

                        if (end > start && start > 0)
                        {
                            foundOne = true;
                            string line = content.Substring(start - 1, end - start + 2);
                            content = content.Remove(start - 1, end - start + 2);
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
