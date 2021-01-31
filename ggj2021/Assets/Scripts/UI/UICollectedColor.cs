using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICollectedColor : MonoBehaviour
{
    [SerializeField] private Sprite emptySprite;
    [SerializeField] private Sprite colorSprite;
    private Image img;

    public int _collected = 0;
    public int _maxParts = 4;


    private void Awake() {
        img = transform.GetChild(0).GetComponent<Image>();
        RefreshImage();
    }

    public void Collect() {
        _collected++;
        RefreshImage();
    }

    public void RefreshImage() {
        img.fillAmount = _collected / (float)_maxParts;
    }
}
