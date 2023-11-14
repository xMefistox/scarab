using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Common
{
    public class VisualFactory 
    {
        [Inject]
        protected List<VisualSpawner> _visualSpawners = new List<VisualSpawner>();

        public VisualFactory(List<VisualSpawner> visualSpawners)
        {
            _visualSpawners = visualSpawners;
        }

        public virtual MonoBehaviour Spawn(MonoBehaviour prefab, Transform parent)
        {
            MonoBehaviour obj = _visualSpawners.Find(spawner => spawner.Prefab == prefab).Spawn();
            obj.transform.SetParent(parent, false);
            return obj;
        }

        public virtual void Reset()
        {
            foreach (VisualSpawner spawner in _visualSpawners)
            {
                spawner.Reset();
            }
        }
    }
}
