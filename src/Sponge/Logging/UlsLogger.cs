using System;
using System.Collections.Generic;
using Microsoft.SharePoint.Administration;
using NLog;

namespace Sponge.Logging
{
    public class UlsLogger : SPDiagnosticsService
    {
        private static string PRODUCT_NAME = "Sponge Logging Component";
        private static UlsLogger logService;

        public static UlsLogger LogService
        {
            get
            {
                if (logService == null)
                {
                    logService = new UlsLogger();
                }
                return logService;
            }
        }

        private UlsLogger() : base("Sponge ULS Logging Component", SPFarm.Local) { }

        protected override IEnumerable<SPDiagnosticsArea> ProvideAreas()
        {
            List<SPDiagnosticsArea> areas = new List<SPDiagnosticsArea>{
                new SPDiagnosticsArea(PRODUCT_NAME, new List<SPDiagnosticsCategory>{
                    new SPDiagnosticsCategory("Off", TraceSeverity.None, EventSeverity.None),
                    new SPDiagnosticsCategory("Fatal", TraceSeverity.Unexpected, EventSeverity.ErrorCritical),
                    new SPDiagnosticsCategory("Error", TraceSeverity.High, EventSeverity.Error),
                    new SPDiagnosticsCategory("Warn", TraceSeverity.Medium, EventSeverity.Warning),
                    new SPDiagnosticsCategory("Info", TraceSeverity.Monitorable, EventSeverity.Information),
                    new SPDiagnosticsCategory("Debug", TraceSeverity.Verbose, EventSeverity.Verbose),
                    new SPDiagnosticsCategory("Trace", TraceSeverity.VerboseEx, EventSeverity.Verbose)
                })
            };

            return areas;
        }

        public static void Log(LogLevel level, string msg, params object[] args)
        {
            var log = string.Format(msg, args);
            SPDiagnosticsCategory category = UlsLogger.LogService.Areas[PRODUCT_NAME].Categories[level.ToString()];
            UlsLogger.logService.WriteTrace(0, category, category.TraceSeverity, log);
        }

        public static void Log(LogLevel level, Exception ex)
        {
            SPDiagnosticsCategory category = UlsLogger.LogService.Areas[PRODUCT_NAME].Categories[level.ToString()];
            UlsLogger.logService.WriteTrace(0, category, category.TraceSeverity, ex.ToString());
        }
    }
}
