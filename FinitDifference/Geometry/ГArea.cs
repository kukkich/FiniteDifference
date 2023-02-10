namespace FinitDifference.Geometry;

public class ГArea
{
    public Point2D[] CornerNodes { get; }
    public Point2D LeftBottom => CornerNodes[0];
    public Point2D RightBottom => new (CornerNodes[5].X, CornerNodes[0].Y);
    public Point2D LeftTop => CornerNodes[4];
    public Point2D RightTop => CornerNodes[5];

    public bool Contains(Point2D point)
    {
        return !(point.X > CornerNodes[1].X && point.Y < CornerNodes[2].Y);
    }

    public ГArea(Point2D[] cornerNodes)
    {
        if (cornerNodes is null)
            throw new ArgumentNullException(nameof(cornerNodes));
        if (cornerNodes.Length != 6)
            throw new ArgumentException(nameof(cornerNodes));
        CornerNodes = cornerNodes;
    }
}