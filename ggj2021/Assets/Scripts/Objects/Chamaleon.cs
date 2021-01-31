using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class Chamaleon : InteractableCharacter
{
    [SerializeField] SkeletonAnimation spine;
    [SerializeField] Renderer render;
    [SerializeField] Sprite interactionSprite;
    [SerializeField] SpeechBubble speechBubble;
    [SerializeField] InteractableObject.ColorType requiredColor;

    [Header("Jump to Crow")]
    [SerializeField] string[] jumpAnimations;
    [SerializeField] string idleKey;
    [SerializeField] string headIdleKey;
    [SerializeField] int sortingOrderOnCrow;
    [SerializeField] float jumpDuration;
    [SerializeField] Vector3 postJumpScale;
    [SerializeField] AnimationCurve jumpCurve;
    [SerializeField] float jumpHeight;
    [SerializeField] AnimationCurve jumpHeightCurve;

    IEnumerator JumpToPlayer_Routine(System.Action onEnd)
    {
        var pivot = player.PlayerController.Instance.GetComponentInChildren<spine.SpineController>().attachmentPivots[0];
        spine.AnimationState.SetAnimation(0, idleKey, true);
        render.sortingOrder = sortingOrderOnCrow;

        yield return new WaitForSeconds(0.65f);

        spine.AnimationState.SetAnimation(0, jumpAnimations[0], false);
        spine.AnimationState.AddAnimation(0, jumpAnimations[1], true, 0f);

        var startPos = transform.position;
        var startScale = transform.localScale;
        bool switched = false;
        yield return StartCoroutine(CoroutineUtility.CurveRoutine(jumpDuration, jumpCurve, (t, v) =>
        {
            transform.position = Vector3.Lerp(startPos, pivot.position, v) + (Vector3.up * jumpHeight * jumpHeightCurve.Evaluate(t));
            transform.localScale = Vector3.Lerp(startScale, postJumpScale, v);
            if (!switched && t > 0.5f) render.sortingLayerName = "Default";
        }));

        spine.AnimationState.SetAnimation(0, headIdleKey, true);
        transform.SetParent(pivot);
        if(transform.position.x > startPos.x) transform.localScale = postJumpScale;

        player.PlayerController.Instance.SetState(player.PlayerController.PlayerState.Happy);
        
        onEnd?.Invoke();
    }

    public override bool StartInteraction(System.Action restoreInput)
    {
        if(player.PlayerController.Instance.collectedColors.Contains(InteractableObject.ColorType.PURPLE))
        {
            StartCoroutine(JumpToPlayer_Routine(restoreInput));
            return false;
        }

        speechBubble.Open(interactionSprite);
        restoreInput?.Invoke();
        return true;
    }
    public override bool Interact(System.Action restoreInput)
    {
        speechBubble.Close();
        return base.Interact(restoreInput);
    }
}
