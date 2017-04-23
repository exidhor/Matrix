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
        
        private Room _activeRoom;

        public void Construct(LevelEntry entry)
        {
            _activeRoom = _roomList[0];

            for (int i = 0; i < _roomList.Count; i++)
            {
                ConstructRoom(i, new Vector2(0, 0), entry);
            }

            RoomCreator.Instance.CreateRoom(_roomList[0], entry.StartingPattern);

            for (int i = 1; i < _roomList.Count; i++)
            {
                RoomCreator.Instance.CreateRoom(_roomList[i], PickPattern(entry));
            }

            ConstructMiniMap();
        }

        private void ConstructMiniMap()
        {
            MiniMap.Instance.Clear();
            List<Room> done = new List<Room>();

            ConstructRoomIcons(_activeRoom, done, new Coord(0, 0));
        }

        private void ConstructRoomIcons(Room todo, List<Room> done, Coord currentPosition)
        {
            for (int i = 0; i < done.Count; i++)
            {
                if (todo == done[i])
                    return;
            }

            MiniMap.Instance.AddRoomIcons(currentPosition, todo.RoomIndex);
            done.Add(todo);

            for (int i = 0; i < 4; i++)
            {
                Direction direction = (Direction) i;

                Room room = todo.RoomConnectionList.GetConnectedRoom(direction);

                if (room != null)
                {
                    ConstructRoomIcons(room, done, GetRoomIconPosition(currentPosition, direction));
                }
            }
        }

        private Coord GetRoomIconPosition(Coord position, Direction direction)
        {
            Coord offset = new Coord();

            switch (direction)
            {
                case Direction.Left:
                    offset.x = -1;
                    break;

                case Direction.Up:
                    offset.y = 1;
                    break;

                case Direction.Right:
                    offset.x = 1;
                    break;

                case Direction.Down:
                    offset.y = -1;
                    break;
            }

            return position + offset;
        }

        private RoomPattern PickPattern(LevelEntry entry)
        {
            float totalWeight = 0;

            for (int i = 0; i < entry.WeightedRoomPatterns.Count; i++)
            {
                totalWeight += entry.WeightedRoomPatterns[i].Weight;
            }

            float rate = RandomGenerator.Instance.NextFloat(totalWeight);

            float currentRate = 0;

            for (int i = 0; i < entry.WeightedRoomPatterns.Count; i++)
            {
                currentRate += entry.WeightedRoomPatterns[i].Weight;

                if (currentRate >= rate)
                {
                    return entry.WeightedRoomPatterns[i].RoomPattern;
                }
            }

            // error
            return entry.WeightedRoomPatterns[-1].RoomPattern;
        }

        public void EnterBy(RoomConnection roomConnection)
        {
            Room oldRoom = _activeRoom;
            Room destinationRoom = roomConnection.GetOtherRoom(oldRoom);

            LoadRoom(destinationRoom);

            Door destinationDoor = roomConnection.GetDoorFrom(destinationRoom);

            Player player = QuickFindManager.Instance.GetPlayer();

            player.transform.position = destinationDoor.OutputPoint.position;
            player.transform.rotation = destinationDoor.OutputPoint.rotation;

            LittleBoy littleBoy = QuickFindManager.Instance.GetLittleBoy();

            littleBoy.transform.position = destinationDoor.OutputPoint.position;
            littleBoy.transform.rotation = destinationDoor.OutputPoint.rotation;
        }

        //public void EnterInto(int roomIndex, Direction enterDirection)
        //{
        //    LoadRoom(roomIndex);

        //    //List<Door> doors = _roomList[_currentRoomIndex].GetDoors();
        //    List<Door> doors = _activeRoom.GetDoors();

        //    Direction targetDirection = RoomConnection.GetOppositeDirection(enterDirection);

        //    for (int i = 0; i < doors.Count; i++)
        //    {
                
        //    }
        //}

        //public void LoadRoom(int indexToLoad)
        //{
        //    _roomList[_currentRoomIndex].Disable();

        //    _currentRoomIndex = indexToLoad;
        //    _roomList[_currentRoomIndex].Activate();
        //}

        public void LoadRoom(Room room)
        {
            _activeRoom.Disable();
            _activeRoom = room;

            ClearOldObjects();

            MiniMap.Instance.SetActiveRoom(_activeRoom.RoomIndex);

            _activeRoom.Activate();
        }

        private void ClearOldObjects()
        {
            ProjectileManager.Instance.Clear();
            EffectManager.Instance.Clear();
            //DynamicControllerManager.Instance.Clear();
            EnnemyManager.Instance.Clear();
        }

        public void OpenDoors()
        {
            //_roomList[_currentRoomIndex].OpenDoors();
            _activeRoom.OpenDoors();
        }

        public void ShowStartingRoom()
        {
            //_roomList[_currentRoomIndex].Activate();
            MiniMap.Instance.SetActiveRoom(_activeRoom.RoomIndex);
            _activeRoom.Activate();
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
                //_roomList[i].DestroyRoom();
                _roomList[i].Release();
            }

            _roomList.Clear();
            _activeRoom = null;
            //_currentRoomIndex = -1;
        }

        public void SetRoomList(List<Room> rooms)
        {
            Clear();
            _roomList = rooms;
        }

        public NodeGrid GetCurrentNodeGrid()
        {
            //return _roomList[_currentRoomIndex].Grid.GetNodeGrid();
            return _activeRoom.Grid.GetNodeGrid();
        }

        public Node GetNodeAt(Vector2 position)
        {
            //return _roomList[_currentRoomIndex].Grid.GetNodeAt(position);
            return _activeRoom.Grid.GetNodeAt(position);
        }

        public Vector2 GetPositionAt(Coord coord)
        {
            //return _roomList[_currentRoomIndex].Grid.GetPositionAt(coord);
            return _activeRoom.Grid.GetPositionAt(coord);
        }
    }
}