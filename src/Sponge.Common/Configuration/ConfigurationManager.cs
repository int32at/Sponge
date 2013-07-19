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
            object obj = null;

            SPSecurity.RunWithElevatedPrivileges(() =>
            {
                var conf =  ConfigurationObject.Local.Foo;
                obj = conf;
            });
            return (T)obj;
        }

        public static void Set(string app, string key, object value)
        {
            SPSecurity.RunWithElevatedPrivileges(() =>
            {
                ConfigurationObject.Local.Foo = value;
                ConfigurationObject.Local.Update();
            });

        }
    }
}
