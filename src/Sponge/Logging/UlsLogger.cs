using System;
using System.Collections.Generic;
using Microsoft.SharePoint.Administration;
using NLog;

namespace Sponge.Logging
{
    public class UlsLogger : SPDiagnosticsService
    {
        private const string ProductName = "Sponge Logging Component";
        private static UlsLogger _logService;

        public static UlsLogger LogService
        {
            get { return _logService ?? (_logService = new UlsLogger()); }
        }

        private UlsLogger() : base("Sponge ULS Logging Component", SPFarm.Local) { }

        protected override IEnumerable<SPDiagnosticsArea> ProvideAreas()
        {
            var areas = new List<SPDiagnosticsArea>{
                new SPDiagnosticsArea(ProductName, new List<SPDiagnosticsCategory>{
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
            var category = LogService.Areas[ProductName].Categories[level.ToString()];
            _logService.WriteTrace(0, category, category.TraceSeverity, log);
        }

        public static void Log(LogLevel level, Exception ex)
        {
            var category = LogService.Areas[ProductName].Categories[level.ToString()];
            _logService.WriteTrace(0, category, category.TraceSeverity, ex.ToString());
        }
    }
}
