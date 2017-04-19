using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Matrix
{
    [Serializable]
    public struct WeightedRoomPattern
    {
        public RoomPattern RoomPattern;
        public float Weight;
    }
}
