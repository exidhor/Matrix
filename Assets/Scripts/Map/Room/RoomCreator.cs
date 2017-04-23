using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Matrix
{
    public class RoomCreator : MonoSingleton<RoomCreator>
    {
        public List<WeightedModel> GroundModels;

        //public List<WeightedModel> ObstacleModels;

        public List<WeightedModel> WallModels;

        public List<WeightedModel> WallIntersectionModels;

        public List<WeightedModel> DoorModels;

        //public float ObstacleRate;

        void Awake()
        {
            CreatePools();
        }

        private void CreatePools()
        {
            for (int i = 0; i < GroundModels.Count; i++)
            {
                PoolTable.Instance.AddPool(GroundModels[i].Model);
            }

            //for (int i = 0; i < ObstacleModels.Count; i++)
            //{
            //    PoolTable.Instance.AddPool(ObstacleModels[i].Model);
            //}

            for (int i = 0; i < WallModels.Count; i++)
            {
                PoolTable.Instance.AddPool(WallModels[i].Model);
            }

            for (int i = 0; i < WallIntersectionModels.Count; i++)
            {
                PoolTable.Instance.AddPool(WallIntersectionModels[i].Model);
            }

            for (int i = 0; i < DoorModels.Count; i++)
            {
                PoolTable.Instance.AddPool(DoorModels[i].Model);
            }
        }

        //void Start()
        //{
        //    RoomList.Add(CreateRoom(0));
        //    CurrentRoomIndex = 0;

        //    RoomList[0].Activate();
        //}

        //public void Construct()
        //{
        //    if (RoomList.Count > 0)
        //    {
        //        for (int i = 0; i < RoomList.Count; i++)
        //        {
        //            RoomList[i].DestroyRoom();
        //            _poolRoom.ReleaseResource(RoomList[i]);
        //        }

        //        RoomList.Clear();
        //    }

        //    RoomList.Add(CreateRoom(0));
        //    CurrentRoomIndex = 0;

        //    RoomList[0].Activate();
        //}

        //public NodeGrid GetCurrentNodeGrid()
        //{
        //    return RoomList[CurrentRoomIndex].Grid.GetNodeGrid();
        //}

        //public Node GetNodeAt(Vector2 position)
        //{
        //    return RoomList[CurrentRoomIndex].Grid.GetNodeAt(position);
        //}

        //public Vector2 GetPositionAt(Coord coord)
        //{
        //    return RoomList[CurrentRoomIndex].Grid.GetPositionAt(coord);
        //}

        //private Room GetNewRoom()
        //{
        //    return _poolRoom.GetFreeResource().GetComponent<Room>();
        //}

        //private Room CreateRoom(int index)
        //{
        //    Room newRoom = GetNewRoom();
        //    newRoom.name = "Room " + index;

        //    newRoom.transform.parent = this.transform;

        //    newRoom.CreateRoom(new Vector2(0, 0), RoomLength, RoomWidth);

        //    CreateGrounds(newRoom);
        //    CreateWalls(newRoom);
        //    CreateObstacles(newRoom);

        //    newRoom.Grid.Organize(1f, 1f);

        //    newRoom.Grid.ComputeNodeConnections();

        //    newRoom.Disable();

        //    return newRoom;
        //}

        public void CreateRoom(Room room, RoomPattern pattern)
        {
            CreateGrounds(room);
            CreateWalls(room);
            CreateDoors(room);

            pattern.Fill(room);
            //CreateObstacles(room);

            room.Grid.Organize(1f, 1f);

            room.Grid.ComputeNodeConnections();

            room.Disable();
        }

        private void CreateGrounds(Room room)
        {
            for (int i = 0; i < room.Length; i++)
            {
                for (int j = 0; j < room.Width; j++)
                {
                    Ground ground = PickGroundModel(i, j);

                    room.SetGround(i, j, ground);
                }
            }
        }

        private void CreateWalls(Room room)
        {
            // left walls
            for (int i = 1; i < room.Width - 1; i++)
            {
                Wall wall = PickFrom<Wall>(0, i, WallModels);
                wall.transform.eulerAngles = new Vector3(0, 0, 90);

                room.SetWall(0, i, wall);
            }

            // top walls
            for (int i = 1; i < room.Length - 1; i++)
            {
                Wall wall = PickFrom<Wall>(i, room.Width - 1, WallModels);
                wall.transform.eulerAngles = new Vector3(0, 0, 0);

                room.SetWall(i, room.Width - 1, wall);
            }

            // right walls
            for (int i = 1; i < room.Width - 1; i++)
            {
                Wall wall = PickFrom<Wall>(room.Length - 1, i, WallModels);
                wall.transform.eulerAngles = new Vector3(0, 0, -90);

                room.SetWall(room.Length - 1, i, wall);
            }

            // bot walls
            for (int i = 1; i < room.Length - 1; i++)
            {
                Wall wall = PickFrom<Wall>(i, 0, WallModels);
                wall.transform.eulerAngles = new Vector3(0, 0, -180);

                room.SetWall(i, 0, wall);
            }

            // top - left intersection
            {
                Wall topLeftIntersection = PickFrom<Wall>(0, room.Width - 1, WallIntersectionModels);
                topLeftIntersection.transform.eulerAngles = new Vector3(0, 0, 90);

                room.SetWall(0, room.Width - 1, topLeftIntersection);
            }

            // top - right intersection
            {
                Wall topRightIntersection = PickFrom<Wall>(room.Length-1, room.Width - 1, WallIntersectionModels);
                topRightIntersection.transform.eulerAngles = new Vector3(0, 0, 0);

                room.SetWall(room.Length - 1, room.Width - 1, topRightIntersection);
            }

            // bot - right intersection
            {
                Wall botRightIntersection = PickFrom<Wall>(room.Length - 1, 0, WallIntersectionModels);
                botRightIntersection.transform.eulerAngles = new Vector3(0, 0, -90);

                room.SetWall(room.Length - 1, 0, botRightIntersection);
            }

            // bot - left intersection
            {
                Wall botLeftIntersection = PickFrom<Wall>(0, 0, WallIntersectionModels);
                botLeftIntersection.transform.eulerAngles = new Vector3(0, 0, 180);

                room.SetWall(0, 0, botLeftIntersection);
            }
        }

        //private void CreateObstacles(Room room)
        //{
        //    for (int i = 1; i < room.Length - 1; i++)
        //    {
        //        for (int j = 1; j < room.Width - 1; j++)
        //        {
        //            float rate = RandomGenerator.Instance.NextFloat();

        //            if (rate < ObstacleRate)
        //            {
        //                Obstacle obstacle = PickObstacleModel(i, j);

        //                room.SetObstacle(i, j, obstacle);
        //            }
        //        }
        //    }
        //}

        private void CreateDoors(Room room)
        {
            int lengthMiddle = room.Length / 2;
            int widthMiddle = room.Width / 2;

            // left
            RoomConnection leftConnection = room.RoomConnectionList.GetConnection(Direction.Left);
            if (leftConnection != null)
            {
                // Get the middle of the wall
                Door door = PickFrom<Door>(0, widthMiddle, DoorModels);
                //door.SetConnectedRoomIndex(leftConnection.GetOtherRoom(room).RoomIndex);
                door.transform.eulerAngles = new Vector3(0, 0, 90);
                door.SetOpeningDoorDirection(Direction.Down);
                //door.SetDirectionRelativeToRoom(Direction.Left);
                door.SetRoomConnection(leftConnection);

                leftConnection.SetDoor(door, room);

                room.SetDoor(0, widthMiddle, door);
            }

            // top
            RoomConnection topConnection = room.RoomConnectionList.GetConnection(Direction.Up);
            if (topConnection != null)
            {
                // Get the middle of the wall
                Door door = PickFrom<Door>(lengthMiddle, room.Width - 1, DoorModels);
                //door.SetConnectedRoomIndex(topConnection.GetOtherRoom(room).RoomIndex);
                door.transform.eulerAngles = new Vector3(0, 0, 0);
                door.SetOpeningDoorDirection(Direction.Left);
                //door.SetDirectionRelativeToRoom(Direction.Up);
                door.SetRoomConnection(topConnection);

                topConnection.SetDoor(door, room);

                room.SetDoor(lengthMiddle, room.Width-1, door);
            }

             // right
            RoomConnection rightConnection = room.RoomConnectionList.GetConnection(Direction.Right);
            if (rightConnection != null)
            {
                // Get the middle of the wall
                Door door = PickFrom<Door>(room.Length-1, widthMiddle, DoorModels);
                //door.SetConnectedRoomIndex(rightConnection.GetOtherRoom(room).RoomIndex);
                door.transform.eulerAngles = new Vector3(0, 0, -90);
                door.SetOpeningDoorDirection(Direction.Up);
                //door.SetDirectionRelativeToRoom(Direction.Right);
                door.SetRoomConnection(rightConnection);

                rightConnection.SetDoor(door, room);

                room.SetDoor(room.Length-1, widthMiddle, door);
            }

            // bottom
            RoomConnection bottomConnection = room.RoomConnectionList.GetConnection(Direction.Down);
            if (bottomConnection != null)
            {
                // Get the middle of the wall
                Door door = PickFrom<Door>(lengthMiddle, 0, DoorModels);
                //door.SetConnectedRoomIndex(bottomConnection.GetOtherRoom(room).RoomIndex);
                door.transform.eulerAngles = new Vector3(0, 0, -180);
                door.SetOpeningDoorDirection(Direction.Right);
                //door.SetDirectionRelativeToRoom(Direction.Down);
                door.SetRoomConnection(bottomConnection);

                bottomConnection.SetDoor(door, room);

                room.SetDoor(lengthMiddle, 0, door);
            }
        }

        private T PickFrom<T>(int x, int y, List<WeightedModel> list)
            where T : PoolObject
        {
            float Rate = RandomGenerator.Instance.NextFloat();

            float rateCount = 0;
            int foundIndex = list.Count - 1;

            for (int i = 0; i < list.Count; i++)
            {
                rateCount += list[i].Weight;

                if (Rate < rateCount)
                {
                    foundIndex = i;
                    break;
                }
            }

            Pool pool = PoolTable.Instance.GetPool(list[foundIndex].Model.Prefab.GetInstanceID());
            T newObject = (T)pool.GetFreeResource();

            newObject.name = list[foundIndex].Model.Prefab.name + " (" + x + ", " + y + ")";

            return newObject;
        }

        //private Obstacle PickObstacleModel(int x, int y)
        //{
        //    return PickFrom<Obstacle>(x, y, ObstacleModels);
        //}

        private Ground PickGroundModel(int x, int y)
        {
            return PickFrom<Ground>(x, y, GroundModels);
        }
    }
}