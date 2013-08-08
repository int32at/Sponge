using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
using System.Xml;
using Sponge.Configuration;
using Sponge.Utilities;

namespace Sponge.WebService
{
    [WebService(Namespace = "http://Sponge.WebService.ConfigService")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ScriptService]
    [System.ComponentModel.ToolboxItem(false)]

    public class ConfigService : System.Web.Services.WebService
    {
        [WebMethod]
        public Sponge.Configuration.Configuration GetCentral(string appName)
        {
            return ConfigurationManager.GetOnline(appName);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetCentralJson(string appName)
        {
            var cfg = ConfigurationManager.GetOnline(appName);
            return new JavaScriptSerializer().Serialize(cfg);
        }

        [WebMethod]
        public Sponge.Configuration.Configuration GetRelative(string spongeUrl, string appName)
        {
            return ConfigurationManager.GetOnline(spongeUrl, appName);
        }
    }
}