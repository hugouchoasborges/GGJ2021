using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ui;

public class InteractableObject : InteractableCharacter
{

    [SerializeField] SpriteRenderer glow;

    public enum ColorType { PURPLE, RED, BLUE, HEART}
    public ColorType objectColor = ColorType.PURPLE;

    public override void StartInteraction() {
        Interacting = true;
        glow.gameObject.SetActive(true);
    }

    public override bool Interact() {
        GameController.Instance.uiController.PickUpObject(objectColor);
        glow.gameObject.SetActive(false);
        Interacting = false;
        return true;
    }

}
