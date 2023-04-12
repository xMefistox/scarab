using DG.Tweening;
using UnityEngine;
using Scarab.Audio;

namespace Scarab.RiddleVisual
{
    public class ScarabVisualController : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Sprite inactiveSprite;
        [SerializeField] private Sprite selectedSprite;
        [SerializeField] private Sprite activeSprite;

        private Sequence changeStateSequence;
        private float shakeDuration = 0.25f;

        public void ChangeScarabVisual(ScarabVisualState state)
        {
            changeStateSequence = DOTween.Sequence();
            changeStateSequence.Append(transform.DOShakePosition(shakeDuration, strength: 0.01f, vibrato: 100));
            changeStateSequence.AppendCallback(() => transform.localPosition = Vector3.zero);
            changeStateSequence.AppendCallback(() => ChangeScarabSprite(state));
            changeStateSequence.Play();
            AudioManager.ScarabStateChanged();
        }

        private void ChangeScarabSprite(ScarabVisualState state)
        {
            switch (state)
            {
                case ScarabVisualState.Inactive:
                    spriteRenderer.sprite = inactiveSprite;
                    break;
                case ScarabVisualState.Selected:
                    spriteRenderer.sprite = selectedSprite;
                    break;
                case ScarabVisualState.Active:
                    spriteRenderer.sprite = activeSprite;
                    break;
            }
        }
    }
}
