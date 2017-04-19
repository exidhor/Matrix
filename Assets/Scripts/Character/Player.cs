using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Matrix
{
    [RequireComponent(typeof(Kinematic), typeof(Collider2D))]
    public class Player : MonoBehaviour
    {
        [HideInInspector]
        public Collider2D Collider;

        private Kinematic _kinematic;
        private AnimatorComponent _animatorComponent;

        private bool _lastFrameWasRunning;
        private bool _lastFrameWasShielding;

        private SteeringOutput _buffer;

        private Shield _shield;

        void Awake()
        {
            _kinematic = GetComponent<Kinematic>();
            _animatorComponent = GetComponent<AnimatorComponent>();

            _lastFrameWasRunning = false;
            _lastFrameWasShielding = false;

            Collider = GetComponent<Collider2D>();

            _buffer = new SteeringOutput();
        }

        void OnEnable()
        {
            QuickFindManager.Instance.Register(this);
        }

        void OnDisable()
        {
            if (QuickFindManager.InternalInstance != null)
            {
                QuickFindManager.Instance.Unregister(this);
            }
        }

        void Update()
        {
            if (TimeManager.Instance.IsPaused)
                return;

            _buffer.Reset();

            bool shieldIsActive = HandleShield();

            if (shieldIsActive)
            {
                HandleOrientation();
            }
            else
            {
                HandleMovement();
            }

            if (Input.GetButtonDown("OpenDoor"))
            {
                Level.Instance.OpenDoors();
            }

            _kinematic.ResetVelocity();
            _kinematic.Actualize(_buffer, Time.fixedDeltaTime);
        }

        private bool HandleShield()
        {
            if (Input.GetButton("Shield"))
            {
                if (!_lastFrameWasShielding)
                {
                    Debug.Log("Shield");

                    _shield = (Shield) EffectManager.Instance.GetFreeEffect(EffectType.Shield);
                    _shield.transform.position = transform.position;
                    _shield.transform.rotation = transform.rotation;
                    _shield.transform.parent = transform;

                    _lastFrameWasShielding = true;
                }

                return true;
            }
            else
            {
                if (_shield != null)
                {
                    _shield.Release();
                    _shield = null;

                    _lastFrameWasShielding = false;
                }
            }

            return false;
        }

        private void HandleOrientation()
        {
            _animatorComponent.SetCurrentAnimation("idle");
            _lastFrameWasRunning = false;

            float horizontal = 0;
            float vertical = 0;

            horizontal = (int)Input.GetAxisRaw("Horizontal");
            vertical = (int)Input.GetAxisRaw("Vertical");



            if (horizontal != 0 || vertical != 0)
            {
                Vector2 direction = transform.position + new Vector3(horizontal, vertical, 0);

                Behavior.Face(ref _buffer, _kinematic, direction);
            }
        }

        private void HandleMovement()
        {
            float horizontal = 0;
            float vertical = 0;

            horizontal = (int)Input.GetAxisRaw("Horizontal");
            vertical = (int)Input.GetAxisRaw("Vertical");

            if (horizontal != 0 && vertical != 0)
            {
                horizontal /= 2;
                vertical /= 2;
            }

            if (_animatorComponent != null)
            {
                if (horizontal != 0 || vertical != 0)
                {
                    if (!_lastFrameWasRunning)
                    {
                        _animatorComponent.SetCurrentAnimation("run");
                        _lastFrameWasRunning = true;
                    }
                }
                else
                {
                    _animatorComponent.SetCurrentAnimation("idle");
                    _lastFrameWasRunning = false;
                }
            }

            _buffer.Linear = new Vector2(horizontal * 1000, vertical * 1000);
        }
    }
}