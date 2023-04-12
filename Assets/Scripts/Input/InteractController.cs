using System;
using UnityEngine;

namespace Scarab.Input
{
    public class InteractController : MonoBehaviour
    {
        [SerializeField] Camera cameraToRaycast;
        [SerializeField] Vector2 raycastPosition;
        [SerializeField] private string scarabLayerString = "Scarab";
        [SerializeField] private string resetButtonLayerString = "Reset";

        public event Action<GameObject> OnScarabClicked;
        public event Action OnResetButtonClicked;

        private RaycastHit hit;
        private LayerMask scarabMask;
        private LayerMask resetButtonMask;
        private float maxDistance = 1000f;

        private void Awake()
        {
            scarabMask = LayerMask.GetMask(scarabLayerString);
            resetButtonMask = LayerMask.GetMask(resetButtonLayerString);
        }

        internal void TryInteracting()
        {
            if (Physics.Raycast(cameraToRaycast.ViewportPointToRay(raycastPosition), out hit, maxDistance, scarabMask + resetButtonMask))
            {
                LayerMask layerHit = hit.transform.gameObject.layer;
                if (layerHit == LayerMask.NameToLayer(scarabLayerString))
                {
                    OnScarabClicked?.Invoke(hit.transform.gameObject);
                }
                else if (layerHit == LayerMask.NameToLayer(resetButtonLayerString))
                {
                    OnResetButtonClicked?.Invoke();
                }
            }
        }
    }
}
