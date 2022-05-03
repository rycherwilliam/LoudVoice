using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LV
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerLocomotion : MonoBehaviour
    {
        [SerializeField] private float movementSpeed = 2f;
        private CharacterController charController;
        private void Awake()
        {
            charController = GetComponent<CharacterController>();
        }

        public void PlayerMovement(float horizontal, float vertical)
        {
            float vertInput = vertical * movementSpeed;
            float horizInput = horizontal * movementSpeed;

            Vector3 forwardMovement = transform.forward * vertInput;
            Vector3 rightMovement = transform.right * horizInput;
            charController.SimpleMove(forwardMovement + rightMovement);
        }
    }
}