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

        //public GameObject GameObject;

        //public bool IsFilled
        //{
        //    get { return GameObject != null; }
        //}

        public PoolObject()
        {
            //GameObject = null;

            Pool = null;
            IndexInPool = -1;
        }

        //public void Reset()
        //{
        //    if (IsFilled)
        //    {
        //        Release();
        //    }

        //    Pool = null;
        //    IndexInPool = -1;
        //}

        //public virtual void Instantiate()
        //{
        //    if (IsFilled)
        //    {
        //        Release();
        //    }

        //    if (Pool != null)
        //    {
        //        Pool.GetFreeResource(this);
        //    }
        //}

        public virtual void Release()
        {
            if (Pool  != null)
            {
                 Pool.ReleaseResource(this);
            }

            //GameObject = null;
        }

        public override string ToString()
        {
            return name + " ( pool : " + Pool.name + " )";
        }
    }
}