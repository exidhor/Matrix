using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Matrix
{
    [Serializable]
    public class MarginAngle : Margin
    {
        public MarginAngle(float average, float offset)
            : base(average, offset)
        {
            // nothing
        }

        public MarginAngle(Margin toCopy)
            : base(toCopy)
        {
            // nothing
        }

        public override bool Surround(float value)
        {
            float distance = Mathf.DeltaAngle(Average, value);

            //Debug.Log("Average : " + Average + " | value : " + value + " | distance : " + distance);

            return -Offset < distance && distance < Offset;
        }
    }
}