using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;

namespace Sponge.Common.Models
{
    public class SPListManager : SPManagerBase
    {
        public SPListManager(SPManager parent) : base(parent) { }

        public SPList Create(string title, string description, SPListTemplate template)
        {
            throw new NotImplementedException();
        }
    }
}
