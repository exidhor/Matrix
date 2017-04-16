using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Matrix
{
    public static class MathHelper
    {
        public static Vector2 GetKinematicMovement_MinCheck(Vector2 direction, float speed, float min)
        {
            if (direction.sqrMagnitude > min)
            {
                direction.Normalize();
                direction *= speed;
            }
            else
            {
                direction = Vector2.zero;
            }

            return direction;
        }

        // source : http://stackoverflow.com/questions/4780119/2d-euclidean-vector-rotations
        public static Vector2 RotateVector(Vector2 vector2, float angleInRadian)
        {
            float cos = Mathf.Cos(angleInRadian);
            float sin = Mathf.Sin(angleInRadian);

            Vector2 result = new Vector2();

            result.x = vector2.x*cos - vector2.y*sin;
            result.y = vector2.x*sin + vector2.y*cos;

            return result;
        }

        // source : http://math.stackexchange.com/questions/180874/convert-angle-radians-to-a-heading-vector
        public static Vector2 GetDirectionFromAngle(float angleInRadian)
        {
            return new Vector2(Mathf.Cos(angleInRadian),
                Mathf.Sin(angleInRadian));
        }

        // source : http://answers.unity3d.com/questions/24983/how-to-calculate-the-angle-between-two-vectors.html
        public static float Angle(Vector2 from, Vector2 to)
        {
            return Mathf.DeltaAngle(Mathf.Atan2(from.y, from.x) * Mathf.Rad2Deg,
                                    Mathf.Atan2(to.y, to.x) * Mathf.Rad2Deg);
        }

        public static float Angle(Vector2 vector2)
        {
            return Mathf.Atan2(vector2.y, vector2.x) * Mathf.Rad2Deg;
        }

        /*!
         * \brief   Put a point forward a segment (same trajectory)
         * \param   origin this point will determine the sens of the projection
         *          (it will be the closer point of the new shooted point)
         * \param   direction this point determine the direction
         *          (it allowed to get a segment)
         * \param   distance the distance between the origin and the new shooted point
         * \return  the position of the new shooted point
        */
        public static Vector2 ShootPoint(Vector2 origin, Vector2 direction, float distance)
        {
            float segmentLength = Vector2.Distance(origin, direction);

            // place the point at the width distance of the end of the edge
            float deltaX = distance * (origin.x - direction.x) / segmentLength;
            float deltaY = distance * (origin.y - direction.y) / segmentLength;

            float newX = origin.x + deltaX;
            float newY = origin.y + deltaY;

            return new Vector2(newX, newY);
        }

        // source : http://gamedev.stackexchange.com/questions/18340/get-position-of-point-on-circumference-of-circle-given-an-angle
        public static Vector2 GetPointOnCircle(Vector2 circleCenter, float radius, float angleInRadian)
        {
            Vector2 newPoint = Vector2.zero;

            newPoint.x = Mathf.Cos(angleInRadian)*radius + circleCenter.x;
            newPoint.y = Mathf.Sin(angleInRadian)*radius + circleCenter.y;

            return newPoint;
        }

        public static Vector2 GetPointOnCircle(Circle circle, float angleInRadian)
        {
            return GetPointOnCircle(circle.Center, circle.Radius, angleInRadian);
        }

        public static Vector2 GetDirection(Vector2 from, Vector2 to)
        {
            return new Vector2(to.x - from.x,
                                to.y - from.y);
        }

        // source : http://stackoverflow.com/questions/101439/the-most-efficient-way-to-implement-an-integer-based-power-function-powint-int
        public static int Pow(int baseValue, int exp)
        {
            int result = 1;

            while (exp != 0)
            {
                if ((exp & 1) != 0)
                    result *= baseValue;
                exp >>= 1;
                baseValue *= baseValue;
            }

            return result;
        }

        public static int PowerOfTwo(int power)
        {
            return Pow(2, power);
        }
    }
}
