using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Matrix
{
    public class Selectable : MonoBehaviour
    {
        public Circle Collider;

        public bool IsSelected
        {
            get { return _isSelected; }
        }

        private bool _isSelected;

        void OnEnable()
        {
            InputManager.Instance.Register(this);
        }

        void OnDisable()
        {
            if (InputManager.InternalInstance != null)
            {
                InputManager.Instance.Unregister(this);
            }
        }

        public void Actualize()
        {
            Collider.Center = transform.position;
        }

        public void Select()
        {
            _isSelected = true;
        }

        public void Unselect()
        {
            _isSelected = false;
        }
    }
}
