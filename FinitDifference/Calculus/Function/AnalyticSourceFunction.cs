using System;
using FinitDifference.Geometry.Base;

namespace FinitDifference.Calculus.Function;

public class AnalyticSourceFunction : ISourceFunction
{
    private readonly Func<Point2D, double> _function;

    public AnalyticSourceFunction(Func<Point2D, double> function)
    {
        _function = function;
    }

    public double CalculateIn(Point2D point)
    {
        return _function(point);
    }
}