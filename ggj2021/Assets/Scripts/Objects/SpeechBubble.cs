using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechBubble : MonoBehaviour
{
    [Header("References")]
    [SerializeField] SpriteRenderer content;

    [Header("Transition")]
    [SerializeField] float appearDuration;
    [SerializeField] AnimationCurve appearCurve;
    [SerializeField] float hideDuration;
    [SerializeField] AnimationCurve hideCurve;

    [Header("Editor")]
    [SerializeField] Sprite[] editorSprites;

    Vector3 defaultScale;
    Coroutine routine;

    private void Start()
    {
        defaultScale = transform.localScale;
        transform.localScale = Vector3.zero;
    }

    public void Open(Sprite sprite)
    {
        content.sprite = sprite;
        if (routine != null) StopCoroutine(routine);
        routine = StartCoroutine(CoroutineUtility.CurveRoutine(appearDuration, appearCurve, (t, v) => transform.localScale = defaultScale * v));
    }
    public void Appear()
    {
        Open(editorSprites[Random.Range(0, editorSprites.Length)]);
    }

    public void Close()
    {
        if (routine != null) StopCoroutine(routine);
        routine = StartCoroutine(CoroutineUtility.CurveRoutine(hideDuration, hideCurve, (t, v) => transform.localScale = defaultScale * v));
    }
}
