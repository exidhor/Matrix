using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Matrix
{
    public class ActionTrigger
    {
        public static implicit operator bool(ActionTrigger actionTrigger)
        {
            return actionTrigger.Value;
        }

        public bool Value
        {
            get
            {
                _isChecked = true;
                return _value;
            }

            set
            {
                if (!_value || _isChecked)
                {
                    _value = value;
                    _isChecked = false;
                }
            }
        }

        private bool _value = false;
        private bool _isChecked = false;

        public void Reset()
        {
            _value = false;
            _isChecked = false;
        }
    }
}
