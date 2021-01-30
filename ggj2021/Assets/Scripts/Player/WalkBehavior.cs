﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using util;

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

        // Components
        [Header("Components")]
        [SerializeField] private GroundChecker _groundChecker;
        private Rigidbody2D _rigidbody2D;

        // Callbacks
        private bool _isWalking = false;
        public event Action onStartWalking;
        public event Action onStopWalking;
        public event Action onAlmostFalling;


        // ========================== Facing Left/Right ============================
        private bool _isFacingRight;
        private bool IsFacingRight
        {
            get => _isFacingRight;
            set
            {
                _isFacingRight = value;
                transform.localScale = new Vector3(
                        Math.Abs(transform.localScale.x) * (_isFacingRight ? 1 : -1),
                        transform.localScale.y,
                        1
                        );
            }
        }
        public float facingSign {
            get { return Mathf.Sign(transform.localScale.x); }
        }

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void WalkInput()
        {

            // Horizontal movement
            _horizontalInput = Input.GetAxis(_horizontalInputKey);
            if (Math.Abs(_horizontalInput) > deadZone)
            {
                //transform.position = new Vector2(
                //    transform.position.x + _horizontalInput * speed * Time.deltaTime,
                //    transform.position.y
                //    );

                _rigidbody2D.velocity = new Vector2(_horizontalInput * speed, _rigidbody2D.velocity.y);

                IsFacingRight = _horizontalInput > 0;
            }
        }

        /// <summary>
        /// Check if the player is walking on the ground and Invoke callbacks.
        /// </summary>
        private void GroundWalkingCheck()
        {
            if (!_isWalking && _groundChecker.isGrounded && Math.Abs(_rigidbody2D.velocity.x) > deadZone)
            {
                GameDebug.Log("Started Walking", util.LogType.MovementEvents);

                _isWalking = true;
                onStartWalking?.Invoke();
            }
            else if (_isWalking && (!_groundChecker.isGrounded || Math.Abs(_rigidbody2D.velocity.x) < deadZone))
            {
                GameDebug.Log("Finished Walking", util.LogType.MovementEvents);

                _isWalking = false;

                if (_groundChecker.isGrounded) {
                    onStopWalking?.Invoke();

                    if (_groundChecker.isFallingBorder) {
                        GameDebug.Log("Player is almost falling.", util.LogType.MovementEvents);
                        onAlmostFalling?.Invoke();
                    }
                }
            }
        }

        private void Update()
        {
            WalkInput();
            GroundWalkingCheck();
        }
    }
}