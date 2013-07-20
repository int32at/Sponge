using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using Microsoft.SharePoint;
using Sponge.Common.Models;

namespace Sponge.Common.Utilities
{
    public static class Utils
    {
        public static SPWeb GetSpongeWeb()
        {
            using (var ca = SPWebManager.GetCentralAdminWeb())
                return ca.Webs[Constants.SPONGE_WEB_URL];
        }

        public static string GetSpongeUrl()
        {
            using (var ca = SPWebManager.GetCentralAdminWeb())
                return ca.Url + "/" + Constants.SPONGE_WEB_URL;
        }

        public static XmlDocument ToXml(Dictionary<string, string> dict)
        {
            XElement el = new XElement("Config",
                dict.Select(kv => new XElement(kv.Key, kv.Value)));
            var doc = new XmlDocument();
            doc.LoadXml(el.ToString());

            return doc;
        }

        public static Dictionary<string, string> FromXml(XmlDocument doc)
        {
            var dict = new Dictionary<string, string>();
            foreach (XmlNode el in doc.ChildNodes)
                dict.Add(el.Name, el.Value);

            return dict;
        }
    }
}
