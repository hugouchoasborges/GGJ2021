using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuraBehaviour : MonoBehaviour
{
    [SerializeField] Material material;
    [SerializeField] Vector3 offset;
    
    // Update is called once per frame
    void Update()
    {
        material.SetVector("_GrayscaleCenterPosition", transform.position + offset);   
    }

    private void OnDrawGizmosSelected()
    {
        if (material) Update();
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + offset, 0.5f);
    }
}
