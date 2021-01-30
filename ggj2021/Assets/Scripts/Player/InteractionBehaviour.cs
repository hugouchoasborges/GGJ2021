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

    Vector3 Offset
    {
        get
        {
            var dir = walkBehavior.IsFacingRight ? 1 : -1;
            return new Vector3(offset.x * dir, offset.y, offset.z);
        }
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
                    if (interactionLink) interactionLink.TriggerFeedback(true);
                }
                interactable = closest;
            }
            else
            {
                if (interactionLink) interactionLink.TriggerFeedback(false);
                interactable = null;
                interactionLink = null;
            }

            if(Input.GetKeyDown(KeyCode.E) && interactionLink != null)
            {
                interactableCharacter = interactionLink.character;
                interactableCharacter.StartInteraction();
                interactionLink.TriggerFeedback(false);
            }
        }
        else
        {
            if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
            {
                var endedInteraction = interactableCharacter.Interact();
                if (endedInteraction)
                {
                    interactableCharacter = null;
                    interactionLink.TriggerFeedback(true, 1f);
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
