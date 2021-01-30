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
            _spineController.PlayAnimation("walk", true);
        }

        private void OnStopWalking()
        {
            _spineController.PlayAnimation("idle", true);
        }

        private void OnAlmostFalling() {
            _spineController.PlayAnimation("almost_falling", true);
        }

        // ========================== Jump Events ============================

        private void OnJump()
        {
            _spineController.PlayAnimation("fly", true);
        }

        private void OnGround()
        {
            _spineController.PlayAnimation("idle", true);
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
        }

        private void OnDisable()
        {
            _walkBehavior.onStartWalking -= OnStartWalking;
            _walkBehavior.onStopWalking -= OnStopWalking;

            _walkBehavior.onAlmostFalling -= OnAlmostFalling;

            _jumpBehavior.OnJump -= OnJump;
            _jumpBehavior.OnGround -= OnGround;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.O)) speechBubble.Appear();
            if (Input.GetKeyDown(KeyCode.P)) speechBubble.Hide();
        }
    }
}