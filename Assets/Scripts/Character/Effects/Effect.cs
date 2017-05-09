using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Matrix
{
    public class Effect : PoolObject
    {
        public bool IsManaged;
        public float MaxLifeTime;

        public bool IsDead
        {
            get { return _isDead; }
        }

        protected bool _isDead;

        protected float _currentLifeTime;

        public override void OnPoolExit()
        {
            base.OnPoolExit();

            _currentLifeTime = 0;
            _isDead = false;
        }

        protected virtual void Update()
        {
            _currentLifeTime += TimeManager.Instance.deltaTime;

            if (_currentLifeTime > MaxLifeTime)
            {
                _isDead = true;
            }
        }
    }
}