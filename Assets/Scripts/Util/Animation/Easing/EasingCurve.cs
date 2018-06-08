using UnityEngine;

public abstract class EasingCurve
{
    // Static constant curve references (you should never really be instantiating curves yourself)
    public static readonly EasingCurve Linear = new Linear();
    public static readonly EasingCurve EaseIn2 = new EaseIn2();
    public static readonly EasingCurve EaseOut2 = new EaseOut2();
    public static readonly EasingCurve EaseInOut2 = new EaseInOut2();
    public static readonly EasingCurve EaseIn3 = new EaseIn3();
    public static readonly EasingCurve EaseOut3 = new EaseOut3();
    public static readonly EasingCurve EaseInOut3 = new EaseInOut3();
    public static readonly EasingCurve EaseIn4 = new EaseIn4();
    public static readonly EasingCurve EaseOut4 = new EaseOut4();
    public static readonly EasingCurve EaseInOut4 = new EaseInOut4();


    // EasingCurve functions
    public float Apply(float minValue, float maxValue, float percentage)
    {
        float easedPercentage = this.GetEasedValue(percentage);

        if (easedPercentage < float.Epsilon && easedPercentage > -float.Epsilon)
        {
            return minValue;
        }
        else
        {
            return Mathf.Lerp(minValue, maxValue, easedPercentage);
        }
    }

    public abstract float GetEasedValue(float t);


    // Utility functions for creating new curves

    // Invert the result of curve (1 - x)
    public static float Invert(EasingCurve curve, float t)
    {
        return 1 - curve.GetEasedValue(t);
    }

    // Multiple the result of one curve by another
    public static float Multiply(EasingCurve curveA, EasingCurve curveB, float t)
    {
        return curveA.GetEasedValue(t) * curveB.GetEasedValue(t);
    }

    // Use the result of one curve as the parameter for another
    public static float Compose(EasingCurve curveA, EasingCurve curveB, float t)
    {
        return curveA.GetEasedValue(curveB.GetEasedValue(t));
    }

    // Blend between two curves based on the value of t
    public static float CrossFade(EasingCurve curveA, EasingCurve curveB, float t)
    {
        float easedValueA = curveA.GetEasedValue(t);
        float easedValueB = curveB.GetEasedValue(t);

        return ((1 - t) * easedValueA) + (t * easedValueB);
    }

    // Use a blend of the results from two curves
    public static float Blend(EasingCurve curveA, EasingCurve curveB, float t, float blendValue)
    {
        float easedValueA = curveA.GetEasedValue(t);
        float easedValueB = curveB.GetEasedValue(t);
        return easedValueA + (blendValue * (easedValueB - easedValueA));
    }
}