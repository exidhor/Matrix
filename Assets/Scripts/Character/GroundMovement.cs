using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Matrix
{
    [Serializable]
    public class GroundMovement
    {
        public Location Target;
        public List<Vector2> DebugPath;

        public bool TargetReached
        {
            get { return _targetReached; }
        }

        private bool _targetReached;
        private SteeringComponent _steeringComponent;
        private Transform _characterTransform;

        public GroundMovement(SteeringComponent steeringComponent, Transform characterTransform)
        {
            _steeringComponent = steeringComponent;
            _characterTransform = characterTransform;

            DebugPath = new List<Vector2>();
        }

        public void Actualize(float weight)
        {
            if (Target == null)
            {
                return;
            }

            if (!TargetReached)
            {
                Node direction = Map.Instance.GetNodeAt(Target.GetPosition());

                if (direction != null && !direction.IsBlocking)
                {
                    List<Node> nodePath = PathFinding.Classical_A_Star(Map.Instance.GetCurrentNodeGrid(),
                        Map.Instance.GetNodeAt(_characterTransform.position), direction, HeuristicType.OctileDistance);

                    DebugPath.Clear();

                    for (int i = 0; i < nodePath.Count; i++)
                    {
                        DebugPath.Add(Map.Instance.GetPositionAt(nodePath[i].Coord));
                    }

                    _steeringComponent.SteeringSpecs.PathToFollow = nodePath;
                    _steeringComponent.AddBehavior(EBehavior.PathFollowing, null, 1f);
                }
            }

            CheckIfTargetIsReached();
        }

        private void CheckIfTargetIsReached()
        {
            float distanceToTarget = Vector2.Distance(_characterTransform.position, Target.GetPosition());

            _targetReached = distanceToTarget < _steeringComponent.SteeringSpecs.RadiusMarginError;
        }
    }
}
