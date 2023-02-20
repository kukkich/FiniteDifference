using FinitDifference.Geometry.Base;

namespace FinitDifference.Calculus.Function;

public interface ISourceFunction
{
    public double CalculateIn(Point2D point);
}