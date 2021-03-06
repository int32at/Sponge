﻿using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using System;

namespace Sponge.Models
{
    public class SPWebManager : SPManagerBase
    {
        public SPWebManager(SPManager parent) : base(parent) { }

        public SPWeb Create(string title, string description, string url, string template)
        {
            if (Exists(url)) 
                throw new Exception(string.Format("SPWeb at {0} already exists", url));

            var web = Parent.ParentWeb.Webs.Add(url, title, description, 1033, template, false, false);
            web.Update();
            return web;
        }

        public bool Exists(string url)
        {
            try
            {
                using (var web = Parent.ParentSite.OpenWeb(url))
                {
                    return web.Exists;
                }
            }
            catch
            {
                return false;
            }
        }

        public void Delete(string url)
        {
            if(Exists(url))
            {
                using (var web = Parent.ParentSite.OpenWeb(url))
                    web.Delete();
            }
        }

        public static SPWeb GetCentralAdminWeb()
        {
            return SPAdministrationWebApplication.Local.Sites[0].RootWeb;
        }
    }
}
