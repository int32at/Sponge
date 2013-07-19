using System;
using System.Runtime.InteropServices;
using Microsoft.SharePoint;
using Sponge.Common.Models;

namespace Sponge.Feature
{
    [Guid("f1542a28-46d4-4d83-bd4e-4854d05bd787")]
    public class EventReceiver : SPFeatureReceiver
    {
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            var ca = SPWebManager.GetCentralAdminWeb();
            using (var mgr = new SPManager(ca.Site))
            {
                var sponge = mgr.Webs.Create("Sponge Framework", "", "Sponge", "STS#1");
                sponge.SiteLogoUrl = "_layouts/images/Sponge/logo.png";
                sponge.Update();
            }
        }

        public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
        {
            var ca = SPWebManager.GetCentralAdminWeb();
            using (var mgr = new SPManager(ca.Site))
            {
                mgr.Webs.Delete("Sponge");
            }
        }
    }
}