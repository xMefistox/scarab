using DG.Tweening;
using Scarrab.Zenject;
using UnityEngine;
using Zenject;

namespace Scarrab.RiddleVisual
{
    public class ScarrabVisualController : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Sprite inactiveSprite;
        [SerializeField] private Sprite selectedSprite;
        [SerializeField] private Sprite activeSprite;

        [Inject]
        private SignalBus signalBus;
        
        private Sequence changeStateSequence;
        private float shakeDuration = 0.25f;

        public void ChangeScarrabVisual(ScarrabVisualState state)
        {
            changeStateSequence = DOTween.Sequence();
            changeStateSequence.Append(transform.DOShakePosition(shakeDuration, strength: 0.01f, vibrato: 100));
            changeStateSequence.AppendCallback(() => transform.localPosition = Vector3.zero);
            changeStateSequence.AppendCallback(() => ChangeScarrabSprite(state));
            changeStateSequence.Play();
            signalBus.Fire(new ScarrabStateChanged());
        }

        private void ChangeScarrabSprite(ScarrabVisualState state)
        {
            switch (state)
            {
                case ScarrabVisualState.Inactive:
                    spriteRenderer.sprite = inactiveSprite;
                    break;
                case ScarrabVisualState.Selected:
                    spriteRenderer.sprite = selectedSprite;
                    break;
                case ScarrabVisualState.Active:
                    spriteRenderer.sprite = activeSprite;
                    break;
            }
        }
    }
}
