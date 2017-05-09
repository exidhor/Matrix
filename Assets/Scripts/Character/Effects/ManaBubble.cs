using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Matrix
{
    [RequireComponent(typeof(SteeringComponent))]
    public class ManaBubble : Effect
    {
        public static readonly int MANA_PER_BUBBLE = 5; 

        public float PauseTime;
        //public float ScaleSizeFactor;

        //public int ManaCarrying
        //{
        //    get { return _manaCarrying; }
        //}

        //private int _manaCarrying;

        private bool _pauseDone;
        private float _currentPauseTime;

        [SerializeField] private Vector2 _targetPause;
        [SerializeField] private Transform _targerReceiver;
        [SerializeField] private FlyingMovement _flyingMovement;

        private SteeringComponent _steeringComponent;

        void Awake()
        {
            _steeringComponent = GetComponent<SteeringComponent>();

            _flyingMovement = new FlyingMovement(_steeringComponent, transform);
        }

        public void SetMovement(Vector2 targetPause, Transform targetReceiver)
        {
            _targetPause = targetPause;
            _targerReceiver = targetReceiver;

            _flyingMovement.Target = new StationaryLocation(targetPause);

            _pauseDone = false;
            _currentPauseTime = 0;
        }

        //public void SetManaCarrying(int manaCarrying)
        //{
        //    _manaCarrying = manaCarrying;

        //    float localScale = ScaleSizeFactor * _manaCarrying;

        //    transform.localScale = new Vector3(localScale, localScale, 1f);
        //}

        public void Absorb()
        {
            _isDead = true;
        }

        void FixedUpdate()
        {
            _steeringComponent.ClearBehaviors();

            float deltaTime = TimeManager.Instance.fixedDeltaTime;

            if (!_pauseDone)
            {
                if (_flyingMovement.TargetReached)
                {
                    _currentPauseTime += deltaTime;

                    if (_currentPauseTime >= PauseTime)
                    {
                        _pauseDone = true;
                        _flyingMovement.Target = new TransformLocation(_targerReceiver);
                    }
                }
            }

            _flyingMovement.Actualize(1f);

            _steeringComponent.ActualizeSteering();
            _steeringComponent.ApplySteeringOnKinematic(deltaTime);
        }
    }
}