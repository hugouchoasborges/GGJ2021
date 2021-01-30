using System;
using UnityEngine;

namespace player
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class GroundChecker : MonoBehaviour
    {
        [SerializeField] private LayerMask _whatIsGround;
        private int _whatIsGroundNumber;

        // Fall/Wall check
        public Transform[] fallPoints;
        public bool isFallingBorder {
            get {
                int count = 0;
                foreach (Transform p in fallPoints) {
                    if (Physics2D.OverlapPoint(p.position, LayerMask.GetMask("Ground", "Wall")))
                        count++;
                }
                return count == 1;
            }
        }


        private bool _isGrounded;
        public bool isGrounded
        {
            get => _isGrounded;
            private set => _isGrounded = value;
        }

        public Action<GameObject> triggerCallback = null;
        public Action onGrounded;
        public Action onUngrounded;

        private void Awake()
        {
            _whatIsGroundNumber = (int)Mathf.Log(_whatIsGround.value, 2);
        }

        // ----------------------------------------------------------------------------------
        // ========================== Trigger Detection ============================
        // ----------------------------------------------------------------------------------


        private void OnTriggerEnter2D(Collider2D other)
        {

            if (other.gameObject.layer == _whatIsGroundNumber)
            {
                _isGrounded = true;
                //triggerCallback?.Invoke(other.gameObject);
                onGrounded?.Invoke();
            }

        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.layer == _whatIsGroundNumber) {
                _isGrounded = false;
                onUngrounded?.Invoke();
            }

        }

        /*private void OnTriggerStay2D(Collider2D other)
        {
            if (_isGrounded)
                return;

            if (other.gameObject.layer == _whatIsGroundNumber)
            {
                _isGrounded = true;
                //triggerCallback?.Invoke(other.gameObject);
            }
        }*/
    }
}