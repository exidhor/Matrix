using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Matrix
{
    [RequireComponent(typeof(TalksComponent))]
    public class Roko : MonoBehaviour
    {
        [HideInInspector]
        public TalksComponent TalksComponent;

        public float Speed;

        private SpriteRenderer _renderer;

        private float _currentTime;

        private bool _isAppearing;

        void Awake()
        {
            TalksComponent = GetComponent<TalksComponent>();
            _renderer = GetComponent<SpriteRenderer>();

            _renderer.color = new Color(255, 255, 255, 0);
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

        void Update()
        {
            if (_isAppearing)
            {
                float scale = _currentTime*Speed;

                _renderer.color = Color.Lerp(new Color(255, 255, 255, 0), new Color(255, 255, 255, 255), scale);

                if (scale >= 1f)
                {
                    _isAppearing = false;
                }
                else
                {
                    _currentTime += TimeManager.Instance.menuDeltaTime;
                }
            }
        }

        public void StartAppearing()
        {
            _isAppearing = true;
        }

        public void HideSprite()
        {
            _renderer.color = new Color(0, 0, 0, 0);
            _isAppearing = false;
        }

        public void ShowSprite()
        {
            _renderer.color = new Color(255, 255, 255, 255);
            _isAppearing = false;
        }
    }
}
