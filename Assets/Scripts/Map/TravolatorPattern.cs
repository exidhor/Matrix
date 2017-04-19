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

        public PoolEntry TravolatorSegment;
        public PoolEntry TableEnd;
        public PoolEntry TableSegment;

        private Pool _travolatorPool;
        private Pool _tableEndPool;
        private Pool _tableSegmentPool;

        public override void CreatePools()
        {
            _travolatorPool = PoolTable.Instance.AddPool(TravolatorSegment);
            _tableEndPool = PoolTable.Instance.AddPool(TableEnd);
            _tableSegmentPool = PoolTable.Instance.AddPool(TableSegment);
        }

        public override void Fill(Room room)
        {
            Direction direction = (Direction) RandomGenerator.Instance.NextInt(5);

            // do nothing if the room is too small
            if (room.Length < 5 || room.Width < 5)
                return;

            if (direction == Direction.Left)
            {
                for (int y = 2; y < room.Width - 2; y += 2)
                {
                    TravolatorController travolatorController = (TravolatorController)
                        DynamicControllerManager.Instance.GetFreeDynamicController(DynamicControllerType.Travolator);

                    SetTableObject(room.Length - 3, y, -90, travolatorController, _tableEndPool, room);

                    for (int x = room.Length - 4; x > 2; x--)
                    {
                        SetTableObject(x, y, 90, travolatorController, _tableSegmentPool, room);
                    }

                    SetTableObject(2, y, 90, travolatorController, _tableEndPool, room);

                    FinalizeTravolatorController(travolatorController, room, direction);
                }
            }

            else if (direction == Direction.Up)
            {
                for (int x = 2; x < room.Length - 2; x += 2)
                {
                    TravolatorController travolatorController = (TravolatorController)
                       DynamicControllerManager.Instance.GetFreeDynamicController(DynamicControllerType.Travolator);

                    SetTableObject(x, 2, -180, travolatorController, _tableEndPool, room);

                    for (int y = 3; y < room.Width - 3; y++)
                    {
                        SetTableObject(x, y, 0, travolatorController, _tableSegmentPool, room);
                    }

                    SetTableObject(x, room.Width - 3, 0, travolatorController, _tableEndPool, room);

                    FinalizeTravolatorController(travolatorController, room, direction);
                }
            }

            else if (direction == Direction.Right)
            {
                for (int y = 2; y < room.Width - 2; y += 2)
                {
                    TravolatorController travolatorController = (TravolatorController)
                       DynamicControllerManager.Instance.GetFreeDynamicController(DynamicControllerType.Travolator);

                    SetTableObject(2, y, 90, travolatorController, _tableEndPool, room);

                    for (int x = 3; x < room.Length - 3; x++)
                    {
                        SetTableObject(x, y, -90, travolatorController, _tableSegmentPool, room);
                    }

                    SetTableObject(room.Length - 3, y, -90, travolatorController, _tableEndPool, room);

                    FinalizeTravolatorController(travolatorController, room, direction);
                }
            }

            else if (direction == Direction.Down)
            {
                for (int x = 2; x < room.Length - 2; x += 2)
                {
                    TravolatorController travolatorController = (TravolatorController)
                       DynamicControllerManager.Instance.GetFreeDynamicController(DynamicControllerType.Travolator);

                    SetTableObject(x, room.Width - 3, 0, travolatorController, _tableEndPool, room);

                    for (int y = room.Width - 4; y > 2; y--)
                    {
                        SetTableObject(x, y, -180, travolatorController, _tableSegmentPool, room);
                    }

                    SetTableObject(x, 2, -180, travolatorController, _tableEndPool, room);

                    FinalizeTravolatorController(travolatorController, room, direction);
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
            travolatorController.SetPool(_travolatorPool);
            travolatorController.SetDirection(direction);
            travolatorController.SetPauseTime(RandomGenerator.Instance.NextFloat(PauseTimeRange.x, PauseTimeRange.y));
            travolatorController.SetSpeed(RandomGenerator.Instance.NextFloat(TravolatorSpeedRange.x, TravolatorSpeedRange.y));
            room.AddDynamicController(travolatorController);
        }
    }
}