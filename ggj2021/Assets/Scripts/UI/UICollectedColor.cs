using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICollectedColor : MonoBehaviour
{
    [SerializeField] private Sprite emptySprite;
    [SerializeField] private Sprite colorSprite;
    private Image img;

    private bool _collected = false;

    private void Awake() {
        img = GetComponent<Image>();
        RefreshImage();
    }

    public void Collect() {
        _collected = true;
        RefreshImage();
    }

    public void RefreshImage() {
        if (_collected) img.sprite = colorSprite;
        else img.sprite = emptySprite;
    }
}
