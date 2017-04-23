using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Matrix
{
    public class EffectManager : MonoSingleton<EffectManager>
    {
        public List<EffectModel> Models;

        private List<PoolObject> _poolObjectList;

        private List<Effect> _effects;

        void Start()
        {
            _effects = new List<Effect>();

            CreateModelList();
            CreatePools();
        }

        public void Clear()
        {
            for (int i = 0; i < _effects.Count; i++)
            {
                _effects[i].Release();
            }

            _effects.Clear();
        }

        private void CreateModelList()
        {
            _poolObjectList = new List<PoolObject>();

            Models.Sort((x, y) => ((int)x.Type).CompareTo((int)y.Type));

            for (int i = 0; i < Models.Count; i++)
            {
                _poolObjectList.Add(Models[i].PoolEntry.Prefab);
            }
        }

        private void CreatePools()
        {
            for (int i = 0; i < Models.Count; i++)
            {
                if (!PoolTable.Instance.Contains(Models[i].PoolEntry))
                {
                    PoolTable.Instance.AddPool(Models[i].PoolEntry);
                }
            }
        }

        public Effect GetFreeEffect(EffectType type)
        {
            Effect effect =
                PoolTable.Instance.GetPool(GetModel(type).GetInstanceID()).GetFreeResource().GetComponent<Effect>();

            if(effect.IsManaged)
                _effects.Add(effect);

            return effect;
        }

        private PoolObject GetModel(EffectType type)
        {
            return _poolObjectList[(int)type];
        }

        void Update()
        {
            for (int i = 0; i < _effects.Count; i++)
            {
                if (_effects[i].IsDead)
                {
                    _effects[i].Release();
                    _effects.RemoveAt(i);
                    i--;
                }
            }
        }
    }
}
