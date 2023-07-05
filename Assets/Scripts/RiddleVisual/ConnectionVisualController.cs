using UnityEngine;

namespace Scarab.RiddleVisual
{
    public class ConnectionVisualController : MonoBehaviour
    {
        public Vector3[] ScarabPositions { get; private set; }

        [SerializeField]
        private LineRenderer lineRenderer;

        public void SetConnection(Vector3 firstScarabPos, Vector3 secondScarabPos)
        {
            ScarabPositions = new[] { firstScarabPos, secondScarabPos };
            
            lineRenderer.enabled = true;
            lineRenderer.SetPositions(ScarabPositions);
        }

        public void HideConnection()
        {
            lineRenderer.SetPositions(new Vector3[] { });
            lineRenderer.enabled = false;
        }
    }
}