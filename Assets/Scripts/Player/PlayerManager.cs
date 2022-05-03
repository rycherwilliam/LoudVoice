using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LV
{
    public class PlayerManager : MonoBehaviour
    {
        private InputHandler inputHandler;       
        private PlayerLocomotion playerLocomotion;
        private CameraHandler cameraHandler;
        private void Awake()
        {
            inputHandler = GetComponent<InputHandler>();
            cameraHandler = GetComponentInChildren<CameraHandler>();
            playerLocomotion = GetComponent<PlayerLocomotion>();            
            Cursor.visible = false;
        }

        private void Update()
        {
            float delta = Time.deltaTime;
            inputHandler.TickInput(delta);            
        }

        private void FixedUpdate()
        {
            float delta = Time.fixedDeltaTime;
            cameraHandler.CameraRotation(inputHandler.mouseX, inputHandler.mouseY, delta);
            playerLocomotion.PlayerMovement(inputHandler.horizontal, inputHandler.vertical);
        }
    }
}