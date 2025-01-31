﻿using spine;
using ui;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using sound;

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
        [SerializeField] AuraBehaviour aura;
        [SerializeField] SpeechBubble speechBubble;
        [SerializeField] StateSettings[] stateSettings;
        [SerializeField] int[] colorPieceTotals;

        [Header("Runtime")]
        [SerializeField] PlayerState currentState;
        [SerializeField] int[] colorPieces;

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

            [Header("Sound")]
            public string soundName;

            [Header("Animations")]
            public string skinName;
            public string idleAnimation;
            public string walkAnimation;

            [Header("Blend")]
            public float innerGsBlend;
            public float outerGsBlend;

            [Header("Jump")]
            [Range(1f, 10)] public float jumpImpulse;
            public bool jump;
            public bool glide;

            [Header("Walk")]
            [Range(0f, 10f)] public float walkSpeed;
        }

        public SpineController spineController => _spineController;
        public bool MovementInputEnabled => !interaction.UsingInput;
        public StateSettings Settings => stateSettings[(int)currentState];
        public List<InteractableObject.ColorType> collectedColors = new List<InteractableObject.ColorType>();

        public delegate void Event();
        public Event OnColorAcquire;
        public Event OnGameEnd;

        protected override void Awake()
        {
            base.Awake();
            _walkBehavior = GetComponent<WalkBehavior>();
            _jumpBehavior = GetComponent<JumpBehavior>();
            _respawnBehavior = GetComponent<RespawnBehavior>();
            colorPieces = new int[3];
        }

        private void Start()
        {
            SetState(PlayerState.Depressed, false);
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

        public void SetMovementLock(bool state) {
            _walkBehavior.lockMovement = state;
            _jumpBehavior.lockMovement = state;
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
        // ========================== Player states ============================
        // ----------------------------------------------------------------------------------


        public void SetState(PlayerState state, bool useTransitions = true)
        {
            currentState = state;
            spineController.SetSkin(Settings.skinName);
            spineController.PlayAnimation(_walkBehavior.Walking ? Settings.walkAnimation : Settings.idleAnimation);
            aura.UpdateBlendValues(Settings.innerGsBlend, Settings.outerGsBlend, useTransitions);

            _jumpBehavior.canJump = Settings.jump;
            _jumpBehavior.canGlide = Settings.glide;
            _jumpBehavior.JumpImpulse = Settings.jumpImpulse;
        }
        public void AdvanceState()
        {
            GameController.Instance.uiController.UpdateHeartPiecesCollected((int)currentState + 1);

            if (currentState == PlayerState.Happy)
            {
                Debug.Log("YOU BEAT THE GAME!!!");
                GameController.Instance.EndGameSequence();
                return;
            }

            SoundController.FadeOut(Settings.soundName, duration: 1.0f, callback: () =>
            {
                SoundController.FadeIn(Settings.soundName, duration: 1.0f); ;
                SoundController.Play(Settings.soundName, true);
            });

            SetState(currentState + 1);
        }

        public void PickColorPiece(InteractableObject.ColorType color)
        {
            var index = (int)color;
            colorPieces[index]++;
            GameController.Instance.uiController.UpdateColorOutputs(colorPieces, colorPieceTotals);
            if (colorPieces[index] >= colorPieceTotals[index] && !collectedColors.Contains(color))
            {
                collectedColors.Add(color);
                OnColorAcquire?.Invoke();
            }

            switch (color)
            {
                case InteractableObject.ColorType.PURPLE:
                    // All purple items should have color on and be interactable
                    GameController.Instance.ActivatePurple();
                    break;
                case InteractableObject.ColorType.RED:
                    // All red items should have color on and be interactable
                    GameController.Instance.ActivateRed();
                    break;
            }
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
    }
}