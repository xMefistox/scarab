using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;
using Common;
using Zenject;
using Scarrab.Zenject;

namespace Scarrab.Audio
{
    public class ScarrabAudioManager : AudioManager
    {
        [SerializeField] private AudioClip ScarrabChangeStateClip;
        [SerializeField] private AudioClip ConnectionActivatedClip;
        [SerializeField] private AudioClip ConnectionDeactivatedClip;
        [SerializeField] private AudioClip RiddleSolvedClip;

        public override void Initialize()
        {
            signalBus.Subscribe<ConnectionActivated>(OnConnectionActivated);
            signalBus.Subscribe<ConnectionDectivated>(OnConnectionDectivated);
            signalBus.Subscribe<RiddleSolved>(OnRiddleSolved);
            signalBus.Subscribe<ScarrabStateChanged>(OnScarrabStateChanged);
        }

        public override void LateDispose()
        {
            signalBus.Unsubscribe<ConnectionActivated>(OnConnectionActivated);
            signalBus.Unsubscribe<ConnectionDectivated>(OnConnectionDectivated);
            signalBus.Unsubscribe<RiddleSolved>(OnRiddleSolved);
            signalBus.Unsubscribe<ScarrabStateChanged>(OnScarrabStateChanged);
        }

        private void OnScarrabStateChanged()
        {
            PlayAudio(ScarrabChangeStateClip);
        }

        private void OnRiddleSolved()
        {
            PlayAudio(RiddleSolvedClip);
        }

        private void OnConnectionDectivated()
        {
            PlayAudio(ConnectionDeactivatedClip);
        }

        private void OnConnectionActivated()
        {
            PlayAudio(ConnectionActivatedClip);
        }
    }
}
