namespace FinitDifference.Geometry.Areas;

public interface IRectangularLikeArea
{
    public Point2D LeftBottom { get; }
    public Point2D RightBottom { get; }
    public Point2D LeftTop { get; }
    public Point2D RightTop { get; }

    public bool Contains(Point2D point);
}