using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Matrix
{
    [Serializable]
    public class RoomConnectionList
    {
        public int ActiveConnectionCount
        {
            get
            {
                int activeConnectionCount = 0;

                for (int i = 0; i < _connectionList.Count; i++)
                {
                    if (_connectionList[i] != null)
                    {
                        activeConnectionCount++;
                    }
                }

                return activeConnectionCount;
            }
        }

        public int DisableConnectionCount
        {
            get { return _connectionList.Count - ActiveConnectionCount; }
        }

        private List<RoomConnection> _connectionList;
        private Room _parent;

        public RoomConnectionList(Room parent)
        {
            _parent = parent;

            ConstructConnectionList();
        }

        private void ConstructConnectionList()
        {
            _connectionList = new List<RoomConnection>();

            for (int i = 0; i < 4; i++)
            {
                _connectionList.Add(null);
            }
        }

        public void Reset()
        {
            for (int i = 0; i < 4; i++)
            {
                _connectionList[i] = null;
            }
        }

        public void Add(RoomConnection roomConnection, Direction direction)
        {
            _connectionList[GetIndex(direction)] = roomConnection;
        }

        public RoomConnection GetConnection(Direction direction)
        {
            return _connectionList[GetIndex(direction)];
        }

        public Room GetConnectedRoom(Direction direction)
        {
            return GetConnection(direction).GetOtherRoom(_parent);
        }

        private int GetIndex(Direction direction)
        {
            return (int) direction;
        }
    }
}