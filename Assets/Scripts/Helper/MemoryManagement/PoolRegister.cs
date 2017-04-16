using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Matrix
{
    [Serializable]
    public class PoolRegister
    {
        public Pool Pool;
        public int IndexInPool;

        public bool IsLinked
        {
            get
            {
                return Pool != null
                       && IndexInPool != -1;
            }
        }

        public PoolRegister()
        {
            Reset();
        }

        public void Reset()
        {
            Pool = null;
            IndexInPool = -1;
        }
    }
}