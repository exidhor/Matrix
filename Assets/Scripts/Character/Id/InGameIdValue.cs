using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Matrix
{
    public struct InGameIdValue
    {
        public int Value
        {
            get { return _value; }
        }

        public bool IsInverse
        {
            get { return _isInverse; }
        }

        private int _value;
        private bool _isInverse;

        public InGameIdValue(int value)
                : this(value, false)
        {
            // nothing
        }

        public static InGameIdValue ConstructInverse(InGameIdValue id)
        {
            return new InGameIdValue(id.Value, !id.IsInverse);
        }

        public InGameIdValue GetInverseCopy()
        {
            return new InGameIdValue(_value, !_isInverse);
        }

        private InGameIdValue(int value, bool isInverse)
        {
            _value = value;
            _isInverse = isInverse;
        }
    }
}
