using System;
using Microsoft.SharePoint;

namespace Sponge.Common.Models
{
    public class SPManager : IDisposable
    {
        private SPSite _site;
        private SPWeb _web;

        public SPManager(string spSiteUrl)
        {
            _site = new SPSite(spSiteUrl);
            _web = _site.RootWeb;
        }
        public SPManager(string spSiteUrl, string spwebUrl)
        {
            _site = new SPSite(spSiteUrl);
            _web = _site.OpenWeb(spwebUrl); ;
        }

        public void Dispose()
        {
            if (_site != null)
                _site.Dispose();

            if (_web != null)
                _web.Dispose();
        }
    }
}
