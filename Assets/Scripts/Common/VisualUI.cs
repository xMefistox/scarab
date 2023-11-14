using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Common
{
    public abstract class VisualUI : Visual, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField]
        private Image _image;

        protected bool _isPointer;

        public void SetSprite(Sprite sprite, Color spriteHue)
        {
            _image.sprite = sprite;
            _image.color = spriteHue;
        }

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            _isPointer = true;
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            _isPointer = false;
        }
    }
}