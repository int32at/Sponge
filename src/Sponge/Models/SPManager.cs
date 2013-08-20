using System;
using Microsoft.SharePoint;

namespace Sponge.Models
{
    public class SPManager : IDisposable
    {
        public SPSite ParentSite;
        public SPWeb ParentWeb;

        public SPListManager Lists { get; set; }
        public SPWebManager Webs { get; set; }

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

        public SPManager(SPWeb web)
        {
            ParentSite = web.Site;
            ParentWeb = web;
            CreateInstances();
        }

        public SPManager(SPSite site, SPWeb web)
        {
            ParentSite = site;
            ParentWeb = web;
            CreateInstances();
        }

        public void Change(SPSite site)
        {
            Change(site, site.RootWeb);
        }

        public void Change(SPWeb web)
        {
            Change(web.Site, web);
        }

        public void Change(SPSite site, SPWeb web)
        {
            ParentSite = site;
            ParentWeb = web;
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
