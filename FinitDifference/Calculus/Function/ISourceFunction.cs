using FinitDifference.Geometry.Base;
using FinitDifference.Geometry.GridComponents;

namespace FinitDifference.Calculus.Function;

public interface ISourceFunction
{
    public double CalculateIn(Point2D point);
}