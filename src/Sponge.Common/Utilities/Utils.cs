using Microsoft.SharePoint;
using Sponge.Common.Models;
using Sponge.Common.Context;

namespace Sponge.Common.Utilities
{
    public static class Utils
    {
        private static SpongeContextDataContext _ctx;
        public static SpongeContextDataContext Context
        {
            get
            {
                if (_ctx == null)
                    _ctx = new SpongeContextDataContext(GetSpongeUrl());

                return _ctx;
            }
        }
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
    }
}
