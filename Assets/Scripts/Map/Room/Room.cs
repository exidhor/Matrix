using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Matrix
{
    public class Room : PoolObject
    {
        public RoomGrid Grid;

        public RoomConnectionList RoomConnectionList
        {
            get { return _roomConnectionList; }
        }

        private RoomConnectionList _roomConnectionList;

        public GameObject GroundContainter;
        public GameObject ObstacleContainer;
        public GameObject WallContainer;
        public GameObject DoorContainer;

        public int Length
        {
            get { return Grid.Length; }
        }

        public int Width
        {
            get { return Grid.Width; }
        }

        public int RoomIndex
        {
            get { return _roomIndex; }
        }

        private int _roomIndex;

        void Awake()
        {
            _roomConnectionList = new RoomConnectionList(this);
        }

        public void CreateRoom(Vector2 position, int length, int width, int roomIndex)
        {
            _roomIndex = roomIndex;

            transform.position = position;
            Grid = new RoomGrid(length, width, position);
        }

        public void DestroyRoom()
        {
            Grid.DestroyObjects();
        }

        public void Activate()
        {
            Grid.SetActive(true);
        }

        public void Disable()
        {
            Grid.SetActive(false);
        }

        public void SetGround(int x, int y, Ground ground)
        {
            ground.transform.parent = GroundContainter.transform;

            Grid.SetGround(x, y, ground);
        }

        public void SetObstacle(int x, int y, Obstacle obstacle)
        {
            obstacle.transform.parent = ObstacleContainer.transform;

            Grid.SetObstacle(x, y, obstacle);
        }

        public void SetWall(int x, int y, Wall wall)
        {
            wall.transform.parent = WallContainer.transform;

            Grid.SetObstacle(x, y, wall);
        }

        public void SetDoor(int x, int y, Door door)
        {
            door.transform.parent = DoorContainer.transform;

            Grid.SetObstacle(x, y, door);
        }
    }
}