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

    [Header("Jump to Crow")]
    [SerializeField] string[] jumpAnimations;
    [SerializeField] string idleKey;
    [SerializeField] string headIdleKey;
    [SerializeField] int sortingOrderOnCrow;
    [SerializeField] float jumpDuration;
    [SerializeField] AnimationCurve jumpCurve;

    bool talkedTo;

    IEnumerator JumpToPlayer_Routine(System.Action onEnd)
    {
        render.sortingOrder = sortingOrderOnCrow;

        var pivot = player.PlayerController.Instance.GetComponentInChildren<spine.SpineController>().attachmentPivots[0];
        yield return new WaitForEndOfFrame();

        spine.AnimationState.SetAnimation(0, jumpAnimations[0], false);
        spine.AnimationState.AddAnimation(0, jumpAnimations[1], true, 0f);

        var startPos = transform.position;
        yield return StartCoroutine(CoroutineUtility.CurveRoutine(jumpDuration, jumpCurve, (t, v) =>
        {
            transform.position = Vector3.Lerp(startPos, pivot.position, v);
        }));

        spine.AnimationState.SetAnimation(0, headIdleKey, true);
        transform.SetParent(pivot);
        transform.localScale = new Vector3(-1, 1, 1);
        onEnd?.Invoke();
    }

    public override bool StartInteraction(System.Action restoreInput)
    {
        if(!talkedTo)
        {
            speechBubble.Open(interactionSprite);
            restoreInput?.Invoke();
            return true;
        }

        StartCoroutine(JumpToPlayer_Routine(restoreInput));
        return false;
    }
    public override bool Interact(System.Action restoreInput)
    {
        spine.AnimationState.SetAnimation(0, idleKey, true);
        talkedTo = true;
        speechBubble.Close();
        return base.Interact(restoreInput);
    }
}
