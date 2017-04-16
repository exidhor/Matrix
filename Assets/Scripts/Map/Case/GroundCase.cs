using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Matrix
{
    public class GroundCase
    {
        private Ground _ground;

        public void Destroy()
        {
            if (_ground != null)
            {
                _ground.Release();
            }
        }

        public void Set(Ground ground)
        {
            _ground = ground;
        }

        public void SetPosition(Vector2 position)
        {
            if (_ground != null)
            {
                _ground.transform.position = position;
            }
        }

        public void SetActive(bool state)
        {
            if (_ground != null)
            {
                _ground.gameObject.SetActive(state);
            }
        }
    }
}