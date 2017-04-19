using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Matrix
{
    public class PoolTable : MonoSingleton<PoolTable>
    {
        private Dictionary<int, Pool> _table;

        private void Awake()
        {
            _table = new Dictionary<int, Pool>();
        }

        public Pool AddPool(PoolObject model, uint poolSize, uint expandSize = 1)
        {
            Pool newPool = InstanciatePool(model, expandSize);
            newPool.SetSize(poolSize);

            _table.Add(model.GetInstanceID(), newPool);

            return newPool;
        }

        public bool Contains(PoolEntry poolEntry)
        {
            return Contains(poolEntry.Prefab);
        }

        public bool Contains(PoolObject model)
        {
            return _table.ContainsKey(model.GetInstanceID());
        }

        public Pool AddPool(PoolEntry poolEntry)
        {
            return AddPool(poolEntry.Prefab,
                poolEntry.PoolSize,
                poolEntry.ExpandPoolSize);
        }

        public Pool GetPool(int instanceID)
        {
            return _table[instanceID];
        }

        private Pool InstanciatePool(PoolObject model, uint expandSize)
        {
            GameObject poolGameObject = new GameObject();
            poolGameObject.transform.parent = gameObject.transform;
            Pool pool = poolGameObject.AddComponent<Pool>();

            pool.transform.parent = gameObject.transform;

            pool.Construct(model, expandSize);

            return pool;
        }
    }
}