using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;

namespace SPCLF3.WebParts
{
    public static class LogEngine
    {
        public static bool Log(Exception ex, string components)
        {
            try
            {
                string url = SPContext.Current.Site.Url + "/Admin";
                using (SPSite site = new SPSite(url))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        SPList logs = web.Lists["Logs"];
                        SPListItem entry = logs.AddItem();
                        entry["Title"] = "View";
                        entry["Message"] = ex.Message.ToString();
                        entry["StackTrace"] = ex.StackTrace.ToString();
                        entry["Component"] = components;
                        entry.Update();
                        logs.Update();
                    }
                }
            }
            catch { return false; }
            return true;
        }
    }
}
