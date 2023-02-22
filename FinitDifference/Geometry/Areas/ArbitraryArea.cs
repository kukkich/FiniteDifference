//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.Linq;
//using FinitDifference.Geometry.Base;

//namespace FinitDifference.Geometry.Areas;

//public class ArbitraryArea : IRectangularLikeArea
//{
//    public Point2D LeftBottom => _cornerNodes[0];
//    public Point2D RightBottom => _cornerNodes[1];
//    public Point2D LeftTop => _cornerNodes[3];
//    public Point2D RightTop => _cornerNodes[2];
//    public ReadOnlyCollection<Point2D> CornerNodes => new(_cornerNodes);
//    public ReadOnlyCollection<Line> Lines => new(_borderLines);
//    public IEnumerable<Line> VerticalBorderLines => _borderLines.Where(x => x.IsVertical);
//    public IEnumerable<Line> HorizontalBorderLines => _borderLines.Where(x => x.IsHorizontal);

//    private readonly Point2D[] _cornerNodes;
//    private readonly List<Line> _borderLines;

//    private readonly Point2D _leftBottom;
//    private readonly Point2D _rightBottom => _cornerNodes[1];
//    private readonly Point2D _leftTop => _cornerNodes[3];
//    private readonly Point2D _rightTop => _cornerNodes[2];

//    public ArbitraryArea(Point2D[] cornerNodes)
//    {
//        if (cornerNodes is null)
//            throw new ArgumentNullException(nameof(cornerNodes));
//        if (cornerNodes.Length != 4)
//            throw new ArgumentException(nameof(cornerNodes));

//        _cornerNodes = cornerNodes;
//        _borderLines = GenerateBorderLines().ToList();
//    }

//    public IEnumerable<Line> GenerateBorderLines()
//    {
//        for (var i = 0; i < _cornerNodes.Length - 1; i++)
//        {
//            yield return new Line(_cornerNodes[i], _cornerNodes[i + 1]);
//        }
//        yield return new Line(_cornerNodes[^1], _cornerNodes[0]);
//    }
//}