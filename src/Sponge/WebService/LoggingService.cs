using System.Web.Script.Services;
using System.Web.Services;
using System.Xml;
using Sponge.Logging;
using Sponge.Utilities;

namespace Sponge.WebService
{
    [WebService(Namespace = "http://Sponge.WebService.ConfigService")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ScriptService]
    [System.ComponentModel.ToolboxItem(false)]

    public class LoggingService : System.Web.Services.WebService
    {
        [WebMethod]
        public XmlDocument GetCentral(string loggerName)
        {
            return LogManager.GetConfig(loggerName);
        }

        [WebMethod]
        public XmlDocument GetRelative(string spongeUrl, string loggerName)
        {
            return LogManager.GetConfig(spongeUrl, loggerName);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void LogCentral(string lvl, string msg)
        {
            var log = LogManager.GetOnline(Constants.SpongeLoggerWsname);
            var level = NLog.LogLevel.FromString(lvl);
            log.Log(level, msg);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void LogRelative(string spongeUrl, string lvl, string msg)
        {
            var log = LogManager.GetOnline(spongeUrl, Constants.SpongeLoggerWsname);
            var level = NLog.LogLevel.FromString(lvl);
            log.Log(level, msg);
        }
    }
}