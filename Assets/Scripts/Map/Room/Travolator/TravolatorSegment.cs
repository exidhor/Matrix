using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Matrix
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class TravolatorSegment : Effect
    {
        [HideInInspector]
        public Rigidbody2D Rigidbody;

        public TravolatorObject TravolatorObject;

        void Awake()
        {
            Rigidbody = GetComponent<Rigidbody2D>();
        }

        public void SetTravolatorObject(TravolatorObject travolatorObject)
        {
            TravolatorObject = travolatorObject;

            if (travolatorObject != null)
            {
                TravolatorObject.transform.parent = transform;
                TravolatorObject.transform.position = transform.position;
                TravolatorObject.transform.rotation = transform.rotation;
            }
        }

        public override void OnPoolEnter()
        {
            TravolatorObject = null;
        }
    }
}
