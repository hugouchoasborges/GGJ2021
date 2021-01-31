using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableCharacter : MonoBehaviour
{
    public bool Interactable { get; protected set; }
    public bool Interacting { get; protected set; }

    protected virtual void Awake()
    {
        Interactable = true;
    }

    /// <summary>
    /// Triggers first interaction with character, returns true if it is continued
    /// </summary>
    /// <param name="restoreInput"></param>
    /// <returns></returns>
    public virtual bool StartInteraction(System.Action restoreInput)
    {
        Interacting = true;
        restoreInput?.Invoke();
        return true;
    }

    /// <summary>
    /// Triggers interaction with character, returns true if it is not continued
    /// </summary>
    /// <param name="restoreInput"></param>
    /// <returns></returns>
    public virtual bool Interact(System.Action restoreInput)
    {
        Interacting = false;
        restoreInput?.Invoke();
        return true;
    }
}
