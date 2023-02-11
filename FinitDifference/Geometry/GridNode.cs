using FinitDifference.Geometry.Materials;

namespace FinitDifference.Geometry;

public readonly record struct GridNode(double X, double Y, bool IsFictitious, Material Material)
{
    public GridNode(Point2D point, bool isFictitious, Material material)
        : this(point.X, point.Y, isFictitious, material)
        { }
        
}
