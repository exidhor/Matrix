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
        private Roko _roko;
        private List<Guard> _ennemies = new List<Guard>();

        public bool PlayerIsThere
        {
            get { return _player != null; }
        }

        public bool LittleBoyIsThere
        {
            get { return _littleBoy != null; }
        }

        public bool RokoIsThere
        {
            get { return _roko != null; }
        }

        public Player GetPlayer()
        {
            return _player;
        }

        public List<Guard> GetEnnemies()
        {
            return _ennemies;
        }

        public LittleBoy GetLittleBoy()
        {
            return _littleBoy;
        }

        public Roko GetRoko()
        {
            return _roko;
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

        public void Register(Roko roko)
        {
            _roko = roko;
        }

        public void Unregister(Roko roko)
        {
            _roko = null;
        }

        public void Register(Guard guard)
        {
            for (int i = 0; i < _ennemies.Count; i++)
            {
                if (_ennemies[i] == guard)
                {
                    return;
                }
            }

            _ennemies.Add(guard);
        }

        public void Unregister(Guard guard)
        {
            _ennemies.Remove(guard);
        }
    }
}