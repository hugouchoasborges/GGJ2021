using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using util;

namespace player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class JumpBehavior : PlayerModule
    {
        [Header("Jump controls")]
        [SerializeField]
        [Range(1f, 10)] private float _jumpImpulse = 5f;
        public float JumpImpulse
        {
            get => _jumpImpulse;
            set => _jumpImpulse = value;
        }

        [SerializeField]
        [Range(0f, 1)] private float _glideMultiplier = 0.5f;
        [SerializeField]
        [Range(0f, 10)] private float _fallMultiplier = 2.5f;
        [SerializeField]
        [Range(0f, 10)] private float _lowJumpMultiplier = 2f;

        public bool canJump = true;
        public bool canGlide = true;

        [Header("Multiple jumps")]
        [SerializeField] int _maxJumps = 2;
        int _jumpCount = 0;
        public bool wallJumpActivated = true;

        [Header("Input Keys (string)")]
        [SerializeField] private string _jumpInputKey = "Jump";

        [Header("Components")]
        [SerializeField] private GroundChecker _groundChecker;
        [SerializeField] private WalkBehavior _walkBehavior;
        private Rigidbody2D _rigidbody2D;

        // Callbacks
        public event Action OnJump = null;
        public event Action OnGround = null;
        public event Action OnFall = null;

        private bool _isFalling = false;


        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();

            // Ground checker callbacks
            _groundChecker.onGrounded = OnGrounded;
            _groundChecker.onUngrounded = OnUngrounded;
        }

        private void JumpInput()
        {
            if (!canJump || !controller.MovementInputEnabled)
            {
                return;
            }

            var canWallJump = wallJumpActivated && _groundChecker.isTouchingWall;

            if ((_groundChecker.isGrounded || canWallJump || _jumpCount < _maxJumps) && Input.GetButtonDown(_jumpInputKey))
            {
                OnJump?.Invoke();

                // Wall jump
                if (canWallJump) {
                    GameDebug.Log("WALL JUMP", util.LogType.MovementEvents);
                    _jumpCount = 1;
                    _rigidbody2D.velocity = Vector2.zero;
                    _rigidbody2D.AddForce(
                        Vector2.up * _jumpImpulse + new Vector2(-_walkBehavior.facingSign, 0) * 5f * _jumpImpulse,
                        ForceMode2D.Impulse
                        );
                    return;
                }

                GameDebug.Log("JUMP", util.LogType.MovementEvents);
                _jumpCount++;
                _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, 0);
                _rigidbody2D.AddForce(Vector2.up * _jumpImpulse, ForceMode2D.Impulse);
            }

            if (!_isFalling && !_groundChecker.isGrounded && _rigidbody2D.velocity.y < 0)
            {
                _isFalling = true;
                OnFall?.Invoke();
            }
        }

        private void BetterJumpModifier()
        {
            if (_rigidbody2D.velocity.y < 0) // Falling
            {
                if (canGlide && Input.GetButton(_jumpInputKey)) // GLIDE
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
            _isFalling = false;

            OnGround?.Invoke();
        }

        private void OnUngrounded()
        {
            if (_jumpCount == 0) _jumpCount = 1;
        }
    }
}