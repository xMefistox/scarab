using System.Collections;
using UnityEngine;
using UnityEngine.Pool;
using Zenject;

namespace Common
{
    public class AudioManager : MonoBehaviour, IInitializable, ILateDisposable
    {
        [field: SerializeField]
        [field: Range(0, 1)]
        public float MasterVolume { get; private set; }

        [SerializeField]
        protected AudioSource backgroundAudioSource;

        [SerializeField]
        protected AudioClip buttonClickClip;

        protected ObjectPool<AudioSource> audioSourcePool;

        protected SignalBus signalBus;

        [Inject]
        private void Construct(SignalBus signalBus)
        {
            this.signalBus = signalBus;
        }

        private void Start()
        {
            audioSourcePool = new ObjectPool<AudioSource>(
                        () => { return gameObject.AddComponent<AudioSource>(); },
                        audioSource => { SetAudioSource(audioSource); },
                        audioSource => { audioSource.enabled = false; },
                        audioSource => { Destroy(audioSource); });
        }

        public void PlayButtonClick()
        {
            PlayAudio(buttonClickClip);
        }

        public void PlayAudio(AudioClip audioClip, float delay = 0.0f)
        {
            AudioSource audioSource = audioSourcePool.Get();
            audioSource.clip = audioClip;
            audioSource.volume = MasterVolume;
            StartCoroutine(PlayAudioSourceOnceAndRelease(audioSource, delay));
        }

        public void PlayBackground(AudioClip audioClip)
        {
            backgroundAudioSource.Stop();
            backgroundAudioSource.clip = audioClip;
            backgroundAudioSource.loop = true;
            backgroundAudioSource.volume = MasterVolume;
            backgroundAudioSource.Play();
        }

        private void SetAudioSource(AudioSource audioSource)
        {
            audioSource.Stop();
            audioSource.loop = false;
            audioSource.enabled = true;
        }

        private IEnumerator PlayAudioSourceOnceAndRelease(AudioSource audioSource, float delay)
        {
            yield return new WaitForSeconds(delay);
            audioSource.Play();
            yield return new WaitWhile(() => audioSource.isPlaying);
            audioSourcePool.Release(audioSource);
        }

        public virtual void Initialize()
        {
        }

        public virtual void LateDispose()
        {
        }
    }
}