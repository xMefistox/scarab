using System;
using UnityEngine;

namespace Common
{
    public abstract class Entity
    {
        public Visual VisualInstance { get; protected set; }

        public virtual void SetVisual(Visual visualInstance, Transform parent, Vector3 localPosition)
        {
            VisualInstance = visualInstance;
            VisualInstance.SetOwner(this);
            visualInstance.transform.SetParent(parent, false);
            visualInstance.transform.localPosition = localPosition;
            visualInstance.gameObject.SetActive(true);
        }
    }
}