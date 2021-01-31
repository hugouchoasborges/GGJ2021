using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private List<UICollectedColor> colorImages;

    public void PickUpObject(InteractableObject.ColorType color) {
        switch (color) {
            case InteractableObject.ColorType.PURPLE:
                colorImages[0].Collect();
                break;
            case InteractableObject.ColorType.RED:
                colorImages[1].Collect();
                break;
            case InteractableObject.ColorType.BLUE:
                colorImages[2].Collect();
                break;
            case InteractableObject.ColorType.HEART:
                break;
            default:
                break;
        }
    }

}
