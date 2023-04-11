using System;
using UnityEngine;

namespace Scarab.Input
{
    public class InteractController : MonoBehaviour
    {
        [SerializeField] Camera cameraToRaycast;
        [SerializeField] Vector2 raycastPosition;

        public event Action<GameObject> OnInteracted;

        private RaycastHit hit;
        private LayerMask scarabMask;
        private float maxDistance = 1000f;

        private void Awake()
        {
            scarabMask = LayerMask.GetMask("Scarab");
        }

        internal void TryInteracting()
        {
            if (Physics.Raycast(cameraToRaycast.ViewportPointToRay(raycastPosition), out hit, maxDistance, scarabMask))
            {
                OnInteracted?.Invoke(hit.transform.gameObject);
            }
        }
    }
}
