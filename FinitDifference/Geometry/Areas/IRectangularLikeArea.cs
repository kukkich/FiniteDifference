using FinitDifference.Geometry.Base;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FinitDifference.Geometry.Areas;

public interface IRectangularLikeArea
{
    public Point2D LeftBottom { get; }
    public Point2D RightBottom { get; }
    public Point2D LeftTop { get; }
    public Point2D RightTop { get; }

    public ReadOnlyCollection<Point2D> CornerNodes { get; }
    public ReadOnlyCollection<Line> Lines { get; }
    public IEnumerable<Line> VerticalBorderLines { get; }
    public IEnumerable<Line> HorizontalBorderLines { get; }
}