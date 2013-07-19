using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.SharePoint;
using Sponge.Common.Models;

namespace Sponge.Feature
{
    [Guid("f1542a28-46d4-4d83-bd4e-4854d05bd787")]
    public class EventReceiver : SPFeatureReceiver
    {
        private static string _webName = "Sponge Framework";
        private static string _webUrl = "Sponge";
        private static string _webTemplate = "STS#1";
        private static string _webImageUrl = "_layouts/images/Sponge/logo.png";
        private static List<string> _listNames = new List<string>() { "LogAppenders", "LogConfigs", "Configurations" };

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
                UpdateLists(mgr, delete);
            }
        }

        private void UpdateWeb(SPManager mgr, bool delete)
        {
            if(delete)
            {
                mgr.Webs.Delete(_webUrl);   
            }
            else
            {
                var sponge = mgr.Webs.Create(_webName, "", _webUrl, _webTemplate);
                sponge.SiteLogoUrl = _webImageUrl;
                sponge.Update();
                mgr.ParentWeb = sponge;
            }
        }

        private void UpdateLists(SPManager mgr, bool delete)
        {
            if(delete)
            {
                _listNames.ForEach(i => mgr.Lists.Delete(i));
            }
            else
            {
                foreach(var name in _listNames)
                {
                    var list = mgr.Lists.Create(name, "", SPListTemplateType.GenericList);
                    list.OnQuickLaunch = true;
                    list.Update();
                }
            }
        }
    }
}