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

        [Header("Input Keys (string)")]
        [SerializeField] private string _jumpInputKey = "Jump";

        [Header("Components")]
        [SerializeField] private GroundChecker _groundChecker;
        private Rigidbody2D _rigidbody2D;

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void JumpInput()
        {
            if (_groundChecker.isGrounded && Input.GetButtonDown(_jumpInputKey))
            {
                GameDebug.Log("JUMP", util.LogType.Player);

                _rigidbody2D.AddForce(Vector2.up * _jumpImpulse, ForceMode2D.Impulse);
            }
        }

        private void BetterJumpModifier()
        {
            if (_rigidbody2D.velocity.y < 0) // Falling
            {
                if(Input.GetButton(_jumpInputKey))
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
    }
}