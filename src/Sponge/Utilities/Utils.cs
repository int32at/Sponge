using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Sponge.Models;

namespace Sponge.Utilities
{
    public static class Utils
    {
        public static SPWeb GetSpongeWeb()
        {
            using (var ca = SPWebManager.GetCentralAdminWeb())
                return ca.Webs[Constants.SpongeWebUrl];
        }

        public static SPManager GetSpongeWebManager()
        {
            return new SPManager(GetSpongeWeb());
        }

        public static string GetSpongeUrl()
        {
            using (var ca = SPWebManager.GetCentralAdminWeb())
                return ca.Url + "/" + Constants.SpongeWebUrl;
        }

        public static XmlDocument ToXml(Dictionary<string, string> dict)
        {
            var el = new XElement("Config",
                dict.Select(kv => new XElement(kv.Key, kv.Value)));
            var doc = new XmlDocument();
            doc.LoadXml(el.ToString());

            return doc;
        }

        public static Dictionary<string, string> FromXml(XmlDocument doc)
        {
            return doc.ChildNodes.Cast<XmlNode>().ToDictionary(el => el.Name, el => el.Value);
        }

        public static IEnumerable<string> GetAvailableServerNames()
        {
            return SPFarm.Local.Servers.Select(s => s.Name);
        }

        public static string GetSPDiagnosticsLocation()
        {
            return SPDiagnosticsService.Local.LogLocation;
        }
    }
}
