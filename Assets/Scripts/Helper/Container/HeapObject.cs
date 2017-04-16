using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Matrix
{
    public struct HeapObject<T>
    {
        public T Object;
        public float Value;

        public HeapObject(T obj, float value)
        {
            Object = obj;
            Value = value;
        }
    }
}
