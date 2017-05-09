using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Matrix
{
    class Camera : MonoBehaviour
    {
        public Player Player;

        void FixedUpdate()
        {
            transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y, transform.position.z);
        }
    }
}
