using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    public float speed = 0;

    private void FixedUpdate() {
        transform.position += Vector3.right * speed * Time.fixedDeltaTime;
    }

}
