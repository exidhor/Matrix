using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Matrix
{
    public class LevelCreator : MonoSingleton<LevelCreator>
    {
        public PoolEntry PoolEntryForRoom;

        private Pool _poolRoom;

        private WeightedRoomList _weightedRoomList;

        void Awake()
        {
            _poolRoom = PoolTable.Instance.AddPool(PoolEntryForRoom);

            _weightedRoomList = new WeightedRoomList();
        }

        public void CreateLevel(LevelEntry entry)
        {
            _weightedRoomList.Reset(entry);

            int roomCount = RandomGenerator.Instance.NextInt(entry.MinRoomCount, entry.MaxRoomCount + 1);

            // The startRoom
            _weightedRoomList.Add(GetNewRoom(0));

            for (int i = 1; i < roomCount; i++)
            {
                int randomIndex = GetRandomRoomIndexByWeight();

                _weightedRoomList.Add(GetNewRoom(i));

                Direction randomDirection = GetRandomFreeConnection(randomIndex);

                RoomConnection roomConnection = new RoomConnection(_weightedRoomList.GetRoom(randomIndex), _weightedRoomList.GetRoom(i));

                _weightedRoomList.Connect(randomIndex, randomDirection, roomConnection);

                _weightedRoomList.Connect(i, RoomConnection.GetOppositeDirection(randomDirection), roomConnection);
            }

            List<Room> roomList = new List<Room>();

            for (int i = 0; i < _weightedRoomList.Count; i++)
            {
                roomList.Add(_weightedRoomList.GetRoom(i));
            }

            Level.Instance.SetRoomList(roomList);
        }

        private int GetRandomRoomIndexByWeight()
        {
            float weight = RandomGenerator.Instance.NextFloat(_weightedRoomList.GlobalWeight);

            float currentWeight = 0;

            for (int i = 0; i < _weightedRoomList.Count; i++)
            {
                currentWeight += _weightedRoomList.GetWeight(i);

                if (currentWeight <= weight)
                {
                    return i;
                }
            }

            return _weightedRoomList.Count - 1;
        }

        public Direction GetRandomFreeConnection(int roomIndex)
        {
            RoomConnectionList roomConnectionList = _weightedRoomList.GetRoom(roomIndex).RoomConnectionList;

            int freeConnectionCount = roomConnectionList.DisableConnectionCount;

            int randomDirectionIndex = RandomGenerator.Instance.NextInt(freeConnectionCount);

            int freeIndex = 0;

            for (int i = 0; i < 4; i++)
            {
                RoomConnection roomConnection = roomConnectionList.GetConnection((Direction) i);

                if (roomConnection == null)
                {
                    if (freeIndex == randomDirectionIndex)
                    {
                        return (Direction) freeIndex;
                    }

                    freeIndex++;
                }
            }

            Debug.Log("Error in GetRandomFreeConnection, no free connection found");
            return Direction.Bottom;
        }

        private Room GetNewRoom(int index)
        {
            Room newRoom = (Room) _poolRoom.GetFreeResource();
            newRoom.RoomConnectionList.Reset();
            newRoom.name = "Room " + index;

            return newRoom;
        }
    }
}