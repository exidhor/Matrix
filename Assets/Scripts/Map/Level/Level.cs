using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Matrix
{
    public class Level : MonoSingleton<Level>
    {
        private List<Room> _roomList = new List<Room>();

        [SerializeField]
        private int _currentRoomIndex;

        public void Construct(LevelEntry entry)
        {
            _currentRoomIndex = 0;

            for (int i = 0; i < _roomList.Count; i++)
            {
                ConstructRoom(i, new Vector2(0, 0), entry);
            }

            for (int i = 0; i < _roomList.Count; i++)
            {
                RoomCreator.Instance.CreateRoom(_roomList[i]);
            }
        }

        public void LoadRoom(int indexToLoad)
        {
            _roomList[_currentRoomIndex].Disable();

            _currentRoomIndex = indexToLoad;
            _roomList[_currentRoomIndex].Activate();
        }

        public void ShowStartingRoom()
        {
            _roomList[_currentRoomIndex].Activate();
        }

        private void ConstructRoom(int roomIndex, Vector2 position, LevelEntry entry)
        {
            int randomLength = RandomGenerator.Instance.NextInt(entry.MinRoomLength, entry.MaxRoomLength + 1);
            int randomWidth = RandomGenerator.Instance.NextInt(entry.MinRoomWidth, entry.MaxRoomWidth + 1);

            _roomList[roomIndex].CreateRoom(position, randomLength, randomWidth, roomIndex);
            _roomList[roomIndex].transform.parent = transform;
        }

        public void Clear()
        {
            for (int i = 0; i < _roomList.Count; i++)
            {
                _roomList[i].DestroyRoom();
                _roomList[i].Release();
            }

            _roomList.Clear();
            _currentRoomIndex = -1;
        }

        public void SetRoomList(List<Room> rooms)
        {
            Clear();
            _roomList = rooms;
        }

        public NodeGrid GetCurrentNodeGrid()
        {
            return _roomList[_currentRoomIndex].Grid.GetNodeGrid();
        }

        public Node GetNodeAt(Vector2 position)
        {
            return _roomList[_currentRoomIndex].Grid.GetNodeAt(position);
        }

        public Vector2 GetPositionAt(Coord coord)
        {
            return _roomList[_currentRoomIndex].Grid.GetPositionAt(coord);
        }
    }
}
