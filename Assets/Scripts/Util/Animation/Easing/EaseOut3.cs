public class EaseOut3 : EasingCurve
{
    public override float GetEasedValue(float t)
    {
        return 1 - ((1 - t) * (1 - t) * (1 - t));
    }
}