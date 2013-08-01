using System.Web.Services;
using System.Xml;
using Sponge.Logging;
using Sponge.Utilities;

namespace Sponge.WebService
{
    [WebService(Namespace = "http://Sponge.WebService.ConfigService")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]

    public class LoggingService : System.Web.Services.WebService
    {
        private static NLog.Logger _log = LogManager.GetOnline(Constants.SPONGE_LOGGER_WSNAME);

        [WebMethod]
        public XmlDocument Get(string loggerName)
        {
            return LogManager.GetConfig(loggerName);
        }

        [WebMethod]
        public void Log(string lvl, string msg)
        {
            var level = NLog.LogLevel.FromString(lvl);
            _log.Log(level, msg);
        }
    }
}