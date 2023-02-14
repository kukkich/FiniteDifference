using System;

namespace FinitDifference.Geometry.Base;

public readonly struct Line
{
    public Point2D Begin { get; }
    public Point2D End { get; }
    public bool IsVertical => Math.Abs(Begin.X - End.X) < CalculusConfig.Eps;
    public bool IsHorizontal => Math.Abs(Begin.Y - End.Y) < CalculusConfig.Eps;
    public LineProjection XProjection { get; }
    public LineProjection YProjection { get; }

    public Line(Point2D begin, Point2D end)
    {
        Begin = begin;
        End = end;
        XProjection = new LineProjection(Math.Min(Begin.X, End.X), Math.Max(Begin.X, End.X));
        YProjection = new LineProjection(Math.Min(Begin.Y, End.Y), Math.Max(Begin.Y, End.Y));
    }
}