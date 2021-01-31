using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InteractionBehaviour : MonoBehaviour
{
    [SerializeField] Vector3 offset;
    [SerializeField] float radius;
    [SerializeField] int layer;
    [SerializeField] player.WalkBehavior walkBehavior;
 
    [Header("Runtime")]
    [SerializeField] Transform interactable;
    [SerializeField] InteractionLink interactionLink;
    public InteractableCharacter interactableCharacter;

    public bool inputEnabled;

    public bool UsingInput => inputEnabled && interactableCharacter != null;

    Vector3 Offset
    {
        get
        {
            var dir = walkBehavior.IsFacingRight ? 1 : -1;
            return new Vector3(offset.x * dir, offset.y, offset.z);
        }
    }

    private void Start()
    {
        inputEnabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(interactableCharacter == null)
        {
            var hits = Physics2D.CircleCastAll(transform.position + Offset, radius, Vector3.up, 0.1f, 1 << layer);
            if (hits.Length > 0)
            {
                var closest = hits.OrderBy(h => Vector2.Distance(h.point, transform.position + Offset)).First().transform;
                if (closest != interactable)
                {
                    if (interactionLink) interactionLink.TriggerFeedback(false);
                    interactionLink = closest.GetComponent<InteractionLink>();
                    if (interactionLink && interactionLink.Interactable) interactionLink.TriggerFeedback(true);
                }
                interactable = closest;
            }
            else
            {
                if (interactionLink) interactionLink.TriggerFeedback(false);
                interactable = null;
                interactionLink = null;
            }

            if(Input.GetKeyDown(KeyCode.E) && interactionLink != null && interactionLink.Interactable)
            {
                inputEnabled = false;
                interactableCharacter = interactionLink.character;
                var hasSequence = interactableCharacter.StartInteraction(() => inputEnabled = true);
                interactionLink.TriggerFeedback(false);
                
                if(!hasSequence)
                {
                    interactableCharacter = null;
                }
            }
        }
        else
        {
            if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
            {
                inputEnabled = false;
                var endedInteraction = interactableCharacter.Interact(() => inputEnabled = true);
                if (endedInteraction)
                {
                    if(interactableCharacter.Interactable) interactionLink.TriggerFeedback(true, 1f);
                    interactableCharacter = null;
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        var offset = Application.isPlaying ? Offset : this.offset;

        Gizmos.DrawWireSphere(transform.position + offset, radius);
    }
}
