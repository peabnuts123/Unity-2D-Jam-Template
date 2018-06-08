public class EaseIn2 : EasingCurve
{
    public override float GetEasedValue(float t)
    {
        return t * t;
    }
}