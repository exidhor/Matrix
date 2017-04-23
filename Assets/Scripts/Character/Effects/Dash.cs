using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Matrix
{
    [RequireComponent(typeof(Collider2D))]
    public class Dash : Effect
    {
        public float DashSpeed;
        public int Damage;

        private Collider2D _collider;

        void Awake()
        {
            _collider = GetComponent<Collider2D>();
        }

        void OnTriggerEnter2D(Collider2D collider)
        {
            DealDamage(collider.gameObject);
        }

        private void DealDamage(GameObject gameObject)
        {
            LifeComponent lifeComponent = gameObject.GetComponent<LifeComponent>();

            if (lifeComponent != null)
            {
                lifeComponent.ReceiveDamage(Damage);
            }
        }
    }
}