using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebPartPages;
using Sponge.Server.Models;

namespace Sponge.Server.Utilities
{
    public static class Setup
    {
        public static void Install(SPSite site)
        {
            Update(site, false);
        }

        public static void Uninstall(SPSite site)
        {
            Update(site, true);
        }

        private static void Update(SPSite site, bool delete)
        {
            using (var mgr = new SPManager(site))
            {
                UpdateWeb(mgr, delete);
                UpdateLists(mgr);
            }
        }
        
        private static void UpdateWeb(SPManager mgr, bool delete)
        {
            if (delete)
            {
                mgr.Webs.Delete(Constants.SPONGE_WEB_URL);
            }
            else
            {
                var sponge = mgr.Webs.Create(Constants.SPONGE_WEB_NAME, "",
                    Constants.SPONGE_WEB_URL, Constants.SPONGE_WEB_TEMPLATE);
                sponge.SiteLogoUrl = Constants.SPONGE_WEB_IMGURL;
                sponge.Update();
                mgr.ParentWeb = sponge;
            }
        }

        private static void UpdateLists(SPManager mgr)
        {
            CreateConfigApplications(mgr);
            CreateConfigItems(mgr);
            CreateLogAppenders(mgr);
            CreateLogConfigs(mgr);
            CreateWebParts(mgr);
            AddDefaultItems(mgr);
        }

        private static void CreateConfigItems(SPManager mgr)
        {
            var list = CreateList(mgr, Constants.SPONGE_LIST_CONFIGITEMS);

            var targetList = list.ParentWeb.Lists[Constants.SPONGE_LIST_CONFIGAPPLICATIONS];

            list.Fields.Add("Value", SPFieldType.Note, true);

            list.Fields.AddLookup("Application", targetList.ID, false);
            SPFieldLookup lkp = (SPFieldLookup)list.Fields["Application"];
            lkp.LookupField = targetList.Fields["Title"].InternalName;
            lkp.Required = true;
            lkp.Update();

            if (list.ContentTypes.Count > 0)
            {
                var ct = list.ContentTypes[0];
                ct.FieldLinks.Reorder(new string[] { "Application", "Key", "Value" });
            }

            var title = list.Fields["Title"];
            title.Title = "Key";
            title.Description = "Do not use blanks or special characters here!";
            title.Update();

            SPView view = list.DefaultView;
            var group = @" <GroupBy Collapse=""TRUE"" GroupLimit=""100""> <FieldRef Name=""Application"" Ascending=""True""/> </GroupBy>";
            view.Query = group;

            view.ViewFields.DeleteAll();
            view.ViewFields.Add("Application");
            view.ViewFields.Add("Key");
            view.ViewFields.Add("Value");
            view.Update();
        }

        private static void CreateConfigApplications(SPManager mgr)
        {
            var list = CreateList(mgr, Constants.SPONGE_LIST_CONFIGAPPLICATIONS);
        }

        private static void CreateLogAppenders(SPManager mgr)
        {
            var list = CreateList(mgr, Constants.SPONGE_LIST_LOGAPPENDERS);

            list.Fields.Add("Xml", SPFieldType.Note, true);
            SPView view = list.DefaultView;
            view.ViewFields.DeleteAll();
            view.ViewFields.Add("Title");
            view.ViewFields.Add("Xml");
            view.Update();
            list.Update();
        }

        private static void CreateLogConfigs(SPManager mgr)
        {
            var list = CreateList(mgr, Constants.SPONGE_LIST_LOGCONFIGS);

            var targetList = list.ParentWeb.Lists[Constants.SPONGE_LIST_LOGAPPENDERS];

            list.Fields.AddLookup("Appender", targetList.ID, false);
            SPFieldLookup lkp = (SPFieldLookup)list.Fields["Appender"];
            lkp.LookupField = targetList.Fields["Title"].InternalName;
            lkp.Required = true;
            lkp.Update();

            SPView view = list.DefaultView;
            var group = @" <GroupBy Collapse=""TRUE"" GroupLimit=""100""> <FieldRef Name=""Appender"" Ascending=""True""/> </GroupBy>";
            view.Query = group;

            view.ViewFields.DeleteAll();
            view.ViewFields.Add("Title");
            view.ViewFields.Add("Appender");
            view.Update();
            list.Update();
        }

        private static SPList CreateList(SPManager mgr, string listName)
        {
            var list = mgr.Lists.Create(listName, "", SPListTemplateType.GenericList);
            list.OnQuickLaunch = true;
            list.Update();

            return list;
        }

        private static void CreateWebParts(SPManager mgr)
        {
            var configItems = new XsltListViewWebPart();
            configItems.ListId = mgr.ParentWeb.Lists[Constants.SPONGE_LIST_CONFIGITEMS].ID;

            var configApps = new XsltListViewWebPart();
            configApps.ListId = mgr.ParentWeb.Lists[Constants.SPONGE_LIST_CONFIGAPPLICATIONS].ID;

            var logConfig = new XsltListViewWebPart();
            logConfig.ListId = mgr.ParentWeb.Lists[Constants.SPONGE_LIST_LOGCONFIGS].ID;

            var logAppenders = new XsltListViewWebPart();
            logAppenders.ListId = mgr.ParentWeb.Lists[Constants.SPONGE_LIST_LOGAPPENDERS].ID;

            AddWebPart(mgr.ParentWeb, "default.aspx", configApps, "left", 1);
            AddWebPart(mgr.ParentWeb, "default.aspx", configItems, "left", 2);
            AddWebPart(mgr.ParentWeb, "default.aspx", logConfig, "right", 1);
            AddWebPart(mgr.ParentWeb, "default.aspx", logAppenders, "right", 2);
        }

        private static void AddWebPart(SPWeb web, string pageURL, System.Web.UI.WebControls.WebParts.WebPart webPart, string zoneID, int zoneIndex)
        {
            SPLimitedWebPartManager webPartManager = web.GetLimitedWebPartManager(pageURL, PersonalizationScope.Shared);
            webPartManager.AddWebPart(webPart, zoneID, zoneIndex);
            webPartManager.SaveChanges(webPart);
            web.Update();
        }

        private static void AddDefaultItems(SPManager mgr)
        {
            var logAppender = mgr.ParentWeb.Lists[Constants.SPONGE_LIST_LOGAPPENDERS];

            var newLogApp = logAppender.AddItem();
            newLogApp["Title"] = "Sponge ULS Logging";
            newLogApp["Xml"] = "XML";
            newLogApp.SystemUpdate();
        }
    }
}
