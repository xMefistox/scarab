using UnityEngine;

namespace Scarab.RiddleVisual
{
    public class ScarabVisualController : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Sprite inactiveSprite;
        [SerializeField] private Sprite selectedSprite;
        [SerializeField] private Sprite activeSprite;

        public void ChangeScarabVisual(ScarabVisualState state)
        {
            ChangeScarabSprite(state);
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
