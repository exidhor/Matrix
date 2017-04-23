using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Matrix
{
    public class WeightedRoomList
    {
        public int Count
        {
            get { return _weightedRooms.Count; }
        }

        public float GlobalWeight
        {
            get { return _globalWeight; }
        }

        private List<WeightedRoom> _weightedRooms;
        private float _globalWeight;
        private LevelEntry _entry;

        public WeightedRoomList()
        {
            _weightedRooms = new List<WeightedRoom>();
        }

        public void Reset(LevelEntry entry)
        {
            _entry = entry;

            _weightedRooms.Clear();
            _globalWeight = 0;
        }

        public void Add(Room room)
        {
            _weightedRooms.Add(new WeightedRoom(room, 1));

            _globalWeight += 1;
        }

        public Room GetRoom(int index)
        {
            return _weightedRooms[index].Room;
        }

        public float GetWeight(int index)
        {
            return _weightedRooms[index].Weight;
        }

        public void Connect(int index, Direction direction, RoomConnection roomConnection)
        {
            _weightedRooms[index].Room.RoomConnectionList.Add(roomConnection, direction);

            ActualizeWeight(index);
        }

        private void ActualizeWeight(int index)
        {
            float currentWeight = _weightedRooms[index].Weight;

            int numberOfConnections = _weightedRooms[index].Room.RoomConnectionList.ActiveConnectionCount;
            float newWeight = _entry.ConnectionRates[numberOfConnections];

            float difference = newWeight - currentWeight;

            _weightedRooms[index].Weight = newWeight;

            _globalWeight += difference;
        }
    }
}