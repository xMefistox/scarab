using System;
using UnityEngine;
using Zenject;
using Scarrab.Zenject;

namespace Scarrab.Input
{
    public class InteractController : MonoBehaviour
    {
        [SerializeField] Camera cameraToRaycast;
        [SerializeField] Vector2 raycastPosition;
        [SerializeField] private string ScarrabLayerString = "Scarrab";
        [SerializeField] private string resetButtonLayerString = "Reset";

        [Inject]
        private SignalBus signalBus;

        private RaycastHit hit;
        private LayerMask ScarrabMask;
        private LayerMask resetButtonMask;
        private float maxDistance = 1000f;

        private void Awake()
        {
            ScarrabMask = LayerMask.GetMask(ScarrabLayerString);
            resetButtonMask = LayerMask.GetMask(resetButtonLayerString);
        }

        internal void TryInteracting()
        {
            if (Physics.Raycast(cameraToRaycast.ViewportPointToRay(raycastPosition), out hit, maxDistance, ScarrabMask + resetButtonMask))
            {
                LayerMask layerHit = hit.transform.gameObject.layer;
                if (layerHit == LayerMask.NameToLayer(ScarrabLayerString))
                {
                    signalBus.Fire(new ScarrabClicked(hit.transform.gameObject));
                }
                else if (layerHit == LayerMask.NameToLayer(resetButtonLayerString))
                {
                    signalBus.Fire(new ResetButtonClicked());
                }
            }
        }
    }
}
