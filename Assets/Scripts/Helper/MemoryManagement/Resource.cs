using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Matrix
{
    [Serializable]
    public class Resource
    {
        public PoolObject PoolObject;
        public bool IsUsed;

        public Resource(PoolObject poolObject)
        {
            this.PoolObject = poolObject;
            IsUsed = false;
        }
    }
}
