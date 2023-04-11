using UnityEngine;

namespace Scarab.Input
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Camera cameraToControl;
        [SerializeField] private float cameraVerticalAngleBound = 90;
        [SerializeField] private float mouseSensitivity = 1f;

        private float mouseX;
        private float mouseY;
        private float verticalRotation;

        internal void UpdateCamera(Vector2 mouseVector)
        {
            mouseX = mouseVector.x * mouseSensitivity;
            mouseY = mouseVector.y * mouseSensitivity;

            transform.Rotate(Vector3.up, mouseX);

            verticalRotation -= mouseY;
            verticalRotation = Mathf.Clamp(verticalRotation, -cameraVerticalAngleBound, cameraVerticalAngleBound);
            Vector3 targetRotation = transform.eulerAngles;
            targetRotation.x = verticalRotation;
            cameraToControl.transform.eulerAngles = targetRotation;
        }
    }
}