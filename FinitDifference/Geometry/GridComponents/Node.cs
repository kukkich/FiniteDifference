using FinitDifference.Geometry.Base;
using FinitDifference.Geometry.Materials;

namespace FinitDifference.Geometry.GridComponents;

public readonly record struct Node(double X, double Y, NodeType Type, Material Material)
{
    public Node(Point2D point, NodeType type, Material material)
        : this(point.X, point.Y, type, material)
    { }

}
