using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;

namespace SPCLF3
{
    public static class LogEngine
    {
        public static void Log(Exception ex, string component)
        {
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.RootWeb.Url + "/Admin"))
                    {
                        using (SPWeb rootweb = site.OpenWeb())
                        {
                            SPList logs = rootweb.Lists["Logs"];
                            SPListItem item = logs.AddItem();
                            item["Message"] = ex.Message;
                            item["StackTrace"] = ex.StackTrace;
                            item["Component"] = component;
                            item.Update();
                            logs.Update();
                        }
                    }
                });
            }
            catch { }
        }
    }
}
