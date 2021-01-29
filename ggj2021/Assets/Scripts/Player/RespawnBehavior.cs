using UnityEngine;
using util;

namespace player
{
    [RequireComponent(typeof(Transform))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class RespawnBehavior : MonoBehaviour
    {
        [SerializeField]
        [Range(-20f, -5f)] private float _heightRespawn = -5f;

        [SerializeField]
        private Transform _respawnPosition;

        private Rigidbody2D _rigidbody2D;

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }


        private void CheckForRespawn()
        {
            if (transform.position.y <= _heightRespawn)
                Respawn();
        }

        private void Respawn()
        {
            GameDebug.Log("RESPAWN", util.LogType.Player);

            transform.position = _respawnPosition.position;
            _rigidbody2D.velocity = Vector2.zero;
        }

        private void Update()
        {
            CheckForRespawn();
        }
    }
}