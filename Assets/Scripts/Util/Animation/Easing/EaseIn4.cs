public class EaseIn4 : EasingCurve
{
    public override float GetEasedValue(float t)
    {
        return t * t * t * t;
    }
}