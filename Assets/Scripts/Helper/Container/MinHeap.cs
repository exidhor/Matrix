using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Matrix
{
    // http://robin-thomas.github.io/min-heap/
    public class MinHeap<T>
    {
        private List<HeapObject<T>> _heap;

        public MinHeap()
        {
            _heap = new List<HeapObject<T>>();
        }

        public void Insert(T obj, float sortedValue)
        {
            HeapObject<T> newHeapObject = new HeapObject<T>(obj, sortedValue);

            int currentIndex = _heap.Count;

            while (currentIndex > 0 && newHeapObject.Value < GetParent(currentIndex).Value)
            {
                //_heap[currentIndex] = 
            }
        }

        private HeapObject<T> GetLeftChild(int parentIndex)
        {
            return _heap[2*parentIndex + 1];
        }

        private HeapObject<T> GetRightChild(int parentIndex)
        {
            return _heap[2*parentIndex + 2];
        }

        private HeapObject<T> GetParent(int childIndex)
        {
            return _heap[(childIndex - 1) / 2];
        }
    }
}
