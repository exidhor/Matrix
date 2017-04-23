using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Matrix
{
    [Serializable]
    public class RoomConnection
    {
        public Room Room0;
        public Door Door0;

        public Room Room1;
        public Door Door1;

        public RoomConnection(Room room0, Room room1)
        {
            Room0 = room0;
            Room1 = room1;
        }

        public void SetDoor(Door door, Room room)
        {
            if (room == Room0)
            {
                Door0 = door;
            }
            else
            {
                Door1 = door;
            }
        }

        public Room GetOtherRoom(Room room)
        {
            if (room != Room0)
            {
                return Room0;
            }

            return Room1;
        }

        public Door GetDoorFrom(Room room)
        {
            if (room == Room0)
            {
                return Door0;
            }

            return Door1;
        }

        public static Direction GetOppositeDirection(Direction direction)
        {
            return (Direction) (((int) direction + 2)%4);
        }
    }
}