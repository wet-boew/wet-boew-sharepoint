using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;


/*
 * No change needed for WET 4 integration - BARIBF 2014-11-24
 */ 

namespace SPWET4
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
                            rootweb.AllowUnsafeUpdates = true;
                            SPList logs = rootweb.Lists.TryGetList("Logs");
                            if (logs != null)
                            {
                                SPListItem item = logs.AddItem();
                                if (ex.Message.Length > 100)
                                    item["Message"] = ex.Message.Substring(0, 100);
                                else
                                    item["Message"] = ex.Message;
                                if (ex.StackTrace != null)
                                    item["StackTrace"] = ex.StackTrace;
                                item["Component"] = component;
                                item.Update();
                                logs.Update();
                            }
                        }
                    }
                });
            }
            catch { }
        }
    }
}
