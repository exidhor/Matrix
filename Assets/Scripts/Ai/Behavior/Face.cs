﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Matrix
{
    public static partial class Behavior
    {
        public static void Face(ref SteeringOutput output, Kinematic character, Vector2 target)
        {
            output.IsInstantOrientation = true;
            output.IsOriented = false;
            //output.IsOriented = true;

            // work out the direction to target
            Vector2 direction = target - character.GetPosition();

            // Check for a zero direction, and make no change if so
            if (direction.sqrMagnitude < float.Epsilon*float.Epsilon)
            {
                return;
            }

            output.AngularInDegree = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        }
    }
}
