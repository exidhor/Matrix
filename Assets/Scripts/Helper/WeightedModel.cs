using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Matrix
{
    [Serializable]
    public class WeightedModel
    {
        public float Weight;
        public PoolEntry Model;
    }
}
