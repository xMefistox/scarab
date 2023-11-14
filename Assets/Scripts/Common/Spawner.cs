using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Pool;

namespace Common
{
    public abstract class Spawner<T> where T : MonoBehaviour
    {
        public ObjectPool<T> Pool { get; private set; }
        public T Prefab { get; private set; }
        protected List<T> _pooledInstances = new List<T>();

        public Spawner(T prefab, int initialCapacity, int maxCapacity)
        {
            Prefab = prefab;
            Pool = new ObjectPool<T>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, defaultCapacity: initialCapacity, maxSize: maxCapacity);
        }

        public T Spawn()
        {
            T obj = Pool.Get();
            obj.gameObject.SetActive(false);
            return obj;
        }

        public void Release(T instance)
        {
            Pool.Release(instance);
        }

        public virtual void Reset()
        {
            for (int i = _pooledInstances.Count - 1; i >= 0; i--)
            {
                Pool.Release(_pooledInstances[i]);
            }
        }

        protected abstract T CreatePooledItem();

        protected virtual void OnDestroyPoolObject(T poolObject)
        {
            _pooledInstances.Remove(poolObject);
            GameObject.Destroy(poolObject.gameObject);
        }

        protected virtual void OnTakeFromPool(T poolObject)
        {
            _pooledInstances.Add(poolObject);
            poolObject.gameObject.SetActive(true);
        }

        protected virtual void OnReturnedToPool(T poolObject)
        {
            _pooledInstances.Remove(poolObject);
            poolObject.gameObject.SetActive(false);
        }
    }
}
