using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Matrix
{
    public class ObstacleCase
    {
        public Obstacle Obstacle
        {
            get { return _obstacle; }
        }

        private Obstacle _obstacle;

        public bool IsFilled
        {
            get { return _obstacle != null; }
        }

        public void Destroy()
        {
            if (_obstacle != null)
            {
                _obstacle.Release();
            }
        }

        public void Set(Obstacle obstacle)
        {
            if (_obstacle != null)
            {
                _obstacle.Release();
            }

            _obstacle = obstacle;
        }

        public void SetPosition(Vector2 position)
        {
            if (IsFilled)
            {
                _obstacle.transform.position = position;
            }
        }

        public void SetActive(bool state)
        {
            if (IsFilled)
            {
                _obstacle.gameObject.SetActive(state);
            }
        }
    }
}