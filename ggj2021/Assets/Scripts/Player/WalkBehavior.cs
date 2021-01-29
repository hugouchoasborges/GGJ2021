using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace player
{
    public class WalkBehavior : MonoBehaviour
    {
        [Header("Walk controls")]
        [SerializeField]
        [Range(0f, 10f)] private float speed = 1.0f;
        [Range(0f, 1f)] private float deadZone = 0.1f;

        [Header("Input Keys (string)")]
        [SerializeField] private string _horizontalInputKey = "Horizontal";
        [SerializeField] private string _verticalInputKey = "Vertical";

        private float _horizontalInput;
        private float _verticalInput;

        private void WalkInput()
        {

            // Horizontal movement
            _horizontalInput = Input.GetAxis(_horizontalInputKey);
            if (Math.Abs(_horizontalInput) > deadZone)
            {
                transform.position = new Vector2(
                    transform.position.x + _horizontalInput * speed * Time.deltaTime,
                    transform.position.y
                    );
            }
        }

        private void Update()
        {
            WalkInput();
        }
    }
}