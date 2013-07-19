using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sponge.Common.Models
{
    public abstract class SPManagerBase
    {
        public SPManager Parent { get; private set; }
        
        public SPManagerBase(SPManager parent)
        {
            Parent = parent;   
        }
    }
}
