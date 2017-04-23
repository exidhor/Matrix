using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Matrix
{
    public class CharacterCase
    {
        private List<Character> _characters;

        public CharacterCase()
        {
            _characters = new List<Character>();
        }

        public void Destroy()
        {
            _characters.Clear();

            //if (_ennemy != null)
            //{
            //    _ennemy.Release();
            //}
        }

        public void Set(Character character)
        {
            _characters.Add(character);
        }

        public void SetPosition(Vector2 position)
        {
            for (int i = 0; i < _characters.Count; i++)
            {
                _characters[i].transform.position = position;
            }
        }
    }
}
