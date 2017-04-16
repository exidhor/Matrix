using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Matrix
{
    public class GunComponent : MonoBehaviour
    {
        public Transform GunPosition;
        public Transform SleeveOutput;
        public ProjectileType ProjectileType;
        public EffectType Sleeve;

        public float ProjectileSpeed;
        public float MaxAngleShot;

        public Vector2 IntervalSleeveSpeed;
        public float MaxSleeveAngle;
        public float MaxSleeveRotation;

        public float ReloadingTime;

        public bool IsReady
        {
            get { return _isReady; }
        }

        private bool _isReady;
        private float _currentReloadingTime = 0;

        void Awake()
        {

        }

        void Start()
        {
            _currentReloadingTime = 0;
            _isReady = true;
        }

        void Update()
        {
            if (!_isReady)
            {
                Reloading();
            }
        }

        private void Reloading()
        {
            _currentReloadingTime += Time.deltaTime;

            if (_currentReloadingTime > ReloadingTime)
            {
                _isReady = true;
                _currentReloadingTime = 0;
            }
        }

        /// <summary>
        /// return true if it really fire, false if it wasn't possible
        /// </summary>
        /// <returns></returns>
        public bool Fire()
        {
            if (IsReady)
            {
                //Debug.Log("Fire");

                Projectile projectile = ProjectileManager.Instance.GetFreeProjectile(ProjectileType);

                projectile.transform.position = GunPosition.position;
                projectile.transform.rotation = transform.rotation;

                float angle = transform.eulerAngles.z + RandomGenerator.Instance.NextFloat(-MaxAngleShot, MaxAngleShot);

                projectile.Rigidbody.velocity = MathHelper.GetDirectionFromAngle(angle * Mathf.Deg2Rad)*ProjectileSpeed;

                ProjectSleeve(projectile.Rigidbody.velocity);

                _isReady = false;

                return true;
            }

            return false;
        }

        public void ProjectSleeve(Vector2 velocity)
        {
            Sleeve sleeve = (Sleeve) EffectManager.Instance.GetFreeEffect(EffectType.Sleeve);

            sleeve.transform.position = SleeveOutput.position;

            sleeve.Rigidbody.angularVelocity = RandomGenerator.Instance.NextFloat(-MaxSleeveRotation, MaxSleeveRotation);

            float speed = RandomGenerator.Instance.NextFloat(IntervalSleeveSpeed.x, IntervalSleeveSpeed.y);
            
            velocity.Normalize();
            velocity *= speed;

            float angle = RandomGenerator.Instance.NextFloat(90 - MaxSleeveAngle, 90 + MaxSleeveAngle);

            velocity = MathHelper.RotateVector(velocity, angle*Mathf.Deg2Rad);

            sleeve.Rigidbody.velocity = velocity;
        }
    }
}