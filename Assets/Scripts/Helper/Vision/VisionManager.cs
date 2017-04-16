using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Matrix
{
    public class VisionManager : MonoSingleton<VisionManager>
    {
        private readonly int MaxSeenObject = 15;
        public LayerMask GroundLayer;

        private RaycastHit2D[] _bufferArray;
        private int _numberOfFilledElement;

        void Start()
        {
            _bufferArray = new RaycastHit2D[MaxSeenObject];
            _numberOfFilledElement = -1;
        }

        public bool See(Collider2D startCollider, Collider2D targetCollider, Vector2 start, Vector2 end, float maxDistance = float.MaxValue, bool obstacleBlockView = true)
        {
            float distance = Vector2.Distance(start, end);

            if (distance > maxDistance)
                return false;

            if (!obstacleBlockView)
                return true;

            FillBufferArray(start, end, obstacleBlockView);

            for (int i = 0; i < _numberOfFilledElement; i++)
            {
                if (_bufferArray[i].collider != startCollider &&
                    _bufferArray[i].collider != targetCollider)
                {
                    return false;
                }
            }

            return true;
        }

        private void FillBufferArray(Vector2 start, Vector2 end, bool obstacleBlockView)
        {
            if (obstacleBlockView)
            {
                _numberOfFilledElement = Physics2D.LinecastNonAlloc(start, end, _bufferArray, GroundLayer);
            }
        }
    }
}
