using System;
using System.IO;
using Microsoft.SharePoint.Administration;

namespace SPCLF3.Objects
{
    /// <summary>
    /// Summary description for Logger.
    /// </summary>
    public class Logger
    {
        public Logger()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public static void WriteLog(string strLogMsg)
        {
            SPDiagnosticsService diagSvc = SPDiagnosticsService.Local;
            diagSvc.WriteTrace(0, new SPDiagnosticsCategory("CLF3Toolkit", TraceSeverity.Monitorable, EventSeverity.Error), TraceSeverity.Monitorable, "CLF3 Toolkit for SharePoint 2010:  {0}", new object[] { strLogMsg });
        }
    }
}
