using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

namespace Scarab.Audio
{
    public class AudioManager : MonoBehaviour
    {
        public static event Action OnScarabStateChanged;
        public static event Action OnConnectionActivated;
        public static event Action OnConnectionDectivated;
        public static event Action OnRiddleSolved;

        [SerializeField] private AudioClip scarabChangeStateClip;
        [SerializeField] private AudioClip ConnectionActivatedClip;
        [SerializeField] private AudioClip ConnectionDeactivatedClip;
        [SerializeField] private AudioClip RiddleSolvedClip;

        private ObjectPool<AudioSource> audioSourcePool;

        private void Awake()
        {
            audioSourcePool = new ObjectPool<AudioSource>(
                () => { return gameObject.AddComponent<AudioSource>(); },
                audioSource => { SetAudioSource(audioSource); },
                audioSource => { audioSource.enabled = false; },
                audioSource => { Destroy(audioSource); });


            OnScarabStateChanged += delegate { PlayAudio(scarabChangeStateClip); };
            OnConnectionActivated += delegate { PlayAudio(ConnectionActivatedClip); };
            OnConnectionDectivated += delegate { PlayAudio(ConnectionDeactivatedClip); };
            OnRiddleSolved += delegate { PlayAudio(RiddleSolvedClip); };

        }

        private void OnDestroy()
        {
            OnScarabStateChanged -= delegate { PlayAudio(scarabChangeStateClip); };
            OnConnectionActivated -= delegate { PlayAudio(ConnectionActivatedClip); };
            OnConnectionDectivated -= delegate { PlayAudio(ConnectionDeactivatedClip); };
            OnRiddleSolved -= delegate { PlayAudio(RiddleSolvedClip); };
        }

        private void PlayAudio(AudioClip audioClip)
        {
            AudioSource audioSource = audioSourcePool.Get();
            audioSource.clip = audioClip;
            StartCoroutine(PlayAudioSourceOnceAndRelease(audioSource));
        }

        public static void ScarabStateChanged()
        {
            OnScarabStateChanged.Invoke();
        }

        public static void ConnectionActivated()
        {
            OnConnectionActivated.Invoke();
        }

        public static void ConnectionDectivated()
        {
            OnConnectionDectivated.Invoke();
        }

        public static void RiddleSolved()
        {
            OnRiddleSolved.Invoke();
        }

        private void SetAudioSource(AudioSource audioSource)
        {
            audioSource.Stop();
            audioSource.loop = false;
            audioSource.enabled = true;
        }

        private IEnumerator PlayAudioSourceOnceAndRelease(AudioSource audioSource)
        {
            audioSource.Play();
            yield return new WaitWhile(() => audioSource.isPlaying);
            audioSourcePool.Release(audioSource);
        }
    }
}
