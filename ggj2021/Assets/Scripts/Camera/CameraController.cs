using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace camera 
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform follow;
        [SerializeField] private float velocity;
        [SerializeField] ParalaxBackground[] backgrounds;

        private Vector3 initialOffset;
        private float lastScaleX = 1;

        void Start()
        {
            initialOffset = transform.position - follow.position;
            lastScaleX = Mathf.Sign(transform.localScale.x);
        }

        void FixedUpdate()
        {
            if (Mathf.Sign(follow.localScale.x) != lastScaleX)
            {
                initialOffset.x *= -1f;
                lastScaleX *= -1f;
            }

            var direction = (follow.position + initialOffset) - transform.position;
            var delta = direction * velocity * Time.fixedDeltaTime;

            for (int i = 0; i < backgrounds.Length; i++)
            {
                backgrounds[i].UpdateMovement(delta);
            }

            transform.position += delta;
        }
    }
}
