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

        void Awake()
        {
            _kinematic = GetComponent<Kinematic>();
            _animatorComponent = GetComponent<AnimatorComponent>();

            _lastFrameWasRunning = false;

            Collider = GetComponent<Collider2D>();
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
            float horizontal = 0;
            float vertical = 0;

            horizontal = (int) Input.GetAxisRaw("Horizontal");
            vertical = (int) Input.GetAxisRaw("Vertical");

            bool isDiagonal = false;

            if (horizontal != 0 && vertical != 0)
            {
                horizontal /= 2;
                vertical /= 2;
            }

            if (_animatorComponent != null && !TimeManager.Instance.IsPaused)
            {
                if (horizontal != 0 || vertical != 0)
                {
                    if(!_lastFrameWasRunning)
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

            SteeringOutput output = new SteeringOutput();
            output.Linear = new Vector2(horizontal*1000, vertical*1000);

            _kinematic.ResetVelocity();

            _kinematic.Actualize(output, Time.fixedDeltaTime);
        }
    }
}