using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Matrix
{
    public class TravolatorController : DynamicController
    {
        private List<Transform> _path;

        private List<TravolatorSegment> _travolatorSegments;

        private Pool _travolatorPool;

        private Direction _direction;

        private bool _isActive;
        private bool _isInitialized;
        private bool _isPaused;

        private Vector2 _velocity;
        private float _speed;
        private float _pauseTime;
        private float _currentTime;

        void Awake()
        {
            _path = new List<Transform>();

            _travolatorSegments = new List<TravolatorSegment>();
        }

        public override void OnPoolEnter()
        {
            base.OnPoolEnter();

            for (int i = 0; i < _travolatorSegments.Count; i++)
            {
                _travolatorSegments[i].Release();
            }
        }

        public override void OnPoolExit()
        {
            base.OnPoolEnter();

            _isInitialized = false;

            _path.Clear();

            transform.position = Vector3.zero;

            _travolatorSegments.Clear();
        }

        public void SetPool(Pool travolatorPool)
        {
            _travolatorPool = travolatorPool;
        }

        public void Add(Transform pathNode)
        {
            _path.Add(pathNode);
        }

        public void SetDirection(Direction direction)
        {
            _direction = direction;
        }

        public void SetPauseTime(float pauseTime)
        {
            _pauseTime = pauseTime;
        }

        public void SetSpeed(float speed)
        {
            _speed = speed;
        }

        private void ConstructTravolator()
        {
            _isPaused = true;

            bool toPause = RandomGenerator.Instance.NextBool();

            if (toPause)
            {
                _currentTime = _pauseTime + 1;
            }
            else
            {
                _currentTime = RandomGenerator.Instance.NextFloat(_pauseTime);
            }

            int lastIndex = _path.Count - 1;

            for (int i = 0; i < _path.Count - 1; i++)
            {
                TravolatorSegment effect = (TravolatorSegment) _travolatorPool.GetFreeResource();
                effect.transform.parent = transform;
                effect.transform.position = _path[i].position;
                effect.transform.rotation = _path[lastIndex].rotation;

                _travolatorSegments.Add(effect);
            }

            _velocity = MathHelper.GetDirection(_path[0].position, _path[1].position);
            _velocity.Normalize();
            _velocity *= _speed;

            _isInitialized = true;
        }

        private void StartMoving()
        {
            for (int i = 0; i < _travolatorSegments.Count; i++)
            {
                _travolatorSegments[i].Rigidbody.velocity = _velocity;
            }
        }

        private void HandleMovement(float deltaTime)
        {
            if (_isPaused)
            {
                _currentTime += deltaTime;

                if (_currentTime > _pauseTime)
                {
                    _isPaused = false;
                    StartMoving();
                }
            }
            if(!_isPaused)
            {
                Vector2 nextPosition = _travolatorSegments[0].transform.position + new Vector3(_velocity.x, _velocity.y) * deltaTime;

                if (_direction == Direction.Left)
                {
                    if (nextPosition.x < _path[1].position.x)
                    {
                        PauseTravolator();
                    }
                }
                else if (_direction == Direction.Up)
                {
                    if (nextPosition.y > _path[1].position.y)
                    {
                        PauseTravolator();
                    }
                }
                else if (_direction == Direction.Right)
                {
                    if (nextPosition.x > _path[1].position.x)
                    {
                        PauseTravolator();
                    }
                }
                else if (_direction == Direction.Down)
                {
                    if (nextPosition.y < _path[1].position.y)
                    {
                        PauseTravolator();
                    }
                }
            }
        }

        private void PauseTravolator()
        {
            _isPaused = true;
            _currentTime = 0;

            for (int i = 0; i < _travolatorSegments.Count - 1; i++)
            {
                _travolatorSegments[i].Rigidbody.velocity = Vector2.zero;
                _travolatorSegments[i].transform.position = _path[i + 1].position;
            }

            _travolatorSegments.Last().transform.position = _path[0].position;
            _travolatorSegments.Last().Rigidbody.velocity = Vector2.zero;

            TravolatorSegment tmp = _travolatorSegments.Last();

            for (int i = _travolatorSegments.Count - 1; i >= 1; i--)
            {
                _travolatorSegments[i] = _travolatorSegments[i - 1];
            }

            _travolatorSegments[0] = tmp;
        }

        void Update()
        {
            if (_isActive)
            {
                if (!_isInitialized)
                {
                    ConstructTravolator();
                }

                HandleMovement(Time.deltaTime);
            }
        }

        public override void Disable()
        {
            _isActive = false;

            for (int i = 0; i < _travolatorSegments.Count; i++)
            {
                _travolatorSegments[i].gameObject.SetActive(false);
            }
        }

        public override void Activate()
        {
            _isActive = true;

            for (int i = 0; i < _travolatorSegments.Count; i++)
            {
                _travolatorSegments[i].gameObject.SetActive(true);
            }
        }
    }
}