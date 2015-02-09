using System;
using System.IO;
using Microsoft.SharePoint.Administration;

namespace SPWET4.Objects
{
    /// <WET4Changes>
    ///     2014-11-24 This file does not need to be adjusted for WET4 as it doesn't do anything - BARIBF
    /// </WET4Changes>
    
    /// <summary>
    /// Summary description for Logger.
    /// </summary>
    public static class Logger
    {
        public static void WriteLog(string strLogMsg)
        {
            // Nik20131028 - Commented out to help clean the logs, and prevent infrastructure from thinking that this slows down the server...
            SPDiagnosticsService diagSvc = SPDiagnosticsService.Local;
            LogEngine.Log(new Exception(strLogMsg), "WET Theme");
            diagSvc.WriteTrace(0, new SPDiagnosticsCategory("WET Theme", TraceSeverity.Unexpected, EventSeverity.Error), TraceSeverity.Unexpected, "WET Theme for SharePoint 2010:  {0}", new object[] { strLogMsg });
        }
    }
}
