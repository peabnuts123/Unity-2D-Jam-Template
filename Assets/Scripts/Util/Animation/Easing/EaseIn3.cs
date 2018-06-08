public class EaseIn3 : EasingCurve
{
    public override float GetEasedValue(float t)
    {
        return t * t * t;
    }
}