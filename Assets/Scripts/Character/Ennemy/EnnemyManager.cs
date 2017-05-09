using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Matrix
{
    public class EnnemyManager : MonoSingleton<EnnemyManager>
    {
        public List<EnnemyModel> Models;

        private List<Pool> _poolList;

        private List<Character> _ennemies;

        void Start()
        {
            _ennemies = new List<Character>();

            CreatePools();
        }

        public void Clear()
        {
            _ennemies.Clear();
        }

        private void CreatePools()
        {
            _poolList = new List<Pool>();

            Models.Sort((x, y) => ((int)x.Type).CompareTo((int)y.Type));

            for (int i = 0; i < Models.Count; i++)
            {
                if (!PoolTable.Instance.Contains(Models[i].Ennemy))
                {
                     _poolList.Add(PoolTable.Instance.AddPool(Models[i].Ennemy));
                }
            }
        }

        public Character GetFreeEnnemy(EnnemyType type)
        {
            Character character = (Character)_poolList[(int) type].GetFreeResource();
            character.gameObject.SetActive(false);

            _ennemies.Add(character);

            return  character;
        }

        void Update()
        {
            for (int i = 0; i < _ennemies.Count; i++)
            {
                if (!_ennemies[i].IsAlive)
                {
                    _ennemies[i].OnDeath();
                    //_ennemies[i].ManaExpeller.Expeller();
                    _ennemies[i].Release();
                    _ennemies.RemoveAt(i);
                    i--;
                }
            }
        }

        public void RestoreEnnemy(Character ennemy)
        {
            _ennemies.Add(ennemy);
        }
    }
}
