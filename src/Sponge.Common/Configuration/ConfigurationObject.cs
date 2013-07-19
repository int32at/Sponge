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
        private Dictionary<string, ConfigurationItemCollection> _configItems = new Dictionary<string, ConfigurationItemCollection>();


        public Dictionary<string, ConfigurationItemCollection> Items
        {
            get { return _configItems; }
            set { _configItems = value; }
        }

        public ConfigurationObject()
        {
        }

        protected override bool HasAdditionalUpdateAccess()
        {
            return true; //return base.HasAdditionalUpdateAccess();
        }

        private ConfigurationObject(SPPersistedObject parent)
            : base(CONFIG_NAME, parent)
        {
        }
    }

    internal class ConfigurationItemCollection : SPAutoSerializingObject
    {
        [Persisted]
        private Dictionary<string, object> _configItems = new Dictionary<string, object>();

        public Dictionary<string, object> Items
        {
            get { return _configItems; }
            set { _configItems = value; }
        }

    }

    internal class ConfigurationItem : SPAutoSerializingObject
    {
        [Persisted]
        private string key;

        [Persisted]
        private object val;

        public string Key { get { return key; } set { key = value; } }
        public object Value { get { return val; } set { val = value; } }
    }
}