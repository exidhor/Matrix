using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

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
            if (direction >= 0 && (int) direction < 4)
            {
                return _connectionList[GetIndex(direction)];
            }

            return null;
        }

        public Room GetConnectedRoom(Direction direction)
        {
            RoomConnection roomConnection = GetConnection(direction);

            if (roomConnection != null)
            {
                return roomConnection.GetOtherRoom(_parent);
            }

            return null;
        }

        public Direction GetDirection(RoomConnection roomConnection)
        {
            for (int i = 0; i < _connectionList.Count; i++)
            {
                if (_connectionList[i] == roomConnection)
                {
                    return (Direction) i;
                }
            }

            Debug.LogError("RoomConnection not found");

            return Direction.Left;
        }

        private int GetIndex(Direction direction)
        {
            return (int) direction;
        }
    }
}