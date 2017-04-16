using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Matrix
{
    [Serializable]
    public class Circle
    {
        public Vector2 Center;
        public float Radius;

        public Circle(Vector2 center, float radius)
        {
            Center = center;
            Radius = radius;
        }

        public bool IsInside(Vector2 point)
        {
            float distance = Vector2.Distance(point, Center);

            return distance < Radius;
        }

        public Rect GetGlobalBounds()
        {
            return new Rect(Center.x - Radius,
                            Center.y - Radius,
                            Radius*2,
                            Radius*2);
        }
    }
}
