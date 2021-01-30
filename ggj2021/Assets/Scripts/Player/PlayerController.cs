using spine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace player
{
    [RequireComponent(typeof(WalkBehavior))]
    public class PlayerController : MonoBehaviour
    {
        private WalkBehavior _walkBehavior;
        private JumpBehavior _jumpBehavior;
        private RespawnBehavior _respawnBehavior;

        [SerializeField] private SpineController _spineController;
        public SpineController spineController => _spineController;

        [SerializeField] SpeechBubble speechBubble;

        private void Awake()
        {
            _walkBehavior = GetComponent<WalkBehavior>();
            _jumpBehavior = GetComponent<JumpBehavior>();
            _respawnBehavior = GetComponent<RespawnBehavior>();
        }


        // ========================== Walk Events ============================
        private void OnStartWalking()
        {
            _spineController.PlayAnimation("walk");
        }

        private void OnStopWalking()
        {
            _spineController.PlayAnimation("idle");
        }

        private void OnAlmostFalling() {
            _spineController.PlayAnimation("almost_falling");
        }

        // ========================== Jump Events ============================

        private void OnJump()
        {
            _spineController.PlayAnimation("jump_up", false);
        }

        private void OnFall()
        {
            _spineController.PlayAnimation("jump_down", false);
        }

        private void OnGround()
        {
            _spineController.PlayAnimation("landing", false);
            _spineController.AddAnimation("idle", true, 0.2f);
        }

        // ----------------------------------------------------------------------------------
        // ========================== Enable/Disable ============================
        // ----------------------------------------------------------------------------------

        private void OnEnable()
        {
            _walkBehavior.onStartWalking += OnStartWalking;
            _walkBehavior.onStopWalking += OnStopWalking;

            _walkBehavior.onAlmostFalling += OnAlmostFalling;

            _jumpBehavior.OnJump += OnJump;
            _jumpBehavior.OnGround += OnGround;
            _jumpBehavior.OnFall += OnFall;
        }

        private void OnDisable()
        {
            _walkBehavior.onStartWalking -= OnStartWalking;
            _walkBehavior.onStopWalking -= OnStopWalking;

            _walkBehavior.onAlmostFalling -= OnAlmostFalling;

            _jumpBehavior.OnJump -= OnJump;
            _jumpBehavior.OnGround -= OnGround;
            _jumpBehavior.OnFall -= OnFall;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.O)) speechBubble.Appear();
            if (Input.GetKeyDown(KeyCode.P)) speechBubble.Hide();
        }
    }
}