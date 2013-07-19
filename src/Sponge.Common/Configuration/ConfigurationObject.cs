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
        private object foo;

        public object Foo { get { return foo; } set { foo = value; } }

        public ConfigurationObject()
        {
        }

        private ConfigurationObject(SPPersistedObject parent)
            : base(CONFIG_NAME, parent)
        {
        }
    }
}