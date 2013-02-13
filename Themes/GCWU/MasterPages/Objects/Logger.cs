using System;
using System.IO;
using Microsoft.SharePoint.Administration;

namespace WET.Theme.GCWU.Objects
{
    /// <summary>
    /// Summary description for Logger.
    /// </summary>
    public static class Logger
    {
        public static void WriteLog(string strLogMsg)
        {
            SPDiagnosticsService diagSvc = SPDiagnosticsService.Local;
            LogEngine.Log(new Exception(strLogMsg), "WET Theme");
            diagSvc.WriteTrace(0, new SPDiagnosticsCategory("WET Theme", TraceSeverity.Unexpected, EventSeverity.Error), TraceSeverity.Unexpected, "WET Theme for SharePoint 2010:  {0}", new object[] { strLogMsg });
        }
    }
}
