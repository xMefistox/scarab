using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Common.UI
{
    public enum TooltipPivot
    {
        UpperLeft,
        UpperRight,
        LowerLeft,
        LowerRight,
    }

    public class TooltipUI : MonoBehaviour
    {
        public static bool IsActive { get; private set; }

        [SerializeField]
        private RectTransform _tooltipParent;
        [SerializeField]
        private TextMeshProUGUI _headerText;
        [SerializeField]
        private TextMeshProUGUI _contentText;

        private void Awake()
        {
            HideTooltip();
        }

        void Update()
        {
            if (_tooltipParent.gameObject.activeSelf)
            {
                _tooltipParent.transform.position = Mouse.current.position.ReadValue();
            }
        }

        private IEnumerator SetTooltip(string headerText, string contentText, float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            _headerText.text = headerText;
            _contentText.text = contentText;
            switch (SetPivotBasedOnMousePosition())
            {
                case TooltipPivot.UpperLeft:
                    _tooltipParent.pivot = new Vector2(0, 1);
                    break;
                case TooltipPivot.UpperRight:
                    _tooltipParent.pivot = new Vector2(1, 1);
                    break;
                case TooltipPivot.LowerLeft:
                    _tooltipParent.pivot = new Vector2(0, 0);
                    break;
                case TooltipPivot.LowerRight:
                    _tooltipParent.pivot = new Vector2(1, 0);
                    break;
            }
            _tooltipParent.gameObject.SetActive(true);
            IsActive = true;
        }

        private TooltipPivot SetPivotBasedOnMousePosition()
        {
            if (Mouse.current.position.ReadValue().x > Screen.width / 2)
            {
                if (Mouse.current.position.ReadValue().y > Screen.height / 2)
                {
                    return TooltipPivot.UpperRight;
                }
                else
                {
                    return TooltipPivot.LowerRight;
                }
            }
            else
            {
                if (Mouse.current.position.ReadValue().y > Screen.height / 2)
                {
                    return TooltipPivot.UpperLeft;
                }
                else
                {
                    return TooltipPivot.LowerLeft;
                }
            }
        }

        private void HideTooltip()
        {
            StopAllCoroutines();
            _tooltipParent.gameObject.SetActive(false);
            IsActive = false;
        }
    }
}
