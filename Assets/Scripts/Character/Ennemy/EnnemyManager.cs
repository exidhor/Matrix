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

        private List<Ennemy> _ennemies;

        void Start()
        {
            _ennemies = new List<Ennemy>();

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

        public Ennemy GetFreeEnnemy(EnnemyType type)
        {
            Ennemy ennemy = (Ennemy)_poolList[(int) type].GetFreeResource();
            ennemy.gameObject.SetActive(false);

            _ennemies.Add(ennemy);

            return  ennemy;
        }

        void Update()
        {
            for (int i = 0; i < _ennemies.Count; i++)
            {
                if (!_ennemies[i].IsAlive)
                {
                    _ennemies[i].Release();
                    _ennemies.RemoveAt(i);
                    i--;
                }
            }
        }

        public void RestoreEnnemy(Ennemy ennemy)
        {
            _ennemies.Add(ennemy);
        }
    }
}
