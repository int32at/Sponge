using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;

namespace Sponge.Common.Configuration
{
    internal class ConfigurationObject : SPPersistedObject
    {
        private const string CONFIG_NAME = "SpongeConfig";

        public static ConfigurationObject Local
        {
            get
            {
                var parent = SPFarm.Local;
                var obj = parent.GetChild<ConfigurationObject>(CONFIG_NAME);
                if (obj == null)
                {
                    obj = new ConfigurationObject(parent);
                    obj.Update();
                }

                return obj;
            }
        }

        [Persisted]
        private Dictionary<string, ConfigurationItem> _items = new Dictionary<string, ConfigurationItem>();
        public Dictionary<string, ConfigurationItem> Items
        {
            get { return _items; }
            set { _items = value; }
        }

        public ConfigurationObject()
        {
        }

        protected override bool HasAdditionalUpdateAccess()
        {
            return true;
        }

        private ConfigurationObject(SPPersistedObject parent)
            : base(CONFIG_NAME, parent)
        {
        }
    }

    internal class ConfigurationItem : SPAutoSerializingObject
    {
        [Persisted]
        private Dictionary<string, object>_items = new Dictionary<string, object>();
        public Dictionary<string, object> Items
        {
            get { return _items; }
            set { _items = value; }
        }
    }
}