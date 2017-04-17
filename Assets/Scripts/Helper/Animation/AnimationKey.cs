using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Matrix
{
    [Serializable]
    public class AnimationKey
    {
        public string Tag;
        public int Number;
        public EOrientation Orientation;

        public AnimationKey(string tag)
            : this(tag, 0)
        {

        }

        public AnimationKey(string tag, int number)
            : this(tag, number, 0)
        {

        }

        public AnimationKey(string tag, int number, EOrientation orientation)
        {
            Tag = tag;
            Number = number;
            Orientation = orientation;
        }
    }
}
