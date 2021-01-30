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

        [SerializeField] SpeechBubble speechBubble;

        private void Awake()
        {
            _walkBehavior = GetComponent<WalkBehavior>();
            _jumpBehavior = GetComponent<JumpBehavior>();
            _respawnBehavior = GetComponent<RespawnBehavior>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.O)) speechBubble.Appear();
            if (Input.GetKeyDown(KeyCode.P)) speechBubble.Hide();
        }
    }
}