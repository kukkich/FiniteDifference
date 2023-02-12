using System;

namespace FinitDifference.Geometry.Base;

public readonly struct LineProjection
{
    public double Left { get; }
    public double Right { get; }

    public LineProjection(double left, double right)
    {
        if (left > right) throw new ArgumentOutOfRangeException();
        Left = left;
        Right = right;
    }

    public bool Has(double value) => Left <= value && value <= Right;
}