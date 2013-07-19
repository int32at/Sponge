using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using Sponge.Common.Utilities;
using Sponge.Common.Context;

namespace Sponge.Common.Configuration
{
    public class ConfigurationManager
    {
        public static T Get<T>(string app, string key)
        {
            object result = null;

            SPSecurity.RunWithElevatedPrivileges(() =>
            {
                var query = from item in Utils.Context.ConfigItems
                            where item.Application.Title == app &&
                                  item.Title == key
                            select item.Value;

                var conf = query.FirstOrDefault();

                if (conf == null)
                    throw new Exception(string.Format("No Entry for Key '{0}' in Application '{1}' found.", key, app));

                result = conf;
            });

            return (T)result;
        }

        public static object Get(string app, string key)
        {
            return Get<object>(app, key);
        }

        public static Dictionary<string, string> GetAll(string app)
        {
            var dict = new Dictionary<string, string>();

            SPSecurity.RunWithElevatedPrivileges(() =>
            {
                var query = from item in Utils.Context.ConfigItems
                            where item.Application.Title == app
                            select new { Key = item.Title, Value = item.Value };

                dict = query.ToDictionary(i => i.Key, i => i.Value);
            });

            return dict;
        }

        public static void Set(string app, string key, object value)
        {
            SPSecurity.RunWithElevatedPrivileges(() =>
            {
                var query = from item in Utils.Context.ConfigItems
                            where item.Application.Title == app &&
                                  item.Title == key
                            select item;

                var conf = query.FirstOrDefault();

                if (conf == null)
                {
                    var appItem = Utils.Context.ConfigApplications.Where(i => i.Title == app).FirstOrDefault();

                    if (appItem == null)
                        throw new Exception(string.Format("Application '{0}' not found"));

                    var item = new ConfigItemsItem()
                    {
                        Application = appItem,
                        Title = key,
                        Value = value.ToString()
                    };

                    Utils.Context.ConfigItems.InsertOnSubmit(item);
                }
                else
                {
                    conf.Value = value.ToString();
                }

                Utils.Context.SubmitChanges();
            });
        }
        
        public static void CreateApplication(string appName)
        {
            SPSecurity.RunWithElevatedPrivileges(() =>
            {
                if (ApplicationExists(appName))
                    throw new Exception(string.Format("Application '{0}' already exists.", appName));

                var app = new Item() { Title = appName };
                Utils.Context.ConfigApplications.InsertOnSubmit(app);
                Utils.Context.SubmitChanges();
            });
        }

        public static bool ApplicationExists(string appName)
        {
            var result = false;

            SPSecurity.RunWithElevatedPrivileges(() =>
            {
                result = Utils.Context.ConfigApplications.Where(i => i.Title == appName).FirstOrDefault() != null;
            });

            return result;
        }
    }
}
