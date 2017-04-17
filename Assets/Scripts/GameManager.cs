using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Matrix
{
    public class GameManager : MonoSingleton<GameManager>
    {
        public LevelEntry Entry;

        void Awake()
        {
            
        }

        void Start()
        {
            StartGame();
        }

        public void StartGame()
        {
            LevelCreator.Instance.CreateLevel(Entry);
            Level.Instance.Construct(Entry);
            Level.Instance.ShowStartingRoom();
        }
    }
}
