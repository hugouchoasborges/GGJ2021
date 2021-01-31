using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private List<UICollectedColor> colorImages;

    public void UpdateColorOutputs(int[] collected, int[] totals) 
    {
        for (int i = 0; i < colorImages.Count; i++)
        {
            colorImages[i].RefreshImage(collected[i], totals[i]);
        }
    }

    [SerializeField] private Image whiteFader;
    [SerializeField] private float fadeVelocity = 1f;

    public void FadeInAndOutWhite(float fadeVelocity = 1, Action midCallback = null, Action endCallback = null) {
        this.fadeVelocity = fadeVelocity;
        StopAllCoroutines();
        StartCoroutine(FadeInAndOutWhite_Routine(midCallback, endCallback));
    }

    IEnumerator FadeInAndOutWhite_Routine(Action midCallback = null, Action endCallback = null) {
        whiteFader.color = new Color(1f, 1f, 1f, 0);
        whiteFader.gameObject.SetActive(true);

        float a = 0;
        while (whiteFader.color.a < 1) {
            a += fadeVelocity * Time.deltaTime;
            whiteFader.color = new Color(1f, 1f, 1f, a);
            yield return new WaitForSeconds(0.1f);
        }

        midCallback?.Invoke();
        yield return new WaitForSeconds(5f);

        a = 1;
        while (whiteFader.color.a > 0) {
            a -= fadeVelocity * Time.deltaTime;
            whiteFader.color = new Color(1f, 1f, 1f, a);
            yield return new WaitForSeconds(0.1f);
        }

        Debug.LogWarning("fadeout white ended");

        yield return new WaitForSeconds(12f);
        endCallback?.Invoke();
    }

}
