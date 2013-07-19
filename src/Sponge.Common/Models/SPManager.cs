using System;
using Microsoft.SharePoint;

namespace Sponge.Common.Models
{
    public class SPManager : IDisposable
    {
        public SPSite ParentSite;
        public SPWeb ParentWeb;

        public SPListManager Lists { get; private set; }
        public SPWebManager Webs { get; private set; }

        public SPManager(string spSiteUrl)
        {
            ParentSite = new SPSite(spSiteUrl);
            ParentWeb = ParentSite.RootWeb;
            CreateInstances();            
        }

        public SPManager(string spSiteUrl, string spwebUrl)
        {
            ParentSite = new SPSite(spSiteUrl);
            ParentWeb = ParentSite.OpenWeb(spwebUrl);
            CreateInstances();
        }

        public SPManager(SPSite site)
        {
            ParentSite = site;
            ParentWeb = site.RootWeb;
            CreateInstances();
        }

        public SPManager(SPSite site, SPWeb web)
        {
            ParentSite = site;
            ParentWeb = web;
            CreateInstances();
        }

        private void CreateInstances()
        {
            Lists = new SPListManager(this);
            Webs = new SPWebManager(this);
        }

        public void Dispose()
        {
            if (ParentSite != null)
                ParentSite.Dispose();

            if (ParentWeb != null)
                ParentWeb.Dispose();
        }
    }
}
