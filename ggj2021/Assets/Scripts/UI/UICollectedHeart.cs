using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICollectedHeart : MonoBehaviour
{
    [SerializeField] private Image[] heartPieces;

    public void ShowHeartPieces(int pieces)
    {
        DisableHeartPieces();

        for (int i = 0; i < pieces; i++)
        {
            if (i >= heartPieces.Length)
                break;

            heartPieces[i].enabled = true;
        }
    }

    private void DisableHeartPieces()
    {
        for (int i = 0; i < heartPieces.Length; i++)
        {
            heartPieces[i].enabled = false;
        }
    }
}
