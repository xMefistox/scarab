using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Common
{
    public class VisualSpawner : Spawner<Visual>
    {
        protected DiContainer _container;
        public Visual VisualPrefab => base.Prefab as Visual;

        public VisualSpawner(Visual prefab, int initialCapacity, int maxCapacity, DiContainer container) : base(prefab, initialCapacity, maxCapacity)
        {
            _container = container;
        }

        protected override Visual CreatePooledItem()
        {
            Visual pooledVisual = _container.InstantiatePrefabForComponent<Visual>(VisualPrefab);
            pooledVisual.SetSpawner(this);
            return pooledVisual;
        }

        protected override void OnDestroyPoolObject(Visual visual)
        {
            _pooledInstances.Remove(visual);
            GameObject.Destroy(visual.gameObject);
        }

        protected override void OnTakeFromPool(Visual visual)
        {
            _pooledInstances.Add(visual);
            visual.gameObject.SetActive(true);
        }

        protected override void OnReturnedToPool(Visual visual)
        {
            _pooledInstances.Remove(visual);
            visual.gameObject.SetActive(false);
        }
    }
}
