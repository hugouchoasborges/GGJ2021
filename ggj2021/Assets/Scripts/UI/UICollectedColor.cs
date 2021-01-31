using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICollectedColor : MonoBehaviour
{
    [SerializeField] private Sprite emptySprite;
    [SerializeField] private Sprite colorSprite;
    [SerializeField] float fillDuration;
    [SerializeField] AnimationCurve fillCurve;

    [Header("Runtime")]
    [SerializeField] private Image img;

    private void Awake() 
    {
        img = transform.GetChild(0).GetComponent<Image>();
        img.fillAmount = 0f;
    }

    public void RefreshImage(int collected, int maxParts) 
    {
        var start = img.fillAmount;
        var target = collected / (float) maxParts;
        var duration = fillDuration * Mathf.Abs(target - start);
        StartCoroutine(CoroutineUtility.CurveRoutine(duration, fillCurve, (t, v) => img.fillAmount = Mathf.Lerp(start, target, v)));
    }
}
