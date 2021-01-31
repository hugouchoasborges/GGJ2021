using spine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace player
{
    [RequireComponent(typeof(WalkBehavior))]
    public class PlayerController : ReferenceSingleton<PlayerController>
    {
        private WalkBehavior _walkBehavior;
        private JumpBehavior _jumpBehavior;
        private RespawnBehavior _respawnBehavior;

        [SerializeField] SpineController _spineController;
        [SerializeField] InteractionBehaviour interaction;
        [SerializeField] SpeechBubble speechBubble;
        [SerializeField] StateSettings[] stateSettings;

        [Header("Runtime")]
        [SerializeField] PlayerState currentState;

        public enum PlayerState
        {
            Depressed,
            Neutral,
            Happy
        }

        [System.Serializable]
        public struct StateSettings
        {
            public PlayerState state;
            public string skinName;
            public string idleAnimation;
            public string walkAnimation;
            [Range(0f, 10f)] public float walkSpeed;
        }

        public SpineController spineController => _spineController;
        public bool MovementInputEnabled => !interaction.UsingInput;
        public StateSettings Settings => stateSettings[(int)currentState];

        private void Awake()
        {
            _walkBehavior = GetComponent<WalkBehavior>();
            _jumpBehavior = GetComponent<JumpBehavior>();
            _respawnBehavior = GetComponent<RespawnBehavior>();
            SetState(PlayerState.Depressed);
        }

        // ========================== Walk Events ============================
        private void OnStartWalking()
        {
            _spineController.PlayAnimation(Settings.walkAnimation);
        }

        private void OnStopWalking()
        {
            _spineController.PlayAnimation(Settings.idleAnimation);
        }

        private void OnAlmostFalling() 
        {
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
            _spineController.AddAnimation(Settings.idleAnimation, true, 0.2f);
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


        // ----------------------------------------------------------------------------------
        // ========================== Speech Bubble test methods ============================
        // ----------------------------------------------------------------------------------

        public void TEST_ShowSpeechBubble()
        {
            speechBubble.Appear();
        }

        public void TEST_HideSpeechBubble()
        {
            speechBubble.Close();
        }
    
        public void SetState(PlayerState state)
        {
            currentState = state;
            spineController.SetSkin(Settings.skinName);
            spineController.PlayAnimation(_walkBehavior.Walking ? Settings.walkAnimation : Settings.idleAnimation);
        }



        // Collected Colors
        List<InteractableObject.ColorType> collectedColors = new List<InteractableObject.ColorType>();

        public void PickUpObject(InteractableObject.ColorType color) {
            if (!collectedColors.Contains(color)) collectedColors.Add(color);
        }
    }
}