namespace FinitDifference.Geometry;

public readonly record struct GridNode(double X, double Y, bool IsFictitious)
{
    public GridNode(Point2D point, bool isFictitious)
        : this(point.X, point.Y, isFictitious)
        { }
        
}