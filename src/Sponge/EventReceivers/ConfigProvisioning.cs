using Microsoft.SharePoint;

namespace Sponge.EventReceivers
{
    public class ConfigProvisioning : SPItemEventReceiver
    {
        public override void ItemAdded(SPItemEventProperties properties)
        {
            base.ItemAdded(properties);
        }

        public override void ItemUpdated(SPItemEventProperties properties)
        {
            base.ItemUpdated(properties);
        }

        public override void ItemDeleted(SPItemEventProperties properties)
        {
            base.ItemDeleted(properties);
        }
    }
}