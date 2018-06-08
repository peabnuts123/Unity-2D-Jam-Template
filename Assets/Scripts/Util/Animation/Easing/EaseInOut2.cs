
public class EaseInOut2 : EasingCurve
{
    public override float GetEasedValue(float t)
    {
        return (t * t) / ((t * t) + ((1 - t) * (1 - t)));
    }
}
