using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Matrix
{
    public abstract class DynamicController : PoolObject
    {
        public abstract void Disable();

        public abstract void Activate();
    }
}
