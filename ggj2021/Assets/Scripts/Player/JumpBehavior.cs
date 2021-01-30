using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using util;

namespace player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class JumpBehavior : MonoBehaviour
    {
        [Header("Jump controls")]
        [SerializeField]
        [Range(1f, 10)] private float _jumpImpulse = 5f;
        [SerializeField]
        [Range(0f, 1)] private float _glideMultiplier = 0.5f;
        [SerializeField]
        [Range(0f, 10)] private float _fallMultiplier = 2.5f;
        [SerializeField]
        [Range(0f, 10)] private float _lowJumpMultiplier = 2f;

        [Header("Multiple jumps")]
        [SerializeField] int _maxJumps = 2;
        int _jumpCount = 0;

        [Header("Input Keys (string)")]
        [SerializeField] private string _jumpInputKey = "Jump";

        [Header("Components")]
        [SerializeField] private GroundChecker _groundChecker;
        [SerializeField] private WalkBehavior _walkBehavior;
        private Rigidbody2D _rigidbody2D;

        // Callbacks
        public event Action OnJump = null;
        public event Action OnGround = null;


        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();

            // Ground checker callbacks
            _groundChecker.onGrounded = OnGrounded;
            _groundChecker.onUngrounded = OnUngrounded;
        }

        private void JumpInput()
        {
            if ((_groundChecker.isGrounded || _jumpCount < _maxJumps) && Input.GetButtonDown(_jumpInputKey))
            {
                OnJump?.Invoke();

                // Wall jump
                if (_groundChecker.isTouchingWall) {
                    GameDebug.Log("WALL JUMP", util.LogType.MovementEvents);
                    _rigidbody2D.velocity = Vector2.zero;
                    _rigidbody2D.AddForce(
                        Vector2.up * _jumpImpulse + Vector2.right * -_walkBehavior.facingSign * _jumpImpulse,
                        ForceMode2D.Impulse
                        );
                    return;
                }

                GameDebug.Log("JUMP", util.LogType.MovementEvents);
                _jumpCount++;
                _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, 0);
                _rigidbody2D.AddForce(Vector2.up * _jumpImpulse, ForceMode2D.Impulse);
            }
        }

        private void BetterJumpModifier()
        {
            if (_rigidbody2D.velocity.y < 0) // Falling
            {
                if (Input.GetButton(_jumpInputKey))
                    _rigidbody2D.velocity += Vector2.up * Physics2D.gravity.y * (_glideMultiplier - 1) * Time.deltaTime;
                else
                    _rigidbody2D.velocity += Vector2.up * Physics2D.gravity.y * (_fallMultiplier - 1) * Time.deltaTime;
            }
            else if (_rigidbody2D.velocity.y > 0 && !Input.GetButton(_jumpInputKey))
            {
                _rigidbody2D.velocity += Vector2.up * Physics2D.gravity.y * (_lowJumpMultiplier - 1) * Time.deltaTime;
            }
        }

        private void Update()
        {
            JumpInput();
            BetterJumpModifier();
        }

        private void OnGrounded()
        {
            _jumpCount = 0;

            OnGround?.Invoke();
        }

        private void OnUngrounded()
        {
            if (_jumpCount == 0) _jumpCount = 1;
        }
    }
}