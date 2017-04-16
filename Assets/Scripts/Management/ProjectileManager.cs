using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Matrix
{
    public class ProjectileManager : MonoSingleton<ProjectileManager>
    {
        public List<ProjectileModel> Models;

        private List<PoolObject> _poolObjectList;

        private List<Projectile> _projectiles;

        void Start()
        {
            _projectiles = new List<Projectile>();

            CreateModelList();
            CreatePools();
        }

        private void CreateModelList()
        {
            _poolObjectList = new List<PoolObject>();

            Models.Sort((x, y) => ((int) x.Type).CompareTo((int) y.Type));

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

        public Projectile GetFreeProjectile(ProjectileType type)
        {
            Projectile projectile =
                PoolTable.Instance.GetPool(GetModel(type).GetInstanceID()).GetFreeResource().GetComponent<Projectile>();

            _projectiles.Add(projectile);

            return projectile;
        }

        private PoolObject GetModel(ProjectileType type)
        {
            return _poolObjectList[(int) type];
        }

        void Update()
        {
            for (int i = 0; i < _projectiles.Count; i++)
            {
                if (_projectiles[i].IsDead)
                {
                    _projectiles[i].Release();
                    _projectiles.RemoveAt(i);
                    i--;
                }
            }
        }
    }
}