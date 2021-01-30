using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionLink : MonoBehaviour
{
    public InteractableCharacter character;

    [Header("Feedback")]
    [SerializeField] SpriteRenderer inputImage;
    [SerializeField] float fadeDuration;
    [SerializeField] AnimationCurve fadeCurve;

    Coroutine fadeRoutine;

    private void Start()
    {
        inputImage.color = new Color(1, 1, 1, 0);
    }

    public void TriggerFeedback(bool interactable)
    {
        if (fadeRoutine != null) StopCoroutine(fadeRoutine);

        var start = inputImage.color.a;
        var target = interactable ? 1f : 0f;

        fadeRoutine = StartCoroutine(CoroutineUtility.CurveRoutine(fadeDuration, fadeCurve, (t, v) =>
        {
            inputImage.color = new Color(1, 1, 1, Mathf.Lerp(start, target, v));
        }));
    }
    public void TriggerFeedback(bool interactable, float delay)
    {
        if (fadeRoutine != null) StopCoroutine(fadeRoutine);
        fadeRoutine = this.Invoke(delay, () => TriggerFeedback(interactable));
    }
}
