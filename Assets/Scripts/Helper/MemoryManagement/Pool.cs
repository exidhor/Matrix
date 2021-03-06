﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Matrix
{
    /// <summary>
    /// Store copies of a GameObject model
    /// </summary>
    public class Pool : MonoBehaviour
    {
        // We place the object at this position
        // to avoid unnecessary collision event
        // (because Unity never disable colliders)
        public static Vector2 StoredPosition = new Vector2(1000000, 1000000);

        public List<Resource> Resources;

        private PoolObject _model;

        /// by how many the pool is increase if there is not enough place 
        private int _expandSize;

        private int _firstFreeResource;
        private int _lastBusyResource;

        /// <summary>
        /// Construct the pool and initialize it
        /// </summary>
        /// <param name="model">The model for all the instances</param>
        /// <param name="expandSize">by how many the pool is increase if there is not enough place </param>
        public void Construct(PoolObject model, uint expandSize)
        {
            _model = model;
            _expandSize = (int)expandSize;

            name = "Pool (" + _model.name + ")";

            Resources = new List<Resource>();

            _firstFreeResource = 0;
            _lastBusyResource = 0;
        }

        /// <summary>
        /// Resize the pool
        /// </summary>
        /// <param name="newSize">The new number of instance we want</param>
        public void SetSize(uint newSize)
        {
            if (newSize < Resources.Count)
            {
                Resources.RemoveRange((int)newSize - 1, Resources.Count - (int)newSize);
            }
            else
            {
                Resources.Capacity = (int)newSize;

                ExpandSize((int)newSize - Resources.Count);
            }
        }

        /// <summary>
        /// Expand the array by creating new instance
        /// </summary>
        /// <param name="numberOfElementsToAdd"></param>
        private void ExpandSize(int numberOfElementsToAdd)
        {
            for (int i = 0; i < numberOfElementsToAdd; i++)
            {
                Resources.Add(CreateResource());
                Resources.Last().PoolObject.IndexInPool = Resources.Count - 1;
            }
        }

        /// <summary>
        /// Configure a pool object to be linked to this pool
        /// </summary>
        /// <param name="poolObject"></param>
        /// <param name="go">GameObject</param>
        /// <param name="PoolIndex">The array index in the pool</param>
        //private void SetPoolObject(PoolObject poolObject, GameObject go, int PoolIndex)
        //{
        //    poolObject.GameObject = go;
        //    poolObject.IndexInPool = PoolIndex;
        //}

        /// <summary>
        /// Return a reference to an free instance and set it to "busy"
        /// </summary>
        public PoolObject GetFreeResource()
        {
            int i;
            for (i = _firstFreeResource; i < Resources.Count; i++)
            {
                if (!Resources[i].IsUsed)
                {
                    SetResourceState(true, i);

                    return Resources[i].PoolObject;
                }
            }

            // we need to create new Resources if there isnt enough place
            ExpandSize(_expandSize);

            SetResourceState(true, i);
            return Resources[i].PoolObject;
        }

        /// <summary>
        /// Actualize the state of a resource
        /// </summary>
        /// <param name="isUsed">The state</param>
        /// <param name="index">The index of the resource to set</param>
        private void SetResourceState(bool isUsed, int index)
        {
            if (isUsed)
            {
                Resources[index].IsUsed = true;
                Resources[index].PoolObject.gameObject.SetActive(true);
                
                Resources[index].PoolObject.OnPoolExit();

                _firstFreeResource = index + 1;

                if (_lastBusyResource < index)
                {
                    _lastBusyResource = index;
                }
            }
            else
            {
                Resources[index].IsUsed = false;
                Resources[index].PoolObject.gameObject.SetActive(false);
                Resources[index].PoolObject.transform.position = StoredPosition;
                Resources[index].PoolObject.transform.SetParent(transform);

                Resources[index].PoolObject.OnPoolEnter();

                if (_firstFreeResource > index)
                {
                    _firstFreeResource = index;
                }
            }
        }

        /// <summary>
        /// Try to release the resource given
        /// </summary>
        /// <param name="poolObject">The PoolObject which contains the resource</param>
        public void ReleaseResource(PoolObject poolObject)
        {
            //if (poolObject.IndexInPool < 0 || poolObject.IndexInPool >= Resources.Count)
            //{
            //    Debug.LogError("bad index : " + poolObject.IndexInPool);
            //}

            if (Resources[poolObject.IndexInPool].PoolObject == poolObject)
            {
                if (Resources[poolObject.IndexInPool].IsUsed)
                {
                    SetResourceState(false, poolObject.IndexInPool);

                    //poolObject.GameObject = null;
                    //poolObject.IndexInPool = -1;
                }
                else
                {
                    Debug.LogError("Try to destroy an unused object in the " + name + ".\n"
                        + "It may be an error in the poolInstanceId (" + poolObject.IndexInPool + ")");
                }
            }
            else
            {
                Debug.LogError("Try to destroy a different object in the " + name + ".\n"
                               + "It may be an error in the poolInstanceId (" + poolObject.IndexInPool + ")");
            }
        }

        /// <summary>
        /// Create a new resource into the pool
        /// </summary>
        /// <returns></returns>
        private Resource CreateResource()
        {
            PoolObject newPoolObject = Instantiate<PoolObject>(_model);
            newPoolObject.transform.SetParent(gameObject.transform);
            newPoolObject.name = _model.name + " " + Resources.Count;

            newPoolObject.Pool = this;
            newPoolObject.gameObject.SetActive(false);

            newPoolObject.transform.position = StoredPosition;

            return new Resource(newPoolObject);
        }
    }
}