using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Matrix
{
    public class QuickFindManager : MonoSingleton<QuickFindManager>
    {
        private Player _player;
        private LittleBoy _littleBoy;
        private List<Ennemy> _ennemies = new List<Ennemy>();

        public bool PlayerIsThere
        {
            get { return _player != null; }
        }

        public bool LittleBoyIsThere
        {
            get { return _littleBoy != null; }
        }

        public Player GetPlayer()
        {
            return _player;
        }

        public List<Ennemy> GetEnnemies()
        {
            return _ennemies;
        }

        public LittleBoy GetLittleBoy()
        {
            return _littleBoy;
        }

        public void Register(LittleBoy littleBoy)
        {
            _littleBoy = littleBoy;
        }

        public void Unregister(LittleBoy littleBoy)
        {
            _littleBoy = null;
        }

        public void Register(Player player)
        {
            _player = player;
        }

        public void Unregister(Player player)
        {
            _player = null;
        }

        public void Register(Ennemy ennemy)
        {
            for (int i = 0; i < _ennemies.Count; i++)
            {
                if (_ennemies[i] == ennemy)
                {
                    return;
                }
            }

            _ennemies.Add(ennemy);
        }

        public void Unregister(Ennemy ennemy)
        {
            _ennemies.Remove(ennemy);
        }
    }
}