using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuraBehaviour : MonoBehaviour
{
    [SerializeField] Material material;
    [SerializeField] Vector3 offset;
    [SerializeField] float fadeDuration;
    [SerializeField] AnimationCurve fadeCurve;
    
    // Update is called once per frame
    void Update()
    {
        material.SetVector("_GrayscaleCenterPosition", transform.position + offset);   
    }

    public void UpdateBlendValues(float innerGsBlend, float outerGsBlend, bool useTransition = true)
    {
        var keys = new string[]
        {
            "_InnerGrayscaleBlend",
            "_OuterGrayscaleBlend",
        };

        var targetValues = new float[]
        {
            innerGsBlend,
            outerGsBlend
        };

        for (int i = 0; i < keys.Length; i++)
        {
            if(useTransition)
            {
                var start = material.GetFloat(keys[i]);
                var target = targetValues[i];
                var key = keys[i];
                StartCoroutine(CoroutineUtility.CurveRoutine(fadeDuration, fadeCurve, (t, v) => material.SetFloat(key, Mathf.Lerp(start, target, v))));
            }
            else
            {
                material.SetFloat(keys[i], targetValues[i]);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (material) Update();
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + offset, 0.5f);
    }
}
