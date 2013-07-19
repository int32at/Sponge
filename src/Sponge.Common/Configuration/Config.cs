using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;

namespace Sponge.Common.Configuration
{
    public class Config : SPPersistedObject
    {
        [Persisted]
        private string clientId;

        public Config(string name,  SPPersistedObject parent)
        {
            
        }

        //public Config(string name, SPPersistedObject parent)
        //    : base(name, parent)
        //{
        //}
    }
}
;