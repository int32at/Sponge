using System;
using System.Runtime.InteropServices;
using Microsoft.SharePoint;
using Sponge.Utilities;

namespace Sponge.Features.Sponge.Site.LogConfig
{
    [Guid("6ed29b67-1b28-4018-8b20-b57ad3aace29")]
    public class SpongeSiteEventReceiver : SPFeatureReceiver
    {
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            Setup.Install((properties.Feature.Parent as SPSite).RootWeb);
        }

        public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
        {
            Setup.Uninstall((properties.Feature.Parent as SPSite).RootWeb);
        }
    }
}
