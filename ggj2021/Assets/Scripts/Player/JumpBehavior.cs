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

        [Header("Input Keys (string)")]
        [SerializeField] private string _jumpInputKey = "Jump";

        private Rigidbody2D _rigidbody2D;

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void WalkInput()
        {
            if (Input.GetButtonDown(_jumpInputKey))
            {
                GameDebug.Log("JUMP", util.LogType.Player);

                _rigidbody2D.AddForce(Vector2.up * _jumpImpulse, ForceMode2D.Impulse);
            }
        }

        private void Update()
        {
            WalkInput();
        }
    }
}