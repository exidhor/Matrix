using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Matrix
{
    [Serializable]
    public class Arc
    {
        public Circle Circle;
        public MarginAngle MarginAngle;

        public Vector2 Start
        {
            get { return MathHelper.GetPointOnCircle(Circle, MarginAngle.Min * Mathf.Deg2Rad); }
        }

        public Vector2 End
        {
            get { return MathHelper.GetPointOnCircle(Circle, MarginAngle.Max * Mathf.Deg2Rad); }   
        }

        public float AngleDirection
        {
            get { return MarginAngle.Average; }
            set { MarginAngle.Average = value; }
        }

        public float Radius
        {
            get { return Circle.Radius; }
            set { Circle.Radius = value; }
        }

        public Vector2 Center
        {
            get { return Circle.Center; }
            set { Circle.Center = value; }
        }

        public Arc(Vector2 center, float directionAngle, float marginOffset, float radius)
        {
            Center = center;
            Radius = radius;

            MarginAngle = new MarginAngle(directionAngle, marginOffset);
        }

        /*!
        * \brief   Check if a point is into an Arc.
        * \param   a_PointToTest the position of the tested point.
        * \return  True if the point is in the arc, false otherwise.
        */
        public bool IsInto(Vector2 pointToTest)
        {
            Vector2 direction = MathHelper.GetDirection(Center, pointToTest);

            if (direction.sqrMagnitude > Radius * Radius)
                return false;

            return MarginAngle.Surround(MathHelper.Angle(direction));
        }

        public Rect GetGlobalBounds()
        {
            // not the best accurency but much quickier to compute
            return Circle.GetGlobalBounds();
        }
    }
}
