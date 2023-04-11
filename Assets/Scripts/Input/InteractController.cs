using System;
using UnityEngine;

namespace Scarab.Input
{
    public class InteractController : MonoBehaviour
    {
        [SerializeField] private string scarabLayerString = "Scarab";
        [SerializeField] private string resetButtonLayerString = "Reset";

        [SerializeField] Camera cameraToRaycast;
        [SerializeField] Vector2 raycastPosition;

        public event Action<GameObject> OnScarabClicked;
        public event Action OnResetButtonClicked;

        private RaycastHit hit;
        private LayerMask scarabMask;
        private LayerMask resetButtonMask;
        private LayerMask raycastMask;
        private float maxDistance = 1000f;

        private void Awake()
        {
            scarabMask = LayerMask.GetMask(scarabLayerString);
            resetButtonMask = LayerMask.GetMask(resetButtonLayerString);
            raycastMask += scarabMask;
            raycastMask += resetButtonMask;
        }

        internal void TryInteracting()
        {
            if (Physics.Raycast(cameraToRaycast.ViewportPointToRay(raycastPosition), out hit, maxDistance, scarabMask))
            {
/*                LayerMask layerHit = hit.transform.gameObject.layer;
                if (layerHit == scarabMask)
                {*/
                    OnScarabClicked?.Invoke(hit.transform.gameObject);
/*                }
                else
                {
                    OnResetButtonClicked?.Invoke();
                }*/
            }
        }
    }
}
