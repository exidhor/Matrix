using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Matrix
{
    [RequireComponent(typeof(Collider2D))]
    public class Tunnel : PoolObject
    {
        public bool IsActivate;

        private int _connectedRoomIndex;

        public void SetConnectedRoomIndex(int connectedRoomIndex)
        {
            _connectedRoomIndex = connectedRoomIndex;
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (IsActivate)
            {
                Player player = other.GetComponent<Player>();

                if (player != null)
                {
                    Debug.Log("Tunnel");

                    Level.Instance.LoadRoom(_connectedRoomIndex);
                }
            }
        }
    }
}
