using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Matrix
{
    public class Effect : PoolObject
    {
        public float MaxLifeTime;

        public bool IsDead
        {
            get { return _isDead; }
        }

        private bool _isDead;

        protected float _currentLifeTime;

        public override void OnPoolExit()
        {
            base.OnPoolExit();

            _currentLifeTime = 0;
            _isDead = false;
        }

        protected virtual void Update()
        {
            _currentLifeTime += Time.deltaTime;

            if (_currentLifeTime > MaxLifeTime)
            {
                _isDead = true;
            }
        }
    }
}