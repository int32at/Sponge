using System.Web.Services;
using System.Xml;
using Sponge.Common.Configuration;
using Sponge.Common.Utilities;

namespace Sponge.WebService
{
    [WebService(Namespace = "http://Sponge.WebService.ConfigService")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]

    public class ConfigService : System.Web.Services.WebService
    {
        [WebMethod]
        public string Get(string appName, string key)
        {
            return ConfigurationManager.Get<string>(appName, key);
        }

        [WebMethod]
        public XmlDocument GetAll(string appName)
        {
            return Utils.ToXml(ConfigurationManager.GetAll(appName));
        }

        [WebMethod]
        public void CreateApplication(string appName)
        {
            ConfigurationManager.CreateApplication(appName);
        }

        [WebMethod]
        public bool ApplicationExists(string appName)
        {
            return ConfigurationManager.ApplicationExists(appName);
        }

        [WebMethod]
        public void Set(string appName, string key, string value)
        {
            ConfigurationManager.Set(appName, key, value);
        }
    }
}