using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LV
{
    public class CameraHandler : MonoBehaviour
    {
        [SerializeField] private float mouseSensitivity = 100f;
        [SerializeField] private Transform playerBody;
        private float xAxisClamp;
        private bool m_cursorIsLocked = true;

        private void Awake()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            xAxisClamp = 0.0f;
        }

        public void CameraRotation(float mouseXInput, float mouseYInput, float delta)
        {
            float mouseX = (mouseXInput * mouseSensitivity) * delta;
            float mouseY = (mouseYInput * mouseSensitivity) * delta;

            xAxisClamp += mouseY;

            if (xAxisClamp > 90.0f)
            {
                xAxisClamp = 90.0f;
                mouseY = 0.0f;
                ClampXAxisRotationToValue(270.0f);
            }
            else if (xAxisClamp < -90.0f)
            {
                xAxisClamp = -90.0f;
                mouseY = 0.0f;
                ClampXAxisRotationToValue(90.0f);
            }

            transform.Rotate(Vector3.left * mouseY);
            playerBody.Rotate(Vector3.up * mouseX);
        }

        private void ClampXAxisRotationToValue(float value)
        {
            Vector3 eulerRotation = transform.eulerAngles;
            eulerRotation.x = value;
            transform.eulerAngles = eulerRotation;
        }
    }
}