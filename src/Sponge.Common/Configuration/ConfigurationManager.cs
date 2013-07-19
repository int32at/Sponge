using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;

namespace Sponge.Common.Configuration
{
    public class ConfigurationManager
    {
        public static T Get<T>(string app, string key)
        {
            var conf = ConfigurationObject.Local;

            if (!conf.Items.ContainsKey(app))
                throw new Exception(string.Format("Application '{0}' not found.", app));

            if (!conf.Items[app].Items.ContainsKey(key))
                throw new Exception(string.Format("Key '{0}' not found.", key));

            return (T)conf.Items[app].Items[key];
        }

        public static void Set(string app, string key, object value)
        {
            var conf = ConfigurationObject.Local;

            if (conf.Items.ContainsKey(app))
            {
                if (conf.Items[app].Items.ContainsKey(key))
                    conf.Items[app].Items[key] = value;
                else
                    conf.Items[app].Items.Add(key, value);
            }
            else
            {
                var item = new ConfigurationItem();
                item.Items.Add(key, value);
                conf.Items.Add(app, item);
            }

            conf.Update();
        }
    }
}
