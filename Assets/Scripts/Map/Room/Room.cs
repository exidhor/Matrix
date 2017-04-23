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
        public GameObject DynamicControllerContainer;
        public GameObject EnnemyContainer;

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

        private List<Door> _doors;

        private List<DynamicController> _dynamicObjectControllers;

        private List<Ennemy> _ennemies;

        void Awake()
        {
            _roomConnectionList = new RoomConnectionList(this);
            _doors = new List<Door>();

            _dynamicObjectControllers = new List<DynamicController>();
            _ennemies = new List<Ennemy>();
        }

        public override void OnPoolEnter()
        {
            base.OnPoolEnter();

            DestroyRoom();
        }

        public void CreateRoom(Vector2 position, int length, int width, int roomIndex)
        {
            _roomIndex = roomIndex;

            transform.position = position;
            Grid = new RoomGrid(length, width, position);
        }

        void Update()
        {
            if (_ennemies.Count == 0)
            {
                OpenDoors();
            }
        }

        public void OpenDoors()
        {
            for (int i = 0; i < _doors.Count; i++)
            {
                _doors[i].Open();
            }
        }

        public void DestroyRoom()
        {
            Grid.DestroyObjects();

            _doors.Clear();

            for (int i = 0; i < _dynamicObjectControllers.Count; i++)
            {
                _dynamicObjectControllers[i].Release();
            }

            _dynamicObjectControllers.Clear();

            for (int i = 0; i < _ennemies.Count; i++)
            {
                _ennemies[i].Release();
            }

            _ennemies.Clear();
        }

        public void Activate()
        {
            Grid.SetActive(true);

            for (int i = 0; i < _dynamicObjectControllers.Count; i++)
            {
                _dynamicObjectControllers[i].Activate();
            }

            for (int i = 0; i < _ennemies.Count; i++)
            {
                _ennemies[i].gameObject.SetActive(true);
                EnnemyManager.Instance.RestoreEnnemy(_ennemies[i]);
            }
        }

        public void Disable()
        {
            Grid.SetActive(false);

            for (int i = 0; i < _dynamicObjectControllers.Count; i++)
            {
                _dynamicObjectControllers[i].Disable();
            }

            for (int i = 0; i < _ennemies.Count; i++)
            {
                _ennemies[i].gameObject.SetActive(false);
            }
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

            _doors.Add(door);

            Grid.SetObstacle(x, y, door);
        }

        public void AddDynamicController(DynamicController controller)
        {
            controller.transform.parent = DynamicControllerContainer.transform;

            _dynamicObjectControllers.Add(controller);
        }

        public void AddEnnemy(Ennemy ennemy, Coord coord)
        {
            ennemy.transform.parent = EnnemyContainer.transform;
            ennemy.SetRoomPlace(this);

            Grid.SetCharacter(coord, ennemy);

            _ennemies.Add(ennemy);
        }

        public void RemoveEnnemy(Ennemy ennemy)
        {
            _ennemies.Remove(ennemy);
        }

        public List<Door> GetDoors()
        {
            return _doors;
        }
    }
}