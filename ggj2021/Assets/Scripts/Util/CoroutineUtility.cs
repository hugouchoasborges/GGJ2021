using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class CoroutineUtility
{
    public static Coroutine WaitUntil(MonoBehaviour mono, System.Func<bool> func, float delay, UnityAction action)
    {
        return mono.StartCoroutine(WaitUntil_Routine(func, delay, action));
    }
    public static Coroutine WaitUntil(MonoBehaviour mono, System.Func<bool> func, UnityAction action)
    {
        return mono.StartCoroutine(WaitUntil_Routine(func, 0f, action));
    }


    public static IEnumerator WaitUntil_Routine(System.Func<bool> func, float delay, UnityAction action)
    {
        yield return new WaitUntil(func);
        yield return new WaitForSeconds(delay);

        action?.Invoke();
    }
    public static IEnumerator WaitUntil_Routine(System.Func<bool> func, System.Action action)
    {
        yield return new WaitUntil(func);
        action?.Invoke();
    }

    public static IEnumerator CurveRoutine(float duration, UnityEngine.AnimationCurve curve, 
        System.Action startAction, System.Action<float, float> midAction, System.Action endAction, bool isRealtime)
    {
        startAction?.Invoke();

        var t = 0f;
        while(t < 1.0f)
        {
            t += (isRealtime ? Time.unscaledDeltaTime : Time.deltaTime) / duration;
            t = Mathf.Clamp01(t);
            midAction?.Invoke(t, curve.Evaluate(t));

            yield return new WaitForEndOfFrame();
        }

        endAction?.Invoke();
    }
    public static IEnumerator CurveRoutine(float duration, UnityEngine.AnimationCurve curve, System.Action startAction, System.Action<float, float> midAction, 
        System.Action endAction)
    {
        return CurveRoutine(duration, curve, startAction, midAction, endAction, false);
    }
    public static IEnumerator CurveRoutine(float duration, UnityEngine.AnimationCurve curve, System.Action startAction, System.Action<float, float> midAction)
    {
        return CurveRoutine(duration, curve, startAction, midAction, null);
    }
    public static IEnumerator CurveRoutine(float duration, UnityEngine.AnimationCurve curve, System.Action<float, float> midAction, System.Action endAction)
    {
        return CurveRoutine(duration, curve, null, midAction, endAction);
    }
    public static IEnumerator CurveRoutine(float duration, System.Action<float, float> midAction, System.Action endAction)
    {
        var curve = new UnityEngine.AnimationCurve();
        curve.AddKey(0f, 0f);
        curve.AddKey(1f, 1f);

        return CurveRoutine(duration, curve, null, midAction, endAction, false);
    }
    public static IEnumerator CurveRoutine(float duration, UnityEngine.AnimationCurve curve, System.Action<float, float> midAction)
    {
        return CurveRoutine(duration, curve, null, midAction, null);
    }
    public static IEnumerator CurveRoutine(float duration, System.Action<float, float> midAction, bool isRealtime)
    {
        var curve = new UnityEngine.AnimationCurve();
        curve.AddKey(0f, 0f);
        curve.AddKey(1f, 1f);

        return CurveRoutine(duration, curve, null, midAction, null, isRealtime);
    }
    public static IEnumerator CurveRoutine(float duration, System.Action<float, float> midAction)
    {
        return CurveRoutine(duration, midAction, false);
    }
}

public static class CoroutineUtility_Extensions
{
    public static Coroutine WaitUntil(this MonoBehaviour mono, System.Func<bool> func, float delay, UnityAction action)
    {
        return CoroutineUtility.WaitUntil(mono, func, delay, action);
    }
    public static Coroutine WaitUntil(this MonoBehaviour mono, System.Func<bool> func, UnityAction action)
    {
        return CoroutineUtility.WaitUntil(mono, func, 0f, action);
    }
    
}