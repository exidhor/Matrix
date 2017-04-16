using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Matrix
{
    [RequireComponent(typeof(SteeringComponent), typeof(Selectable), typeof(Kinematic))]
    public class TestAI : MonoBehaviour
    {
        public bool IsStraightMovement;

        //   |||||||||||||||||||||||||||||||||||
        //   ||------------- tmp -------------||
        //   ||||||||||||||||||||||||||||||||||| 

        public bool _seePlayer = false;
        public List<Vector2> CurrentPath;
        public StationaryLocation Target;
        public bool TargetReached;

        //   |||||||||||||||||||||||||||||||||||

        private SteeringComponent _steeringComponent;
        private Selectable _selectable;
        private Kinematic _kinematic;
        private GunComponent _gunComponent;
        private Collider2D _collider;

        void Start()
        {
            _steeringComponent = GetComponent<SteeringComponent>();
            _selectable = GetComponent<Selectable>();
            _kinematic = GetComponent<Kinematic>();
            _gunComponent = GetComponent<GunComponent>();
            _collider = GetComponent<Collider2D>();

            // tmp 
            CurrentPath = new List<Vector2>();

            Target = new StationaryLocation(transform.position);
            TargetReached = true;
        }

        void Update()
        {
            _steeringComponent.ClearBehaviors();

            if (_selectable.IsSelected)
            {
                if (IsStraightMovement)
                {
                    HandleStraightMovement();
                }
                else
                {
                    HandlePathFinding();
                }

                HandleGun();
            }

            HandleMovement();

            if (TargetReached)
            {
                //HandleVision();
            }

            _steeringComponent.ActualizeSteering();
            _steeringComponent.ApplySteeringOnKinematic(Time.deltaTime);

            if (_seePlayer && _gunComponent != null)
            {
                _gunComponent.Fire();
            }
        }

        //private void HandleVision()
        //{
        //    Vector2 playerPosition = Player.Instance.transform.position;
        //    Vector2 objectPosition = transform.position;

        //    _seePlayer = VisionManager.Instance.See(_collider, Player.Instance.Collider, objectPosition,
        //        playerPosition);

        //    if (_seePlayer)
        //    {
        //        //_steeringComponent.ClearBehaviors();
        //        _steeringComponent.AddBehavior(EBehavior.Face, new StationaryLocation(playerPosition), 1f);
        //    }
        //}

        private void HandleStraightMovement()
        {
            if (Input.GetButtonDown("Movement"))
            {
                Debug.Log("Straight Movement");

                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                //Location target = new StationaryLocation(mousePosition);
                Target.SetPosition(mousePosition);

                //_steeringComponent.ClearBehaviors();

                //_steeringComponent.AddBehavior(EBehavior.Face, target, 0.5f);
                
            }
        }

        private void HandleMovement()
        {
            float distanceToTarget = Vector2.Distance(transform.position, Target.GetPosition());
            TargetReached = distanceToTarget < 0.2;

            if (!TargetReached)
            {
                _steeringComponent.AddBehavior(EBehavior.Arrive, Target, 1f);
            }
        }

        private void HandlePathFinding()
        {
            if (Input.GetButtonDown("Movement"))
            {
                Debug.Log("PathFinding");

                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                Node direction = Map.Instance.GetNodeAt(mousePosition);

                Debug.Log("Direction : " + direction);

                if (direction != null && !direction.IsBlocking)
                {
                    List<Node> nodePath = PathFinding.Classical_A_Star(Map.Instance.GetCurrentNodeGrid(),
                        Map.Instance.GetNodeAt(transform.position), direction, HeuristicType.OctileDistance);

                    CurrentPath.Clear();

                    for (int i = 0; i < nodePath.Count; i++)
                    {
                        CurrentPath.Add(Map.Instance.GetPositionAt(nodePath[i].Coord));
                    }

                    _steeringComponent.ClearBehaviors();
                    _steeringComponent.SteeringSpecs.PathToFollow = nodePath;
                    _steeringComponent.AddBehavior(EBehavior.PathFollowing, null, 1f);
                }
            }
        }

        private void HandleGun()
        {
            if (_gunComponent != null && Input.GetButton("Fire"))
            {
                _gunComponent.Fire();
            }
        }
    }
}