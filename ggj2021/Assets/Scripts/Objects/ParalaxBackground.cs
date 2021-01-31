using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParalaxBackground : MonoBehaviour
{
    [SerializeField] float movementMultiplier;

    public void UpdateMovement(Vector3 cameraDelta)
    {
        transform.position += new Vector3(cameraDelta.x * movementMultiplier * 0.1f, 0, 0);
    }
}
