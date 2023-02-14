using FinitDifference.Geometry.Base;
using System.Collections.Generic;
using System.Linq;

namespace FinitDifference.Geometry.GridComponents;

public class Border
{
    public Line Line { get; }
    public bool IsVertical => Line.IsVertical;
    public bool IsHorizontal => Line.IsHorizontal;

    public List<(int row, int column)> BelongedNodeIndexes { get; }

    public static explicit operator Border(Line line)
    {
        return new Border(line);
    }

    public Border(Line line)
    {
        Line = line;
        BelongedNodeIndexes = new List<(int row, int column)>();
    }
}