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
}
