using System;
using System.Collections;
using UnityEngine;

// @TODO dunno about this. It needs to be tested at the very least
public static class DelayedCallback
{

    public static void CallIn(float delayMilliseconds, Action callback, MonoBehaviour context)
    {
        context.StartCoroutine(DelayedCoroutine(delayMilliseconds, callback));
    }

    private static IEnumerator DelayedCoroutine(float delayMilliseconds, Action callback)
    {
        yield return new WaitForSeconds(delayMilliseconds / 1000);
        callback();
    }
}