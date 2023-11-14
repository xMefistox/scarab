using Common;
using UnityEngine;
using Zenject;

namespace Scarrab.Zenject
{
    public class ScarrabProjectInstaller : MonoInstaller
    {
        [SerializeField]
        private AudioManager audioManager;

        public override void InstallBindings()
        {
            InstallSignals();

            Container.BindInterfacesAndSelfTo<AudioManager>().FromInstance(audioManager).AsSingle().NonLazy();
        }

        private void InstallSignals()
        {
            SignalBusInstaller.Install(Container);

            Container.DeclareSignal<ScarrabStateChanged>();
            Container.DeclareSignal<ConnectionActivated>();
            Container.DeclareSignal<ConnectionDectivated>();
            Container.DeclareSignal<RiddleSolved>();
            Container.DeclareSignal<ScarrabClicked>();
            Container.DeclareSignal<ResetButtonClicked>();
        }
    }
}

