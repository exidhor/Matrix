using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Matrix
{
    public class LevelEntry : MonoBehaviour
    {
        public int MinRoomLength;
        public int MaxRoomLength;

        public int MinRoomWidth;
        public int MaxRoomWidth;

        public int MinRoomCount;
        public int MaxRoomCount;

        public float DistanceBetweenRooms;

        public List<float> ConnectionRates;

        public List<WeightedRoomPattern> WeightedRoomPatterns;
    }
}
