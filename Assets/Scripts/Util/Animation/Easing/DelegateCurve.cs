using System;

/// <summary>
/// Class for easily creating new `EasingCurve`s from combination functions.
/// </summary>
public class DelegateCurve : EasingCurve
{
    public delegate float Curve1(EasingCurve curve, float t);
    public delegate float Curve2(EasingCurve curveA, EasingCurve curveB, float t);

    private Order order;
    private EasingCurve curveA;
    private EasingCurve curveB;
    private Curve1 curveFunc1;
    private Curve2 curveFunc2;

    /// <summary>
    /// Create a first-order Delegate Curve
    /// </summary>
    /// <param name="curve">Instance of `EasingCurve`</param>
    /// <param name="curveFunc1">First-order combination function </param>
    public DelegateCurve(EasingCurve curve, Curve1 curveFunc1)
    {
        this.curveA = curve;
        this.curveFunc1 = curveFunc1;
        this.order = Order.One;
    }

    /// <summary>
    /// Create a second-order Delegate Curve
    /// </summary>
    /// <param name="curveA">First instance of `EasingCurve`</param>
    /// <param name="curveB">Second instance of `EasingCurve`</param>
    /// <param name="curveFunc2">Second-order combination function</param>
    public DelegateCurve(EasingCurve curveA, EasingCurve curveB, Curve2 curveFunc2)
    {
        this.curveA = curveA;
        this.curveB = curveB;
        this.curveFunc2 = curveFunc2;
        this.order = Order.Two;
    }

    public override float GetEasedValue(float t)
    {
        switch (this.order)
        {
            case Order.One:
                return this.curveFunc1(this.curveA, t);
            case Order.Two:
                return this.curveFunc2(this.curveA, this.curveB, t);
            default:
                throw new NotImplementedException("Order " + this.order + " is not yet implemented in DelegateCurve");
        }
    }

    private enum Order
    {
        One,
        Two,
    }
}