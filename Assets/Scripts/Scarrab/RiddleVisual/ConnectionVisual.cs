using UnityEngine;
using VolumetricLines;
using DG.Tweening;
using Zenject;
using Common;
using Scarrab.Zenject;
using Scarrab.RiddleLogic;

namespace Scarrab.RiddleVisual
{
    public class ConnectionVisual : Visual
    {
        private readonly float fixedZPosition = -0.005f;
        private readonly float lineWidth = 0.08f;
        private readonly float lineWidthDuration = 0.5f;

        public ScarrabController[] Scarrabs { get; private set; }

        [SerializeField] 
        private VolumetricLineBehavior volumetricLineBehaviour;

        private SignalBus signalBus;

        private Sequence hideSequence;

        public void SetConnection(ScarrabController firstScarrab, ScarrabController secondScarrab, SignalBus signalBus)
        {
            this.signalBus = signalBus;

            transform.DOLocalMoveZ(fixedZPosition, 0f);
            volumetricLineBehaviour.LineWidth = 0f;
            Scarrabs = new[] { firstScarrab, secondScarrab };
            volumetricLineBehaviour.StartPos = new Vector3(firstScarrab.transform.localPosition.x, firstScarrab.transform.localPosition.y, 0f);
            volumetricLineBehaviour.EndPos = new Vector3(secondScarrab.transform.localPosition.x, secondScarrab.transform.localPosition.y, 0f);
            DOTween.To(() => volumetricLineBehaviour.LineWidth, x => volumetricLineBehaviour.LineWidth = x, lineWidth, lineWidthDuration);
            signalBus.Fire(new ConnectionActivated());
        }

        public void HideConnection()
        {
            hideSequence = DOTween.Sequence();
            hideSequence.Append(DOTween.To(() => volumetricLineBehaviour.LineWidth, x => volumetricLineBehaviour.LineWidth = x, 0f, lineWidthDuration));
            hideSequence.AppendCallback(() => gameObject.SetActive(false));
            hideSequence.Play();
            signalBus.Fire(new ConnectionDectivated());
        }
    }
}