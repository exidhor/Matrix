using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Matrix
{
    [RequireComponent(typeof(LifeComponent))]
    public abstract class Character : PoolObject
    {
        public bool IsAlive
        {
            get { return _lifeComponent.IsAlive; }
        }

        protected LifeComponent _lifeComponent;
        protected Room _roomPlace;

        protected virtual void Awake()
        {
            _lifeComponent = GetComponent<LifeComponent>();
        }

        public override void OnPoolExit()
        {
            base.OnPoolExit();
            _lifeComponent.Init();
        }

        public void SetRoomPlace(Room roomPlace)
        {
            _roomPlace = roomPlace;
        }

        public override void OnPoolEnter()
        {
            base.OnPoolEnter();

            if (_roomPlace != null)
            {
                _roomPlace.RemoveEnnemy(this);
                _roomPlace = null;
            }
        }

        public abstract void OnDeath();
    }
}
