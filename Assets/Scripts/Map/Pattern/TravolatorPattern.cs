using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Matrix
{
    public class TravolatorPattern : RoomPattern
    {
        public Vector2 PauseTimeRange;
        public Vector2 TravolatorSpeedRange;
        public float ObjectRate;
        public int Spacing;

        public PoolEntry TravolatorSegment;
        public PoolEntry TableEnd;
        public PoolEntry TableSegment;
        public List<PoolEntry> TravolatorObjects;

        private Pool _travolatorPool;
        private Pool _tableEndPool;
        private Pool _tableSegmentPool;
        private List<Pool> _travolatorObjectPools;

        private Direction _directionBuffer;

        public override void CreatePools()
        {
            _travolatorPool = PoolTable.Instance.AddPool(TravolatorSegment);
            _tableEndPool = PoolTable.Instance.AddPool(TableEnd);
            _tableSegmentPool = PoolTable.Instance.AddPool(TableSegment);

            _travolatorObjectPools = new List<Pool>();

            for (int i = 0; i < TravolatorObjects.Count; i++)
            {
                _travolatorObjectPools.Add(PoolTable.Instance.AddPool(TravolatorObjects[i]));
            }
        }

        public override void Fill(Room room)
        {
            PlaceTravolator(room);

            PlaceEnnemies(room);
        }

        private void PlaceEnnemies(Room room)
        {
            // place some guard at the corners
            List<Coord> guardCoords = new List<Coord>();

            guardCoords.Add(new Coord(1, 1));
            guardCoords.Add(new Coord(room.Length - 2, 1));
            guardCoords.Add(new Coord(room.Length - 2, room.Width - 2));
            guardCoords.Add(new Coord(1, room.Width - 2));

            for (int i = 0; i < guardCoords.Count; i++)
            {
                room.AddEnnemy(EnnemyManager.Instance.GetFreeEnnemy(EnnemyType.Guard), guardCoords[i]);
            }

            List<Coord> workerCoords = new List<Coord>();

            // do nothing if the room is too small
            if (room.Length < 5 || room.Width < 5)
                return;

            if (_directionBuffer == Direction.Left)
            {
                for (int y = Spacing; y < room.Width - 2 - Spacing; y += 1 + Spacing)
                {
                    for (int x = room.Length - 3 - Spacing; x > 1 + Spacing; x--)
                    {
                        workerCoords.Add(new Coord(x, y));
                    }
                }
            }

            else if (_directionBuffer == Direction.Up)
            {
                for (int x = Spacing; x < room.Length - 2 - Spacing; x += 1 + Spacing)
                {
                    for (int y = 2 + Spacing; y < room.Width - 2 - Spacing; y++)
                    {
                        workerCoords.Add(new Coord(x, y));
                    }
                }
            }

            else if (_directionBuffer == Direction.Right)
            {
                for (int y = Spacing; y < room.Width - 2 - Spacing; y += 1 + Spacing)
                {
                    for (int x = 2 + Spacing; x < room.Length - 2 - Spacing; x++)
                    {
                        workerCoords.Add(new Coord(x, y));
                    }
                }
            }

            else if (_directionBuffer == Direction.Down)
            {
                for (int x = Spacing; x < room.Length - 2 - Spacing; x += 1 + Spacing)
                {
                    for (int y = room.Width - 3 - Spacing; y > 1 + Spacing; y--)
                    {
                        workerCoords.Add(new Coord(x, y));
                    }
                }
            }

            for (int i = 0; i < workerCoords.Count; i++)
            {
                room.AddEnnemy(EnnemyManager.Instance.GetFreeEnnemy(EnnemyType.Worker), workerCoords[i]);
            }
        }

        private void PlaceTravolator(Room room)
        {
            _directionBuffer = (Direction)RandomGenerator.Instance.NextInt(4);

            // do nothing if the room is too small
            if (room.Length < 5 || room.Width < 5)
                return;

            if (_directionBuffer == Direction.Left)
            {
                for (int y = 1 + Spacing; y < room.Width - 1 - Spacing; y += 1 + Spacing)
                {
                    TravolatorController travolatorController = (TravolatorController)
                        DynamicControllerManager.Instance.GetFreeDynamicController(DynamicControllerType.Travolator);

                    SetTableObject(room.Length - 2 - Spacing, y, -90, travolatorController, _tableEndPool, room);

                    for (int x = room.Length - 3 - Spacing; x > 1 + Spacing; x--)
                    {
                        SetTableObject(x, y, 90, travolatorController, _tableSegmentPool, room);
                    }

                    SetTableObject(1 + Spacing, y, 90, travolatorController, _tableEndPool, room);

                    FinalizeTravolatorController(travolatorController, room, _directionBuffer);
                }
            }

            else if (_directionBuffer == Direction.Up)
            {
                for (int x = 1 + Spacing; x < room.Length - 1 - Spacing; x += 1 + Spacing)
                {
                    TravolatorController travolatorController = (TravolatorController)
                       DynamicControllerManager.Instance.GetFreeDynamicController(DynamicControllerType.Travolator);

                    SetTableObject(x, 1 + Spacing, -180, travolatorController, _tableEndPool, room);

                    for (int y = 2 + Spacing; y < room.Width - 2 - Spacing; y++)
                    {
                        SetTableObject(x, y, 0, travolatorController, _tableSegmentPool, room);
                    }

                    SetTableObject(x, room.Width - 2 - Spacing, 0, travolatorController, _tableEndPool, room);

                    FinalizeTravolatorController(travolatorController, room, _directionBuffer);
                }
            }

            else if (_directionBuffer == Direction.Right)
            {
                for (int y = 1 + Spacing; y < room.Width - 1 - Spacing; y += 1 + Spacing)
                {
                    TravolatorController travolatorController = (TravolatorController)
                       DynamicControllerManager.Instance.GetFreeDynamicController(DynamicControllerType.Travolator);

                    SetTableObject(1 + Spacing, y, 90, travolatorController, _tableEndPool, room);

                    for (int x = 2 + Spacing; x < room.Length - 2 - Spacing; x++)
                    {
                        SetTableObject(x, y, -90, travolatorController, _tableSegmentPool, room);
                    }

                    SetTableObject(room.Length - 2 - Spacing, y, -90, travolatorController, _tableEndPool, room);

                    FinalizeTravolatorController(travolatorController, room, _directionBuffer);
                }
            }

            else if (_directionBuffer == Direction.Down)
            {
                for (int x = 1 + Spacing; x < room.Length - 1 - Spacing; x += 1 + Spacing)
                {
                    TravolatorController travolatorController = (TravolatorController)
                       DynamicControllerManager.Instance.GetFreeDynamicController(DynamicControllerType.Travolator);

                    SetTableObject(x, room.Width - 2 - Spacing, 0, travolatorController, _tableEndPool, room);

                    for (int y = room.Width - 3 - Spacing; y > 1 + Spacing; y--)
                    {
                        SetTableObject(x, y, -180, travolatorController, _tableSegmentPool, room);
                    }

                    SetTableObject(x, 1 + Spacing, -180, travolatorController, _tableEndPool, room);

                    FinalizeTravolatorController(travolatorController, room, _directionBuffer);
                }
            }
        }

        private void SetTableObject(int x, int y, float angle, TravolatorController parent, Pool pool, Room room)
        {
            Obstacle tableObject = (Obstacle)pool.GetFreeResource();
            parent.Add(tableObject.transform);
            tableObject.transform.eulerAngles = new Vector3(0, 0, angle);
            room.SetObstacle(x, y, tableObject);
        }

        private void FinalizeTravolatorController(TravolatorController travolatorController, Room room, Direction direction)
        {
            travolatorController.SetPools(_travolatorPool, _travolatorObjectPools);
            travolatorController.SetDirection(direction);
            travolatorController.SetPauseTime(RandomGenerator.Instance.NextFloat(PauseTimeRange.x, PauseTimeRange.y));
            travolatorController.SetSpeed(RandomGenerator.Instance.NextFloat(TravolatorSpeedRange.x, TravolatorSpeedRange.y));
            travolatorController.SetObjectRate(ObjectRate);
            room.AddDynamicController(travolatorController);
        }
    }
}