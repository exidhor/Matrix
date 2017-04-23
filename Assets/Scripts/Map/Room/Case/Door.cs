using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Matrix
{
    public class Door : Obstacle
    {
        public Tunnel Tunnel;
        public Rigidbody2D MovableDoorRigidbody;
        public Transform EndPoint;
        public Transform OutputPoint;
        public float OpenningSpeed;

        public bool IsOpen = false;
        public bool IsOpenning = false;

        //public Direction DirectionRelativeToRoom
        //{
        //    get { return Tunnel.Direction; }
        //}

        private float _currentTime;

        private Direction _openningDoorDirection;

        void Awake()
        {
            _currentTime = 0;
        }

        public override void OnPoolExit()
        {
            base.OnPoolExit();

            _currentTime = 0;
            IsOpen = false;
            IsOpenning = false;

            MovableDoorRigidbody.transform.localPosition = Vector3.zero;

            Tunnel.IsActivate = false;
        }

        public void SetOpeningDoorDirection(Direction direction)
        {
            _openningDoorDirection = direction;
        }

        public void SetRoomConnection(RoomConnection roomConnection)
        {
            Tunnel.SetRoomConnection(roomConnection);
        }

        //public void SetDirectionRelativeToRoom(Direction direction)
        //{
        //    Tunnel.Direction = direction;
        //}

        //public void SetConnectedRoomIndex(int connectedRoomIndex)
        //{
        //    Tunnel.SetConnectedRoomIndex(connectedRoomIndex);
        //}

        public void Open()
        {
            IsOpenning = true;
            TranslateDoor();
            Tunnel.IsActivate = true;
        }

        void Update()
        {
            if (IsOpenning)
            {
                if (CheckForStop())
                {
                    IsOpenning = false;
                    IsOpen = true;

                    MovableDoorRigidbody.velocity = Vector2.zero;
                }
            }
        }

        private bool CheckForStop()
        {
            if (_openningDoorDirection == Direction.Left)
            {
                return MovableDoorRigidbody.position.x < EndPoint.position.x;
            }

            if (_openningDoorDirection == Direction.Up)
            {
                return MovableDoorRigidbody.position.y > EndPoint.position.y;
            }

            if (_openningDoorDirection == Direction.Right)
            {
                return MovableDoorRigidbody.position.x > EndPoint.position.x;
            }

            return MovableDoorRigidbody.position.y < EndPoint.position.y;
        }

        private void TranslateDoor()
        {
            if (_openningDoorDirection == Direction.Left)
            {
                MovableDoorRigidbody.velocity = Vector2.left*OpenningSpeed;
            }

            else if (_openningDoorDirection == Direction.Up)
            {
                MovableDoorRigidbody.velocity = Vector2.up*OpenningSpeed;
            }

            else if (_openningDoorDirection == Direction.Right)
            {
                MovableDoorRigidbody.velocity = Vector2.right*OpenningSpeed;
            }

            else
            {
                MovableDoorRigidbody.velocity = Vector2.down*OpenningSpeed;
            }
        }
    }
}