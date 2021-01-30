using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace camera 
{
    [RequireComponent(typeof(Camera))]
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform follow;
        [SerializeField] private float velocity;
        [SerializeField] ParalaxBackground[] backgrounds;
        private Camera cam;

        private Vector3 initialOffset;
        private float lastScaleX = 1;

        [Header("Zoom Control")]
        [SerializeField] [Range(0f, 10)] private float targetZoom;
        [SerializeField] private float zoomVelocity = 1;
        private float initialZoomSize;

        private void Awake() {
            initialOffset = transform.position - follow.position;
            lastScaleX = Mathf.Sign(transform.localScale.x);

            cam = GetComponent<Camera>();
            initialZoomSize = targetZoom = cam.orthographicSize;
        }

        void FixedUpdate()
        {
            PositionUpdate();
            ZoomUpdate();
        }

        private void PositionUpdate() {
            if (Mathf.Sign(follow.localScale.x) != lastScaleX) {
                initialOffset.x *= -1f;
                lastScaleX *= -1f;
            }

            var direction = (follow.position + initialOffset) - transform.position;
            var delta = direction * velocity * Time.fixedDeltaTime;

            for (int i = 0; i < backgrounds.Length; i++) {
                backgrounds[i].UpdateMovement(delta);
            }

            transform.position += delta;
        }

        private void ZoomUpdate() {
            var direction = targetZoom - cam.orthographicSize;
            var delta = direction * zoomVelocity * Time.fixedDeltaTime;

            cam.orthographicSize += delta;
        }

    }
}
