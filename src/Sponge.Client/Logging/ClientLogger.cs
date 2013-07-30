using System;
using System.IO;
using NLog;
using NLog.Config;
using Sponge.Client.Utilities;
using Sponge.Client.LoggingServiceReference;
using System.Xml;

namespace Sponge.Client.Logging
{
    public class ClientLogManager
    {
        public static Logger GetOnline(string webUrl, string loggerName)
        {
            using (var reader = GetXml(webUrl, loggerName))
            {
                var config = new XmlLoggingConfiguration(reader, null);
                NLog.LogManager.Configuration = config;
            }

            return NLog.LogManager.GetLogger(loggerName);
        }

        public static Logger GetOffline(string configPath, Type type)
        {
            var name = type == null ? Constants.SPONGE_LOGGER_NAME : type.FullName;

            var config = new XmlLoggingConfiguration(configPath);
            NLog.LogManager.Configuration = config;
            return NLog.LogManager.GetLogger(name);
        }

        public static Logger GetOffline(string configPath)
        {
            return GetOffline(configPath, null);
        }

        public static Logger GetOffline()
        {
            return NLog.LogManager.GetLogger(Constants.SPONGE_LOGGER_NAME);
        }

        private static XmlReader GetXml(string webUrl, string loggerName)
        {
            XmlReader reader = null;
            using (var svc = GetLoggingService(webUrl))
            {
                var result = svc.Get(loggerName);

                if (result == null)
                    throw new Exception(string.Format("Invalid Result for Logger '{0}'", loggerName));

                using (var data = new StringReader(result.OuterXml))
                    reader = XmlReader.Create(data);
            }

            return reader;
        }

        public static LoggingService GetLoggingService(string url)
        {
            if (!url.EndsWith("/"))
                url = url += "/";

            url = url + "_layouts/Sponge/LoggingService.asmx";

            var svc = new LoggingService();
            svc.Url = url;
            svc.Credentials = System.Net.CredentialCache.DefaultNetworkCredentials;

            return svc;
        }
    }
}