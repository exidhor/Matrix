using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Matrix
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Sleeve : Effect
    {
        [HideInInspector]
        public Rigidbody2D Rigidbody;

        void Awake()
        {
            Rigidbody = GetComponent<Rigidbody2D>();
        }
    }
}
