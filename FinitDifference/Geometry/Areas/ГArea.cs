using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using FinitDifference.Geometry.Base;

namespace FinitDifference.Geometry.Areas;

public class ГArea : IRectangularLikeArea
{
    private const double Eps = 1e-15;

    public Point2D LeftBottom => _cornerNodes[0];
    public Point2D RightBottom => new(_cornerNodes[4].X, _cornerNodes[0].Y);
    public Point2D RightTop => _cornerNodes[4];
    public Point2D LeftTop => _cornerNodes[5];
    
    public ReadOnlyCollection<Point2D> CornerNodes => new(_cornerNodes);
    public ReadOnlyCollection<Line> Lines => new (_borderLines);
    public IEnumerable<Line> VerticalBorderLines => _borderLines.Where(x => x.IsVertical);
    public IEnumerable<Line> HorizontalBorderLines => _borderLines.Where(x => x.IsHorizontal);

    private readonly Point2D[] _cornerNodes;
    private readonly List<Line> _borderLines;

    public ГArea(Point2D[] cornerNodes)
    {
        if (cornerNodes is null)
            throw new ArgumentNullException(nameof(cornerNodes));
        if (cornerNodes.Length != 6)
            throw new ArgumentException(nameof(cornerNodes));

        _cornerNodes = cornerNodes;
        _borderLines = GenerateBorderLines().ToList();
    }

    public IEnumerable<Line> GenerateBorderLines()
    {
        for (var i = 0; i < _cornerNodes.Length - 1; i++)
        {
            yield return new Line(_cornerNodes[i], _cornerNodes[i + 1]);
        }
        yield return new Line(_cornerNodes[^1], _cornerNodes[0]);
    }


    // TODO Удалить, когда будет придуман алгоритм построения сетки
    //public NodeType GetNodeType(Point2D point)
    //{
    //    if (point.X < LeftBottom.X || point.X > RightTop.X ||
    //        point.Y < LeftBottom.Y || point.Y > RightTop.Y)
    //    {
    //        throw new ArgumentOutOfRangeException();
    //    }

    //    if (!(point.X > _cornerNodes[1].X && point.Y < _cornerNodes[2].Y))
    //    {
    //        return NodeType.Fictitious;
    //    }

    //    if (point.X > _cornerNodes[1].X)
    //    {

    //    }

    //    // между x_0 и x_1
    //    if (InInterval(_cornerNodes[0].X, _cornerNodes[1].X, point.X))
    //    {

    //    }
    //    // в x0+0
    //    if (DistanceX(point, _cornerNodes[0]) <= Eps)
    //    {

    //    }
    //    // в x1-0
    //    if (DistanceX(point, _cornerNodes[1]) <= Eps)
    //    {

    //    }

    //    throw new NotImplementedException();
    //}
}