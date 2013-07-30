using System;
using System.Runtime.InteropServices;
using Microsoft.SharePoint;
using Sponge.Models;
using Sponge.Utilities;

namespace Sponge.Features.Sponge
{
    [Guid("8b705cc1-c636-48d9-b5e6-7a3c9c83f9b6")]
    public class SpongeEventReceiver : SPFeatureReceiver
    {
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            using (var ca = SPWebManager.GetCentralAdminWeb())
            {
                Setup.Install(ca.Site);
            }
        }

        public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
        {
            using (var ca = SPWebManager.GetCentralAdminWeb())
            {
                Setup.Uninstall(ca.Site);
            }
        }
    }
}
