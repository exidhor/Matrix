using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Matrix
{
    public class GameManager : MonoSingleton<GameManager>
    {
        public LevelEntry Entry;

        private bool _isPlaying = false;

        public GameObject BlackScreen;
        public GameObject InGameUI;

        public bool GameIsStarted;

        public float StartPause;
        private float _currentTime;

        void Awake()
        {
            CreatePools();
        }

        void Start()
        {
            TimeManager.Instance.Pause();

            StartGame();

            Roko roko = QuickFindManager.Instance.GetRoko();
            roko.TalksComponent.StartTalks("Start", false);
            InGameUI.SetActive(false);
        }

        void Update()
        {
            if (!GameIsStarted)
            {
                _currentTime += TimeManager.Instance.menuDeltaTime;

                if (_currentTime > StartPause)
                {
                    GameIsStarted = true;

                    Roko roko = QuickFindManager.Instance.GetRoko();
                    roko.StartAppearing();
                }
            }

            if (GameIsStarted)
            {
                Roko roko = QuickFindManager.Instance.GetRoko();

                if (!roko.TalksComponent.IsTalking)
                {
                    roko.HideSprite();
                    BlackScreen.gameObject.SetActive(false);
                    InGameUI.SetActive(true);
                    TimeManager.Instance.Resume();
                }
            }
        }

        private void CreatePools()
        {
            for (int i = 0; i < Entry.WeightedRoomPatterns.Count; i++)
            {
                Entry.WeightedRoomPatterns[i].RoomPattern.CreatePools();
            }
        }

        public void StartGame()
        {
            RandomGenerator.Instance.Restart();

            if (_isPlaying)
            {
                Level.Instance.Clear();
            }

            LevelCreator.Instance.CreateLevel(Entry);
            Level.Instance.Construct(Entry);
            Level.Instance.ShowStartingRoom();

            _isPlaying = true;
        }
    }
}
