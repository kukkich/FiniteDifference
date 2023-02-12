using FinitDifference.Geometry.Base;
using FinitDifference.Geometry.Materials;

namespace FinitDifference.Geometry;

public readonly record struct Node(double X, double Y, NodeType IsFictitious, Material Material)
{
    public Node(Point2D point, NodeType isFictitious, Material material)
        : this(point.X, point.Y, isFictitious, material)
        { }
        
}
