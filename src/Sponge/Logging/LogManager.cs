using System;
using System.IO;
using System.Xml;
using Microsoft.SharePoint;
using NLog;
using NLog.Config;
using Sponge.Utilities;

namespace Sponge.Logging
{
    public class LogManager
    {
        public static Logger GetOnline(string loggerName)
        {
            using (var reader = GetXml(Utils.GetSpongeUrl(), loggerName))
            {
                Configure(reader);
            }

            return NLog.LogManager.GetLogger(loggerName);
        }

        public static Logger GetOnline(string spongeUrl, string loggerName)
        {
            using (var reader = GetXml(loggerName, spongeUrl))
            {
                Configure(reader);
            }

            return NLog.LogManager.GetLogger(loggerName);
        }

        public static Logger GetOffline()
        {
            return NLog.LogManager.GetLogger(Constants.SPONGE_LOGGER_NAME);
        }

        public static Logger GetOffline(string configPath)
        {
            return GetOffline(configPath, "");
        }

        public static Logger GetOffline(string configPath, Type type)
        {
            var name = type == null ? Constants.SPONGE_LOGGER_NAME : type.FullName;
            return GetOffline(configPath, name);
        }

        public static Logger GetOffline(string configPath, string loggerName)
        {
            if (string.IsNullOrEmpty(loggerName))
                loggerName = Constants.SPONGE_LOGGER_NAME;

            Configure(configPath);
            return NLog.LogManager.GetLogger(loggerName);
        }

        public static XmlDocument GetConfig(string loggerName)
        {
            return GetConfig(Utils.GetSpongeUrl(), loggerName);
        }

        public static XmlDocument GetConfig(string spongeUrl, string loggerName)
        {
            var doc = new XmlDocument();

            SPSecurity.RunWithElevatedPrivileges(() =>
            {
                using (var site = new SPSite(spongeUrl))
                {
                    using (var sponge = site.OpenWeb(Constants.SPONGE_WEB_URL))
                    {
                        var configItems = sponge.Lists[Constants.SPONGE_LIST_LOGCONFIGS];
                        var q = new SPQuery() { Query = GetLoggerNameQuery(loggerName), ViewFields = "<FieldRef Name='Target' /><FieldRef Name='Title' />" };

                        var items = configItems.GetItems(q);

                        if (items.Count == 0)
                            throw new Exception(string.Format("No Logger '{0}' found", loggerName));

                        var item = items[0];

                        var app = Convert.ToInt32(item["Target"].ToString().Split(';')[0]);
                        var target = sponge.Lists[Constants.SPONGE_LIST_LOGTARGETS].GetItemById(app);

                        var xml = target["Xml"].ToString();

                        doc.LoadXml(xml);
                    }
                }
            });

            return doc;
        }

        private static XmlReader GetXml(string spongeUrl, string loggerName)
        {
            XmlReader reader = null;

            var doc = GetConfig(spongeUrl, loggerName);

            using (var builder = new StringReader(doc.OuterXml))
                reader = XmlReader.Create(builder);

            return reader;
        }

        private static string GetLoggerNameQuery(string loggerName)
        {
            return string.Format(@"<Where>
                                        <Eq>
                                            <FieldRef Name='Title' />
                                            <Value Type='Text'>{0}</Value>
                                        </Eq>
                                   </Where>", loggerName);
        }

        private static void Configure(XmlReader reader)
        {
            AddCustomTargets();
            var config = new XmlLoggingConfiguration(reader, null);
            NLog.LogManager.Configuration = config;
        }

        private static void Configure(string path)
        {
            AddCustomTargets();
            var config = new XmlLoggingConfiguration(path);
            NLog.LogManager.Configuration = config;
        }

        private static void Configure()
        {
            AddCustomTargets();
        }

        private static void AddCustomTargets()
        {
            ConfigurationItemFactory.Default.Targets.RegisterDefinition("UlsTarget", typeof(UlsTarget));
        }
    }
}
