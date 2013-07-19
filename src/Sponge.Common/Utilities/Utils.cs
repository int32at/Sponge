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
    }
}
