using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableCharacter : MonoBehaviour
{
    [SerializeField] Sprite interactionSprite;
    [SerializeField] SpeechBubble speechBubble;

    public bool Interacting { get; protected set; }

    public virtual void StartInteraction()
    {
        Interacting = true;
        speechBubble.Open(interactionSprite);
    }
    public virtual bool Interact()
    {
        speechBubble.Close();
        Interacting = false;
        return true;
    }
}
