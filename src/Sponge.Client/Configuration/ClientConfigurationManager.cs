using System.Linq;
using Sponge.Client.ConfigServiceReference;

namespace Sponge.Client.Configuration
{
    public class ClientConfigurationManager
    {
        public static Configuration GetOnline(string spongeUrl, string appName, bool central)
        {
            using (var svc = GetConfigService(spongeUrl))
            {
                var cfg = central ? svc.GetCentral(appName) : svc.GetRelative(spongeUrl, appName);
                return GetConfig(cfg);
            }
        }

        private static Configuration GetConfig(ConfigServiceReference.Configuration cfg)
        {
            var items = (from i in cfg.ConfigurationItems
                         select new ConfigurationItem { Key = i.Key, Value = i.Value }).ToList();

            return new Configuration(cfg.Name, cfg.SpongeUrl, cfg.IsOnline, items);
        }

        private static ConfigService GetConfigService(string url)
        {
            if (!url.EndsWith("/"))
                url = url += "/";

            url = url + "_layouts/Sponge/ConfigService.asmx";

            var svc = new ConfigService();
            svc.Url = url;
            svc.Credentials = System.Net.CredentialCache.DefaultNetworkCredentials;

            return svc;
        }
    }
}
