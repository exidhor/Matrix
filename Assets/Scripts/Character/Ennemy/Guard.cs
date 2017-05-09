using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Matrix
{
    [RequireComponent(typeof(SteeringComponent), typeof(Kinematic), typeof(LifeComponent))]
    public class Guard : Character
    {
        public float RefreshTimeVision = 0.1f;
        public float MaxRange = 15f;

        [HideInInspector] public Collider2D Collider;
        [HideInInspector] public ManaExpeller ManaExpeller;

        public VisionAI VisionAI;
        public GroundMovement GroundMovement;

        private SteeringComponent _steeringComponent;
        private Kinematic _kinematic;
        private GunComponent _gunComponent;

        protected override void Awake()
        {
            base.Awake();

            Collider = GetComponent<Collider2D>();
            _steeringComponent = GetComponent<SteeringComponent>();
            _kinematic = GetComponent<Kinematic>();
            _gunComponent = GetComponent<GunComponent>();
            ManaExpeller = GetComponent<ManaExpeller>();

            VisionAI = new VisionAI(Collider, transform, true, false, RefreshTimeVision);
            GroundMovement = new GroundMovement(_steeringComponent, transform);
        }

        public override void OnDeath()
        {
            ManaExpeller.Expeller();
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

                    if (_gunComponent != null)
                    {
                        _gunComponent.Fire();
                    }
                }
            }

            _steeringComponent.ActualizeSteering();
            _steeringComponent.ApplySteeringOnKinematic(TimeManager.Instance.fixedDeltaTime);
        }

        private void SetTargetMovement()
        {
            if (QuickFindManager.Instance.PlayerIsThere)
            {
                Vector2 playerPosition = QuickFindManager.Instance.GetPlayer().transform.position;

                float distanceToPlayer = Vector2.Distance(transform.position, playerPosition);

                if (distanceToPlayer > MaxRange)
                {
                    GroundMovement.Target = new StationaryLocation(playerPosition);
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

        private void FaceSeenTarget()
        {
            if (VisionAI.SeeSomething)
            {
                _steeringComponent.AddBehavior(EBehavior.Face, new TransformLocation(VisionAI.Seen[0].transform), 1f);
            }
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

        public override string ToString()
        {
            return gameObject.name;
        }
    }
}