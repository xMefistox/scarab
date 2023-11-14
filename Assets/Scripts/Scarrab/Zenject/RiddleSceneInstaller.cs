using Common;
using Scarrab.RiddleVisual;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Scarrab.Zenject
{
    public class RiddleSceneInstaller : MonoInstaller
    {
        [field: SerializeField]
        private ConnectionVisual connectionVisualPrefab;

        public override void InstallBindings()
        {
            List<VisualSpawner> visualSpawners = new();
            visualSpawners.Add(new VisualSpawner(connectionVisualPrefab, 45, 100, Container));
            Container.BindInstance(new VisualFactory(visualSpawners)).AsSingle().NonLazy();
        }
    }
}
