using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Matrix
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Projectile : PoolObject
    {
        [HideInInspector]
        public Rigidbody2D Rigidbody;
        
        public int Damage;
        public float MaxLifeTime;

        public EffectType DyingEffect;

        public bool IsDead
        {
            get { return _isDead; }
        }

        private bool _isDead;

        private float _currentLifeTime;

        void Awake()
        {
            Rigidbody = GetComponent<Rigidbody2D>();
        }

        void OnEnable()
        {
            _currentLifeTime = 0;
            _isDead = false;
        }

        void Update()
        {
            _currentLifeTime += Time.deltaTime;

            if (_currentLifeTime > MaxLifeTime)
            {
                _isDead = true;
            }
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            DealDamage(collision.gameObject);

            CreateDyingEffect(collision.contacts[0], collision.gameObject.transform);

            _isDead = true;
        }

        private void CreateDyingEffect(ContactPoint2D contact, Transform hitObject)
        {
            Effect effect = EffectManager.Instance.GetFreeEffect(DyingEffect);

            effect.transform.position = contact.point;
            effect.transform.parent = hitObject.transform;

            float angle = Mathf.Atan2(contact.normal.y, contact.normal.x) * Mathf.Rad2Deg;

            effect.transform.eulerAngles = new Vector3(0, 0, angle);
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