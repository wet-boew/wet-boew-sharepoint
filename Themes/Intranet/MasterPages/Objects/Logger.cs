using System;
using System.IO;
using Microsoft.SharePoint.Administration;

namespace WET.Theme.Intranet.Objects
{
    /// <summary>
    /// Summary description for Logger.
    /// </summary>
    public static class Logger
    {
        public static void WriteLog(string strLogMsg)
        {
            SPDiagnosticsService diagSvc = SPDiagnosticsService.Local;
            diagSvc.WriteTrace(0, new SPDiagnosticsCategory("WET Intranet Theme", TraceSeverity.Unexpected, EventSeverity.Error), TraceSeverity.Unexpected, "WET Intranet Theme for SharePoint 2010:  {0}", new object[] { strLogMsg });
        }
    }
}
