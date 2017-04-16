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

        public GameObject GroundContainter;
        public GameObject ObstacleContainer;
        public GameObject WallContainer;

        public int Length
        {
            get { return Grid.Length; }
        }

        public int Width
        {
            get { return Grid.Width; }
        }

        public void CreateRoom(Vector2 position, int length, int width)
        {
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
    }
}