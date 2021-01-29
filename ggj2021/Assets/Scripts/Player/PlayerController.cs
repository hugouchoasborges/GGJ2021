using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace player
{
    [RequireComponent(typeof(WalkBehavior))]
    public class PlayerController : MonoBehaviour
    {
        private WalkBehavior _walkBehavior;
        private RespawnBehavior _respawnBehavior;

        private void Awake()
        {
            _walkBehavior = GetComponent<WalkBehavior>();
            _respawnBehavior = GetComponent<RespawnBehavior>();
        }
    }
}