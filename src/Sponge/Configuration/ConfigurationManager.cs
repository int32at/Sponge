using Microsoft.SharePoint;
using Sponge.Utilities;
using System;
using System.Linq;

namespace Sponge.Configuration
{
    public class ConfigurationManager
    {
        public static Configuration GetOnline(string appName)
        {
            return GetOnline(Utils.GetSpongeUrl(), appName);
        }

        public static Configuration GetOnline(string spongeUrl, string appName)
        {
            Configuration cfg = null;

            try
            {
                SPSecurity.RunWithElevatedPrivileges(() =>
                {
                    using (var site = new SPSite(spongeUrl))
                    {
                        using (var sponge = site.OpenWeb(Constants.SpongeWebUrl))
                        {
                            var query = new SPQuery { Query = GetAppQueryItems(appName) };

                            var items = sponge.Lists[Constants.SpongeListConfigitems].GetItems(query);

                            if (items.Count == 0)
                                throw new Exception(string.Format("No Entries in Application '{0}' found.", appName));

                            var conf = (from SPListItem item in items
                                        select new ConfigurationItem { Key = item["Title"].ToString(), Value = item["Value"] }).ToList();

                            cfg = new Configuration(appName, sponge.Url, true, conf);
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Error getting online config for app '{0}'. {1}",
                    appName, ex.Message), ex);
            }

            return cfg;
        }

        private static string GetAppQueryItems(string app)
        {
            return string.Format(@"<Where>
                                        <Eq>
                                            <FieldRef Name='Application' />
                                            <Value Type='Lookup'>{0}</Value>
                                        </Eq>
                                   </Where>", app);
        }
    }
}

//        public static object Get(string app, string key)
//        {
//            return Get<object>(app, key);
//        }

//        public static Dictionary<string, string> GetAll(string app)
//        {
//            using (var sponge = Utils.GetSpongeWeb())
//            {
//                var query = new SPQuery() { Query = GetAppQueryItems(app) };

//                var items = sponge.Lists[Constants.SPONGE_LIST_CONFIGITEMS].GetItems(query);

//                if (items.Count == 0)
//                    throw new Exception(string.Format("No Entries in Application '{0}' found.", app));

//                return items.Cast<SPListItem>().ToDictionary(i => i["Title"].ToString(),
//                    i => i["Value"].ToString());
//            }
//        }

//        public static void Set(string app, string key, object value)
//        {
//            using (var sponge = Utils.GetSpongeWeb())
//            {
//                sponge.AllowUnsafeUpdates = true;
//                var query = new SPQuery() { Query = GetAppAndKeyQuery(app, key) };
//                var list = sponge.Lists[Constants.SPONGE_LIST_CONFIGITEMS];
//                var items = list.GetItems(query);

//                SPListItem item = items.Count == 0 ? list.AddItem() : items[0];

//                item["Key"] = key;
//                item["Value"] = value;
//                item["Application"] = GetAppId(app);
//                item.SystemUpdate();
//                sponge.AllowUnsafeUpdates = false;
//            }
//        }

//        public static void CreateApplication(string appName)
//        {
//            using (var sponge = Utils.GetSpongeWeb())
//            {
//                sponge.AllowUnsafeUpdates = true;
//                var item = sponge.Lists[Constants.SPONGE_LIST_CONFIGAPPLICATIONS].AddItem();
//                item["Title"] = appName;
//                item.SystemUpdate();
//                sponge.AllowUnsafeUpdates = false;
//            }
//        }

//        public static bool ApplicationExists(string appName)
//        {
//            using (var sponge = Utils.GetSpongeWeb())
//            {
//                var query = new SPQuery() { Query = GetAppQuery(appName) };

//                var items = sponge.Lists[Constants.SPONGE_LIST_CONFIGAPPLICATIONS].GetItems(query);
//                return items.Count != 0;
//            }
//        }

//        private static string GetAppAndKeyQuery(string app, string key)
//        {
//            return string.Format(@"<Where>
//                                      <And>
//                                         <Eq>
//                                            <FieldRef Name='Application' />
//                                            <Value Type='Lookup'>{0}</Value>
//                                         </Eq>
//                                         <Eq>
//                                            <FieldRef Name='Title' />
//                                            <Value Type='Text'>{1}</Value>
//                                         </Eq>
//                                      </And>
//                                   </Where>", app, key);
//        }

//        private static string GetAppQuery(string app)
//        {
//            return string.Format(@"<Where>
//                                        <Eq>
//                                            <FieldRef Name='Title' />
//                                            <Value Type='Text'>{0}</Value>
//                                        </Eq>
//                                   </Where>", app);
//        }

//        private static string GetAppQueryItems(string app)
//        {
//            return string.Format(@"<Where>
//                                        <Eq>
//                                            <FieldRef Name='Application' />
//                                            <Value Type='Lookup'>{0}</Value>
//                                        </Eq>
//                                   </Where>", app);
//        }

//        private static int GetAppId(string appName)
//        {
//            using (var sponge = Utils.GetSpongeWeb())
//            {
//                var query = new SPQuery()
//                {
//                    Query = string.Format(@"<Where><Eq><FieldRef Name='Title' /><Value Type='Text'>{0}</Value></Eq></Where>", appName)
//                };

//                var items = sponge.Lists[Constants.SPONGE_LIST_CONFIGAPPLICATIONS].GetItems(query);

//                if (items.Count == 0)
//                    throw new Exception(string.Format("No Entries in Application '{0}' found.", appName));

//                return items[0].ID;
//            }
//        }
