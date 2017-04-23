using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Matrix
{
    public class WeightedRoom
    {
        public Room Room;
        public float Weight;

        public WeightedRoom(Room room, float weight)
        {
            Room = room;
            Weight = weight;
        }

        public override string ToString()
        {
            return "WeightedRoom (" + Room + ", " + Weight + ")";
        }
    }
}
