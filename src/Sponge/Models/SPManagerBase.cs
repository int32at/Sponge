using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sponge.Models
{
    public abstract class SPManagerBase
    {
        public SPManager Parent { get; private set; }

        protected SPManagerBase(SPManager parent)
        {
            Parent = parent;   
        }
    }
}
