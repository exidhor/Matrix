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
        private List<TravolatorObject> _travolatorObjects;

        private Pool _travolatorSegmentPool;
        private List<Pool> _travolatorObjectPools;

        private Direction _direction;

        private bool _isActive;
        private bool _isInitialized;
        private bool _isPaused;

        private Vector2 _velocity;
        private float _speed;
        private float _pauseTime;
        private float _currentTime;

        private float _objectRate;

        void Awake()
        {
            _path = new List<Transform>();

            _travolatorSegments = new List<TravolatorSegment>();

            _travolatorObjects = new List<TravolatorObject>();
        }

        public override void OnPoolEnter()
        {
            base.OnPoolEnter();

            for (int i = 0; i < _travolatorSegments.Count; i++)
            {
                _travolatorSegments[i].Release();
            }

            _travolatorSegments.Clear();

            for (int i = 0; i < _travolatorObjects.Count; i++)
            {
                _travolatorObjects[i].Release();
            }

            _travolatorObjects.Clear();
        }

        public override void OnPoolExit()
        {
            base.OnPoolEnter();

            _isInitialized = false;

            _path.Clear();

            transform.position = Vector3.zero;
        }

        public void SetPools(Pool travolatorSegmentPool, List<Pool> travolatorObjectPools)
        {
            _travolatorSegmentPool = travolatorSegmentPool;
            _travolatorObjectPools = travolatorObjectPools;
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

        public void SetObjectRate(float objectRate)
        {
            _objectRate = objectRate;
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
                TravolatorSegment travolatorSegment = (TravolatorSegment) _travolatorSegmentPool.GetFreeResource();
                travolatorSegment.transform.parent = transform;
                travolatorSegment.transform.position = _path[i].position;
                travolatorSegment.transform.rotation = _path[lastIndex].rotation;

                RandomCarryingObject(travolatorSegment);

                _travolatorSegments.Add(travolatorSegment);
            }

            _velocity = MathHelper.GetDirection(_path[0].position, _path[1].position);
            _velocity.Normalize();
            _velocity *= _speed;

            _isInitialized = true;
        }

        private void Move()
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
                }
            }
            if(!_isPaused)
            {
                Move();

                Vector2 nextPosition = _travolatorSegments[0].transform.position + new Vector3(_velocity.x, _velocity.y) * deltaTime;

                switch (_direction)
                {
                    case Direction.Left:
                        if (nextPosition.x < _path[1].position.x)
                        {
                            PauseTravolator();
                        }
                        break;

                    case Direction.Up:
                        if (nextPosition.y > _path[1].position.y)
                        {
                            PauseTravolator();
                        }
                        break;

                    case Direction.Right:
                        if (nextPosition.x > _path[1].position.x)
                        {
                            PauseTravolator();
                        }
                        break;

                    case Direction.Down:
                        if (nextPosition.y < _path[1].position.y)
                        {
                            PauseTravolator();
                        }
                        break;
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

            if (_travolatorSegments[0].TravolatorObject != null)
            {
                _travolatorSegments[0].TravolatorObject.Release();
            }

            //_travolatorSegments[0].SetTravolatorObject(GetRandomTravolatorObject());
            RandomCarryingObject(_travolatorSegments[0]);
        }

        private TravolatorObject GetRandomTravolatorObject()
        {
            int random = RandomGenerator.Instance.NextInt(_travolatorObjectPools.Count);

            return (TravolatorObject) _travolatorObjectPools[random].GetFreeResource();
        }

        void Update()
        {
            if (_isActive)
            {
                if (!_isInitialized)
                {
                    ConstructTravolator();
                }

                HandleMovement(TimeManager.Instance.deltaTime);
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

        public void RandomCarryingObject(TravolatorSegment segment)
        {
            float rate = RandomGenerator.Instance.NextFloat(1f);

            if (rate < _objectRate)
            {
                segment.SetTravolatorObject(GetRandomTravolatorObject());
            }
            else
            {
                segment.SetTravolatorObject(null);   
            }
        }
    }
}