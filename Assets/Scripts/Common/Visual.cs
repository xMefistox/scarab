using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

namespace Common
{
    public abstract class Visual : SerializedMonoBehaviour
    {
        public Entity Owner { get; private set; }
        protected Vector3 _originalPosition;
        protected VisualSpawner _spawner;

        protected Tweener punchTween;

        protected virtual void Awake()
        {
            //punchTween = transform.DOPunchScale(Vector3.one * 0.5f, 0.5f).SetAutoKill(false);
        }

        public virtual void SetOwner(Entity entityOwner)
        {
            Owner = entityOwner;
        }

        public virtual void SetSpawner(VisualSpawner spawner)
        {
            _spawner = spawner;
        }

        public void SetOriginalPosition(Vector3 originalPosition)
        {
            _originalPosition = originalPosition;
        }

        public virtual void PlaySpawnAnimation()
        {}

        public virtual void PlayPunchAnimation()
        {
            punchTween.Restart();
        }

        public virtual void Release()
        {
            _spawner.Release(this);
        }
    }
}