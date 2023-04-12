using UnityEngine;
using VolumetricLines;
using DG.Tweening;
using Scarab.Audio;

namespace Scarab.RiddleVisual
{
    public class ConnectionVisualController : MonoBehaviour
    {
        private readonly float fixedZPosition = -0.005f;
        private readonly float lineWidth = 0.08f;
        private readonly float lineWidthDuration = 0.5f;

        public Vector3[] ScarabPositions { get; private set; }

        [SerializeField] private VolumetricLineBehavior volumetricLineBehaviour;

        private Sequence hideSequence;

        public void SetConnection(Vector3 firstScarabPos, Vector3 secondScarabPos)
        {
            transform.DOLocalMoveZ(fixedZPosition, 0f);
            volumetricLineBehaviour.LineWidth = 0f;
            ScarabPositions = new[] { firstScarabPos, secondScarabPos };
            volumetricLineBehaviour.StartPos = new Vector3(firstScarabPos.x, firstScarabPos.y, 0f);
            volumetricLineBehaviour.EndPos = new Vector3(secondScarabPos.x, secondScarabPos.y, 0f);
            DOTween.To(() => volumetricLineBehaviour.LineWidth, x => volumetricLineBehaviour.LineWidth = x, lineWidth, lineWidthDuration);
            AudioManager.ConnectionActivated();
        }

        public void HideConnection()
        {
            hideSequence = DOTween.Sequence();
            hideSequence.Append(DOTween.To(() => volumetricLineBehaviour.LineWidth, x => volumetricLineBehaviour.LineWidth = x, 0f, lineWidthDuration));
            hideSequence.AppendCallback(() => gameObject.SetActive(false));
            hideSequence.Play();
            AudioManager.ConnectionDectivated();
        }
    }
}