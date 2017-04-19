using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Matrix
{
    public class DynamicControllerManager : MonoSingleton<DynamicControllerManager>
    {
        public List<DynamicControllerModel> Models;

        private List<PoolObject> _poolObjectList;

        private List<DynamicController> _dynamicControllers;

        void Start()
        {
            CreateModelList();
            CreatePools();

            _dynamicControllers = new List<DynamicController>();
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

        public DynamicController GetFreeDynamicController(DynamicControllerType type)
        {
            DynamicController dynamicController =
                PoolTable.Instance.GetPool(GetModel(type).GetInstanceID()).GetFreeResource().GetComponent<DynamicController>();

            _dynamicControllers.Add(dynamicController);

            return dynamicController;
        }

        private PoolObject GetModel(DynamicControllerType type)
        {
            return _poolObjectList[(int)type];
        }
    }
}