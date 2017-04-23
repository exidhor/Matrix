using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Matrix
{
    [RequireComponent(typeof(Collider2D))]
    public class Tunnel : MonoBehaviour
    {
        public bool IsActivate;

        private RoomConnection _roomConnection;

        //public Direction Direction;

        //private int _connectedRoomIndex;

        //public void SetConnectedRoomIndex(int connectedRoomIndex)
        //{
        //    _connectedRoomIndex = connectedRoomIndex;
        //}

        public void SetRoomConnection(RoomConnection roomConnection)
        {
            _roomConnection = roomConnection;
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (IsActivate)
            {
                Player player = other.GetComponent<Player>();

                if (player != null)
                {
                    //Debug.Log("Tunnel");

                    Level.Instance.EnterBy(_roomConnection);
                    //Level.Instance.LoadRoom(_connectedRoomIndex);
                }
            }
        }
    }
}