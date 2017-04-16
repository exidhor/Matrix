using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Matrix
{
    [Serializable]
    public class VisionAI
    {
        public bool SeeSomething
        {
            get { return Seen.Count > 0; }    
        }

        public List<GameObject> Seen;

        public bool TargetPlayer;
        public bool TargetLittleBoy;

        private Transform _characterTransform;
        private Collider2D _characterCollider;

        private float _timeToRefresh;
        private float _currentTime;

        public VisionAI(Collider2D characterCollider, Transform characterTransform,
            bool targetPlayer, bool targetLittleBoy, float timeToRefresh)
        {
            _characterCollider = characterCollider;
            _characterTransform = characterTransform;

            TargetPlayer = targetPlayer;
            TargetLittleBoy = targetLittleBoy;

            _timeToRefresh = timeToRefresh;
            _currentTime = 0;

            Seen = new List<GameObject>();
        }

        public void TryToSeeSomething(float deltaTime)
        {
            _currentTime += deltaTime;

            bool needUpdate = false;

            if (_timeToRefresh == 0f)
            {
                needUpdate = true;
            }
            else
            {
                while (_currentTime > _timeToRefresh)
                {
                    _currentTime -= _timeToRefresh;
                    needUpdate = true;
                }
            }


            if (needUpdate)
            {
                Seen.Clear();

                if (TargetPlayer && QuickFindManager.Instance.PlayerIsThere)
                {
                    Player player = QuickFindManager.Instance.GetPlayer();

                    if (TryToSee(player.transform.position, player.Collider))
                    {
                        Seen.Add(player.gameObject);
                    }
                }

                if (TargetLittleBoy && QuickFindManager.Instance.LittleBoyIsThere)
                {
                    LittleBoy littleBoy = QuickFindManager.Instance.GetLittleBoy();

                    if (TryToSee(littleBoy.transform.position, littleBoy.Collider))
                    {
                        Seen.Add(littleBoy.gameObject);
                    }
                }
            }
            
        }

        private bool TryToSee(Vector2 targetPosition, Collider2D targetCollider)
        {
            return VisionManager.Instance.See(_characterCollider, targetCollider, _characterTransform.position,
                targetPosition);
        }
    }
}