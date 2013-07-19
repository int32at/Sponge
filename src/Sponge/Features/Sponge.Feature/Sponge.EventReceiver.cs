using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.SharePoint;
using Sponge.Common.Models;
using Sponge.Common.Utilities;

namespace Sponge.Feature
{
    [Guid("f1542a28-46d4-4d83-bd4e-4854d05bd787")]
    public class EventReceiver : SPFeatureReceiver
    {
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            Update(false);
        }

        public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
        {
            Update(true);
        }

        private void Update(bool delete)
        {
            var ca = SPWebManager.GetCentralAdminWeb();
            using (var mgr = new SPManager(ca.Site))
            {
                UpdateWeb(mgr, delete);
                UpdateLists(mgr);
            }
        }

        private void UpdateWeb(SPManager mgr, bool delete)
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

        private void UpdateLists(SPManager mgr)
        {
            CreateConfigApplications(mgr);
            CreateConfigItems(mgr);
            CreateLogAppenders(mgr);
            CreateLogConfigs(mgr);
        }

        private void CreateConfigItems(SPManager mgr)
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

        private void CreateConfigApplications(SPManager mgr)
        {
            var list = CreateList(mgr, Constants.SPONGE_LIST_CONFIGAPPLICATIONS);
        }

        private void CreateLogAppenders(SPManager mgr)
        {
            CreateList(mgr, Constants.SPONGE_LIST_LOGAPPENDERS);
        }

        private void CreateLogConfigs(SPManager mgr)
        {
            CreateList(mgr, Constants.SPONGE_LIST_LOGCONFIGS);
        }

        private SPList CreateList(SPManager mgr, string listName)
        {
            var list = mgr.Lists.Create(listName, "", SPListTemplateType.GenericList);
            list.OnQuickLaunch = true;
            list.Update();

            return list;
        }
    }
}