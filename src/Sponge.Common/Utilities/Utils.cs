using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;

namespace Sponge.Common.Utilities
{
    public static class Utils
    {
        public static SPWeb GetCentralAdminRootWeb()
        {
            return SPAdministrationWebApplication.Local.Sites[0].RootWeb;
        }
    }
}
