using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;

namespace Sponge.Models
{
    public class SPListManager : SPManagerBase
    {
        public SPListManager(SPManager parent) : base(parent) { }

        public SPList Create(string title, string description, SPListTemplateType template)
        {
            if (!Exists(title))
            {
                var guid = Parent.ParentWeb.Lists.Add(title, description, template);
                return Parent.ParentWeb.Lists[guid];
            }
            else
                throw new Exception(string.Format("List with Name '{0}' already exists", title));
        }

        public bool Exists(string internalName)
        {
            try
            {
                var tmp = Parent.ParentWeb.Lists[internalName];
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool Exists(SPWeb web, string internalName)
        {
            try
            {
                var tmp = web.Lists[internalName];
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void Delete(string internalName)
        {
            if(Exists(internalName))
            {
                Parent.ParentWeb.Lists[internalName].Delete();
            }
        }
    }
}
