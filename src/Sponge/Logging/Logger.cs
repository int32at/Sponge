using NLog;
using NLog.Config;
using Sponge.Utilities;

namespace Sponge.Logging
{
    public class Logger : NLog.Logger
    {
        private static string _loggerName = "SpongeLogger";
        public static Logger GetOnline(string loggerName)
        {
            //load config from SP
            var config = new XmlLoggingConfiguration(null, null);
            LogManager.Configuration = config;
            return (Logger)LogManager.GetLogger(loggerName);
        }

        public static Logger GetOffline(string configPath)
        {
            var config = new XmlLoggingConfiguration(configPath);
            LogManager.Configuration = config;
            return (Logger)LogManager.GetLogger(Constants.SPONGE_LOGGER_NAME);
        }
    }
}
