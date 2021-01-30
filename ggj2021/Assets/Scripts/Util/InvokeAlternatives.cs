using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InvokeAlternatives
{
    static IEnumerator InvokeAfterFrame_Routine(System.Action action)
    {
        yield return new WaitForEndOfFrame();
        action?.Invoke();
    }

    static IEnumerator Invoke_Routine(float delay, System.Action action, bool isRealtime)
    {
        if (isRealtime)
            yield return new WaitForSecondsRealtime(delay);
        else
            yield return new WaitForSeconds(delay);

        action?.Invoke();
    }

    public static void InvokeAfterFrame(this MonoBehaviour host, System.Action action)
    {
        host.StartCoroutine(InvokeAfterFrame_Routine(action));
    }

    public static void Invoke(this MonoBehaviour host, float delay, System.Action action, bool isRealtime = false)
    {
        host.StartCoroutine(Invoke_Routine(delay, action, isRealtime));
    }
}
