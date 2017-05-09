using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Matrix
{
    [RequireComponent(typeof(SteeringComponent), typeof(Kinematic), typeof(LifeComponent))]
    public class Worker : Character
    {
        public float RefreshTimeVision = 0.1f;
        public float RangeToFlee = 15f;

        [HideInInspector]
        public Collider2D Collider;
        [HideInInspector]
        public ManaExpeller ManaExpeller;

        public VisionAI VisionAI;
        public GroundMovement GroundMovement;

        public bool IsAlive
        {
            get { return _lifeComponent.IsAlive; }
        }

        private SteeringComponent _steeringComponent;
        private Kinematic _kinematic;
        private AnimatorComponent _animatorComponent;

        private bool _isWorking;

        private bool _lastFrameWasRunning;
        private bool _lastFrameWasWorking;

        void Awake()
        {
            Collider = GetComponent<Collider2D>();
            _steeringComponent = GetComponent<SteeringComponent>();
            _kinematic = GetComponent<Kinematic>();
            _animatorComponent = GetComponent<AnimatorComponent>();
            _lifeComponent = GetComponent<LifeComponent>();
            ManaExpeller = GetComponent<ManaExpeller>();


            VisionAI = new VisionAI(Collider, transform, true, false, RefreshTimeVision);
            GroundMovement = new GroundMovement(_steeringComponent, transform);
        }

        public override void OnPoolExit()
        {
            base.OnPoolExit();
            _lifeComponent.Init();
        }

        public override void OnDeath()
        {
            ManaExpeller.Expeller();
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

        void FixedUpdate()
        {
            _steeringComponent.ClearBehaviors();

            SetTargetMovement();
            GroundMovement.Actualize(1f);

            if (!_kinematic.isMoving)
            {
                VisionAI.TryToSeeSomething(TimeManager.Instance.fixedDeltaTime);

                if (VisionAI.SeeSomething)
                {
                    FaceSeenTarget();
                }
            }

            UpdateAnimator();

            _steeringComponent.ActualizeSteering();
            _steeringComponent.ApplySteeringOnKinematic(TimeManager.Instance.fixedDeltaTime);
        }

        private void UpdateAnimator()
        {
            if (_kinematic.isMoving)
            {
                if (!_lastFrameWasRunning)
                {
                    _animatorComponent.SetCurrentAnimation("Run");
                    _lastFrameWasRunning = true;
                    _lastFrameWasWorking = false;
                }
            }
            else
            {
                if (_isWorking)
                {
                    if (!_lastFrameWasWorking)
                    {
                        _animatorComponent.SetCurrentAnimation("Work");
                        _lastFrameWasRunning = false;
                        _lastFrameWasWorking = true;
                    }
                }
                else
                {
                    _animatorComponent.SetCurrentAnimation("Idle");
                    _lastFrameWasRunning = false;
                    _lastFrameWasWorking = false;
                }
            }
        }

        private void SetTargetMovement()
        {
            if (QuickFindManager.Instance.PlayerIsThere)
            {
                Vector2 playerPosition = QuickFindManager.Instance.GetPlayer().transform.position;

                float distanceToPlayer = Vector2.Distance(transform.position, playerPosition);

                if (distanceToPlayer < RangeToFlee)
                {
                    GroundMovement.Target = FindFleeDirection(playerPosition);
                }
                else
                {
                    GroundMovement.Target = null;
                }
            }
            else
            {
                GroundMovement.Target = null;
            }
        }

        private StationaryLocation FindFleeDirection(Vector2 playerPosition)
        {
            Vector2 workerPosition = new Vector2(transform.position.x, transform.position.y);

            Vector2 direction = workerPosition - playerPosition;
            direction *= 100f;

            return new StationaryLocation(direction + workerPosition);
        }

        private void FaceSeenTarget()
        {
            if (VisionAI.SeeSomething)
            {
                _steeringComponent.AddBehavior(EBehavior.Face, new TransformLocation(VisionAI.Seen[0].transform), 1f);
            }
        }
    }
}
