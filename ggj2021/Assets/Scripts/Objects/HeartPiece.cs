using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartPiece : InteractableCharacter
{
    [SerializeField] SpriteRenderer render;
    [SerializeField] Sprite[] heartSprites;
    [SerializeField] int pieceIndex;

    [Header("Appear Sequence")]
    [SerializeField] AnimationCurve appearCurve;
    [SerializeField] float appearSequenceDuration;

    [Header("Merge Sequence")]
    [SerializeField] AnimationCurve mergeCurve;
    [SerializeField] float mergeMotionDuration;
    [SerializeField] float mergeFadeDuration;
    [SerializeField] float postMergeScale;
    [SerializeField, Range(0f, 1f)] float mergeEffectPoint;

    protected override void Awake()
    {
        base.Awake();
        Interactable = true;
    }

    IEnumerator MergeWithPlayer(System.Action onEnd)
    {
        var pivot = player.PlayerController.Instance.GetComponentInChildren<spine.SpineController>().attachmentPivots[1];

        var startPos = transform.position;
        var startScale = transform.localScale;
        bool acted = false;

        StartCoroutine(CoroutineUtility.CurveRoutine(mergeFadeDuration, mergeCurve, (t, v) => render.SetAlpha(1f - v)));
        yield return StartCoroutine(CoroutineUtility.CurveRoutine(mergeMotionDuration, mergeCurve, (t, v) =>
        {
            transform.position = Vector3.Lerp(startPos, pivot.position, v);
            transform.localScale = Vector3.Lerp(startScale, Vector3.one * postMergeScale, v);
            if (!acted && t >= mergeEffectPoint)
            {
                player.PlayerController.Instance.AdvanceState();
                acted = true;
            }
        }));

        onEnd?.Invoke();

        Interactable = false;
    }

    public Coroutine Appear(int index)
    {
        pieceIndex = index;
        render.sprite = heartSprites[index];
        return StartCoroutine(CoroutineUtility.CurveRoutine(appearSequenceDuration, appearCurve, (t, v) => transform.localScale = Vector3.one * v));
    }

    public override bool StartInteraction(System.Action restoreInput)
    {
        StartCoroutine(MergeWithPlayer(restoreInput));

        return false;
    }

}
