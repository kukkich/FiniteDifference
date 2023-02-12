using System;

namespace FinitDifference.Geometry.Base;

public readonly struct Line
{
    public Point2D A { get; }
    public Point2D B { get; }
    public bool IsVertical => Math.Abs(A.X - B.X) < CalculusConfig.Eps;
    public bool IsHorizontal => Math.Abs(A.Y - B.Y) < CalculusConfig.Eps;
    public LineProjection XProjection { get; }
    public LineProjection YProjection { get; }

    public Line(Point2D a, Point2D b)
    {
        A = a;
        B = b;
        XProjection = new LineProjection(Math.Min(A.X, B.X), Math.Max(A.X, B.X));
        YProjection = new LineProjection(Math.Min(A.Y, B.Y), Math.Max(A.Y, B.Y));
    }
}