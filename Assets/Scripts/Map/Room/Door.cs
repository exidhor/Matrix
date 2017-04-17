using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Matrix
{
    [RequireComponent(typeof(Collider2D), typeof(AnimatorComponent))]
    public class Door : Obstacle
    {
        public Tunnel Tunnel;

        public bool IsOpen = false;
        public bool IsOpenning = false;

        private Collider2D _collider;
        private AnimatorComponent _animatorComponent;

        private int _animationIndex;

        void Awake()
        {
            _collider = GetComponent<Collider2D>();
            _animatorComponent = GetComponent<AnimatorComponent>();

            _animationIndex = _animatorComponent.GetAnimationIndex(new AnimationKey("Open"));
        }

        public void SetConnectedRoomIndex(int connectedRoomIndex)
        {
            Tunnel.SetConnectedRoomIndex(connectedRoomIndex);
        }

        public void Open()
        {
            IsOpenning = true;
            _animatorComponent.SetCurrentAnimation("Open");
        }

        void Update()
        {
            if (IsOpenning && _animatorComponent.Animations[_animationIndex].IsFinished)
            {
                IsOpenning = false;
                IsOpen = true;

                _collider.isTrigger = true;

                Tunnel.IsActivate = true;
            }
        }
    }
}