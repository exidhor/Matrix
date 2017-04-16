using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Matrix
{
    [Serializable]
    public class FlyingMovement
    {
        public Location Target;

        public bool TargetReached
        {
            get { return _targetReached; }
        }

        private bool _targetReached;
        private SteeringComponent _steeringComponent;
        private Transform _characterTransform;

        public FlyingMovement(SteeringComponent steeringComponent, Transform characterTransform)
        {
            _steeringComponent = steeringComponent;
            _characterTransform = characterTransform;
        }

        public void Actualize(float weight)
        {
            if (Target == null)
            {
                return;
            }

            if (!TargetReached)
            {
                _steeringComponent.AddBehavior(EBehavior.Arrive, Target, weight);
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
