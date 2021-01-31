using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace camera 
{
    [RequireComponent(typeof(Camera))]
    public class CameraController : MonoBehaviour
    {
        public Transform follow;
        public float velocity;
        [SerializeField] ParalaxBackground[] backgrounds;
        private Camera cam;

        [HideInInspector] public Vector3 initialOffset;
        private float lastScaleX = 1;

        [Header("Parallax")]
        public bool parallaxOn = false;

        [Header("Locking")]
        public bool fullLock = false;
        public bool lockY = true;

        [Header("Limiting")]
        public bool maxX = true;
        public float maxXValue = 64.46f;
        public bool minX = false;
        public float minXValue = 0;

        [Header("Zoom Control")]
        [Range(0f, 10)] public float targetZoom;
        public float zoomVelocity = 1;
        private float initialZoomSize;

        private void Awake() {
            initialOffset = transform.position - follow.position;
            initialOffset.z = 0;
            lastScaleX = Mathf.Sign(transform.localScale.x);

            cam = GetComponent<Camera>();
            initialZoomSize = targetZoom = cam.orthographicSize;

            backgrounds = FindObjectsOfType<ParalaxBackground>();
        }

        void FixedUpdate()
        {
            PositionUpdate();
            ZoomUpdate();
        }

        private void PositionUpdate() {
            if (fullLock) return;

            if (Mathf.Sign(follow.localScale.x) != lastScaleX) {
                initialOffset.x *= -1f;
                lastScaleX *= -1f;
            }

            var direction = (follow.position + initialOffset) - transform.position;
            direction.z = 0;
            var delta = direction * velocity * Time.fixedDeltaTime;

            if (parallaxOn) {
                for (int i = 0; i < backgrounds.Length; i++) {
                    backgrounds[i].UpdateMovement(delta);
                }
            }

            if (lockY) delta.y = 0;

            // X max limit
            if (maxX && transform.position.x + delta.x >= maxXValue) delta.x = 0;

            // X min limit
            if (minX && transform.position.x + delta.x <= minXValue) delta.x = 0;

            transform.position += delta;
        }

        private void ZoomUpdate() {
            var direction = targetZoom - cam.orthographicSize;
            var delta = direction * zoomVelocity * Time.fixedDeltaTime;

            cam.orthographicSize += delta;
        }

    }
}
