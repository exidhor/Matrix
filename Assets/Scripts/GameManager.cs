using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Matrix
{
    public class GameManager : MonoSingleton<GameManager>
    {
        void Awake()
        {
            
        }

        void Start()
        {
            StartGame();
        }

        public void StartGame()
        {
            Map.Instance.Construct();
        }
    }
}
