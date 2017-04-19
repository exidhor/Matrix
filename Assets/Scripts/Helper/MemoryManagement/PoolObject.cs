using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Matrix
{
    public class PoolObject : MonoBehaviour
    {
        public Pool Pool;
        public int IndexInPool;

        void Awake()
        {
            Pool = null;
            IndexInPool = -1;
        }

        public virtual void OnPoolExit()
        {
            // nothing
        }

        public virtual void OnPoolEnter()
        {
            // nothing
        }

        public virtual void Release()
        {
            if (Pool  != null)
            {
                 Pool.ReleaseResource(this);
            }
        }

        public override string ToString()
        {
            return name + " ( pool : " + Pool.name + " )";
        }
    }
}