using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Matrix
{
    public abstract class RoomPattern : MonoBehaviour
    {
        public abstract void CreatePools();

        public abstract void Fill(Room room);
    }
}
