using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LV
{
    public class InputHandler : MonoBehaviour
    {       
        public float mouseX;
        public float mouseY;
        public float horizontal;
        public float vertical;
        public float moveAmount;
        public bool interactFlag;
        private PlayerControls inputActions;       
        
        private Vector2 cameraInput;
        private Vector2 movementInput;

        public void OnEnable()
        {
            if (inputActions == null)
            {
                Cursor.visible = false;
                inputActions = new PlayerControls();
                inputActions.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
                inputActions.PlayerMovement.Movement.performed += inputActions => movementInput = inputActions.ReadValue<Vector2>();           
            }
            inputActions.Enable();
        }

        private void OnDisable()
        {
            inputActions.Disable();
        }

        public void TickInput(float delta)
        {
            MoveInput();
            InteractInput();
        }

        private void MoveInput()
        {
            horizontal = movementInput.x;
            vertical = movementInput.y;
            moveAmount = horizontal + vertical;
            mouseX = cameraInput.x;
            mouseY = cameraInput.y;
        }

        private void InteractInput()
        {
            if (inputActions.PlayerActions.Interact.triggered)
            {
                interactFlag = true;
            }
            else
            {
                interactFlag = false;
            }
        }
    }
}