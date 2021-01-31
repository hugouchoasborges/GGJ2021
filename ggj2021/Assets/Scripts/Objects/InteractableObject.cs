using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ui;

public class InteractableObject : InteractableCharacter
{
    [SerializeField] SpriteRenderer glow;
    [SerializeField] float glowDuration;
    [SerializeField] AnimationCurve glowCurve;

    public enum ColorType { PURPLE, RED, BLUE, HEART}
    public ColorType objectColor = ColorType.PURPLE;

    private void Start()
    {
        glow.SetAlpha(0f);
    }

    IEnumerator Glow_Routine(System.Action onEnd)
    {
        yield return StartCoroutine(CoroutineUtility.CurveRoutine(glowDuration, glowCurve, (t, v) => glow.SetAlpha(v)));

        yield return new WaitForSeconds(1f);
        player.PlayerController.Instance.SetState(player.PlayerController.PlayerState.Neutral);
        yield return new WaitForSeconds(1f);

        GameController.Instance.uiController.PickUpObject(objectColor);
        yield return StartCoroutine(CoroutineUtility.CurveRoutine(glowDuration, glowCurve, (t, v) => glow.SetAlpha(1f - v)));

        onEnd?.Invoke();
    }

    public override bool StartInteraction(System.Action restoreInput) 
    {
        StopAllCoroutines();
        StartCoroutine(Glow_Routine(restoreInput));
        Interactable = false;
        return false;
    }
}
