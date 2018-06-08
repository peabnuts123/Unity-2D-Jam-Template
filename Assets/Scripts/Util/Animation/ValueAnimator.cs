using System;
using System.Collections;
using UnityEngine;

// @TODO any way to diagnose if one of these animation functions never ends?
public class ValueAnimator : MonoBehaviour
{
    // Animate over time
    public void Animate(float fromValue, float toValue, float timeSpanSeconds, Action<float> mutatorFunction)
    {
        this.Animate(fromValue, toValue, timeSpanSeconds, mutatorFunction, EasingCurve.Linear);
    }
    public void Animate(float fromValue, float toValue, float timeSpanSeconds, Action<float> mutatorFunction, EasingCurve easingCurve)
    {
        StartCoroutine(AnimateOverTimeCoroutine(fromValue, toValue, timeSpanSeconds, mutatorFunction, easingCurve));
    }

    // Animate with respect to other variables
    public void Animate(float fromValue, float toValue, Func<float> percentageFetcherFunction, Action<float> mutatorFunction)
    {
        this.Animate(fromValue, toValue, percentageFetcherFunction, mutatorFunction, EasingCurve.Linear);
    }
    public void Animate(float fromValue, float toValue, Func<float> percentageFetcherFunction, Action<float> mutatorFunction, EasingCurve easingCurve)
    {
        StartCoroutine(AnimateWithRespectCoroutine(fromValue, toValue, percentageFetcherFunction, mutatorFunction, easingCurve));
    }

    private IEnumerator AnimateOverTimeCoroutine(float fromValue, float toValue, float timeSpanSeconds, Action<float> mutatorFunction, EasingCurve easingCurve)
    {
        System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
        stopwatch.Start();

        float currentPercentage;
        do
        {
            currentPercentage = (stopwatch.ElapsedMilliseconds / 1000F) / timeSpanSeconds;
            float newValue = easingCurve.Apply(fromValue, toValue, currentPercentage);
            mutatorFunction(newValue);
            yield return null;
        } while (currentPercentage + float.Epsilon < 1);
    }

    private IEnumerator AnimateWithRespectCoroutine(float fromValue, float toValue, Func<float> percentageFetcherFunction, Action<float> mutatorFunction, EasingCurve easingCurve)
    {
        float currentPercentage;
        do
        {
            currentPercentage = percentageFetcherFunction();
            float newValue = easingCurve.Apply(fromValue, toValue, currentPercentage);
            mutatorFunction(newValue);
            yield return null;
        } while (currentPercentage + float.Epsilon < 1);
    }
}