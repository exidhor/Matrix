using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Matrix
{
    public class GameManager : MonoSingleton<GameManager>
    {
        public LevelEntry Entry;

        private bool _isPlaying = false;

        void Awake()
        {
            CreatePools();
        }

        void Start()
        {
            StartGame();
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
