using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Matrix
{
    [Serializable]
    public class Margin
    {
        public float Average;
        public float Offset;

        public float Min
        {
            get { return Average - Offset; }
        }

        public float Max
        {
            get { return Average + Offset; }
        }


        public Margin(float average, float offset)
        {
            Average = average;
            Offset = offset;
        }

        public Margin(Margin toCopy)
        {
            Average = toCopy.Average;
            Offset = toCopy.Offset;
        }

        public virtual bool Surround(float value)
        {
            return Min <= value && value <= Max;
        }
    }
}
