using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Matrix
{
    [RequireComponent(typeof(Kinematic), typeof(Collider2D), typeof(ManaComponent))]
    public class Player : Character
    {
        [HideInInspector]
        public Collider2D Collider;

        public float MaxRunSpeed;

        public int DashManaCost;

        private Kinematic _kinematic;
        private AnimatorComponent _animatorComponent;
        private ManaComponent _manaComponent;

        private bool _lastFrameWasRunning;
        private bool _lastFrameWasShielding;

        private SteeringOutput _buffer;

        private Shield _shield;
        private Dash _dash;

        private ActionTrigger _openDoorAction;
        private ActionTrigger _dashAction;
        private ActionTrigger _shieldAction;
        private AxisTrigger _horizontalAction;
        private AxisTrigger _verticalAction;

        void Awake()
        {
            _kinematic = GetComponent<Kinematic>();
            _animatorComponent = GetComponent<AnimatorComponent>();
            _manaComponent = GetComponent<ManaComponent>();

            _lastFrameWasRunning = false;
            _lastFrameWasShielding = false;

            Collider = GetComponent<Collider2D>();

            _buffer = new SteeringOutput();

            _openDoorAction = new ActionTrigger();
            _dashAction = new ActionTrigger();
            _shieldAction = new ActionTrigger();
            _horizontalAction = new AxisTrigger();
            _verticalAction = new AxisTrigger();
        }

        public override void OnDeath()
        {
            // todo
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
            if (!TimeManager.Instance.IsPaused)
            {
                HandleInputAction();
            }
            else
            {
                // smth ?
            }
        }

        private void HandleInputAction()
        {
            _openDoorAction.Value = Input.GetButtonDown("OpenDoor");
            _dashAction.Value = Input.GetButtonDown("Dash");
            _shieldAction.Value = Input.GetButton("Shield");
            _horizontalAction.Axis = Input.GetAxisRaw("Horizontal");
            _verticalAction.Axis = Input.GetAxisRaw("Vertical");
        }

        void FixedUpdate()
        {
            if (TimeManager.Instance.IsPaused)
                return;

            _buffer.Reset();

            bool dashIsActive = HandleDash();

            float horizontal = _horizontalAction.Axis;
            float vertical = _verticalAction.Axis;

            if (!dashIsActive)
            {
                bool shieldIsActive = HandleShield();

                if (shieldIsActive)
                {
                    HandleOrientation(horizontal, vertical);
                }
                else
                {
                    HandleMovement(horizontal, vertical);
                }

                //if (Input.GetButtonDown("OpenDoor"))
                if (_openDoorAction)
                {
                    Level.Instance.OpenDoors();
                }
            }
           

            _kinematic.ResetVelocity();
            _kinematic.Actualize(_buffer, TimeManager.Instance.fixedDeltaTime);
        }

        private bool HandleShield()
        {
            //if (Input.GetButton("Shield"))
            if (_shieldAction)
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

        private void HandleOrientation(float horizontal, float vertical)
        {
            _animatorComponent.SetCurrentAnimation("idle");
            _lastFrameWasRunning = false;

            //float horizontal = 0;
            //float vertical = 0;

            //horizontal = (int)Input.GetAxisRaw("Horizontal");
            //vertical = (int)Input.GetAxisRaw("Vertical");



            if (horizontal != 0 || vertical != 0)
            {
                Vector2 direction = transform.position + new Vector3(horizontal, vertical, 0);

                Behavior.Face(ref _buffer, _kinematic, direction);
            }
        }

        private void HandleMovement(float horizontal, float vertical)
        {
            //float horizontal = 0;
            //float vertical = 0;

            //horizontal = (int)Input.GetAxisRaw("Horizontal");
            //vertical = (int)Input.GetAxisRaw("Vertical");

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

            _buffer.Linear = new Vector2(horizontal, vertical);
            _buffer.Linear.Normalize();
            _buffer.Linear *= MaxRunSpeed;
        }

        private bool HandleDash()
        {
            if (_dash == null)
            {
                if (_dashAction && _manaComponent.HasEnoughMana(DashManaCost))
                {
                    _dash = (Dash)EffectManager.Instance.GetFreeEffect(EffectType.Dash);
                    _dash.transform.position = transform.position;
                    _dash.transform.rotation = transform.rotation;
                    _dash.transform.parent = transform;

                    _manaComponent.Use(DashManaCost);
                }
                else
                {
                    return false;
                }
            }

            if (_dash.IsDead)
            {
                _dash.Release();
                _dash = null;

                return false;
            }

            _lastFrameWasRunning = false;
            _animatorComponent.SetCurrentAnimation("dash");
            _buffer.IsOriented = true;
            _buffer.Linear = Vector2.right * _dash.DashSpeed;

            return true;
        }
    }
}