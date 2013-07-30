using System.Web.Services;
using System.Xml;
using Sponge.Logging;

namespace Sponge.WebService
{
    [WebService(Namespace = "http://Sponge.WebService.ConfigService")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]

    public class LoggingService : System.Web.Services.WebService
    {
        [WebMethod]
        public XmlDocument Get(string loggerName)
        {
            return LogManager.GetConfig(loggerName);
        }
    }
}