using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Scarab.Input
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private CameraController cameraController;
        [SerializeField] private MovementController movementController;

        private InputActions inputActions;

        private Vector2 movementInput;
        private Vector2 mouseInput;

        private void Awake()
        {
            inputActions = new InputActions();

            inputActions.Player.MouseX.performed += ctx => mouseInput.x = ctx.ReadValue<float>();
            inputActions.Player.MouseY.performed += ctx => mouseInput.y = ctx.ReadValue<float>();
            inputActions.Player.LeftMouse.performed += OnLeftMouseClicked;
            inputActions.Player.Movement.performed += ctx => movementInput = ctx.ReadValue<Vector2>();
        }

        private void OnEnable()
        {
            inputActions.Player.Enable();
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void OnDisable()
        {
            inputActions.Player.Disable();
        }

        private void Update()
        {
            cameraController.UpdateCamera(mouseInput);
            movementController.Move(movementInput);
        }

        private void OnLeftMouseClicked(InputAction.CallbackContext context)
        {
            Debug.Log($"Left Mouse Clicked! {Mouse.current.position}");
        }
    }
}
