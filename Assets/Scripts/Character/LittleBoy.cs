using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Matrix
{
    [RequireComponent(typeof(SteeringComponent), typeof(Selectable), typeof(Kinematic))]
    public class LittleBoy : Character
    {
        public float MaxDistanceToThePlayer;
        public float RefreshTimeVision = 0.1f;

        [HideInInspector]
        public Collider2D Collider;

        public VisionAI VisionAI;
        public FlyingMovement FlyingMovement;

        private SteeringComponent _steeringComponent;
        private Kinematic _kinematic;

        void Awake()
        {             
            Collider = GetComponent<Collider2D>();
            _steeringComponent = GetComponent<SteeringComponent>();
            _kinematic = GetComponent<Kinematic>();

            VisionAI = new VisionAI(Collider, transform, true, false, RefreshTimeVision);
            FlyingMovement = new FlyingMovement(_steeringComponent, transform);
        }

        public override void OnDeath()
        {
            // todo
        }

        void FixedUpdate()
        {
            _steeringComponent.ClearBehaviors();

            SetTargetMovement();
            FlyingMovement.Actualize(1f);

            if (!_kinematic.isMoving)
            {
                VisionAI.TryToSeeSomething(TimeManager.Instance.fixedDeltaTime);
                FaceSeenTarget();
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

                if (distanceToPlayer > MaxDistanceToThePlayer)
                {
                    FlyingMovement.Target = new StationaryLocation(playerPosition);
                }
                else
                {
                    FlyingMovement.Target = null;
                }
            }
            else
            {
                FlyingMovement.Target = null;
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
    }
}
