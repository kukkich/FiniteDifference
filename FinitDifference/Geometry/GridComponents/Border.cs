using FinitDifference.Geometry.Base;
using System;
using System.Collections.Generic;

namespace FinitDifference.Geometry.GridComponents;

public class Border
{
    public Line Line { get; }
    public bool IsVertical => Line.IsVertical;
    public bool IsHorizontal => Line.IsHorizontal;

    public NormalOrientation NormalOrientation
    {
        get
        {
            if (IsHorizontal)
            {
                if (Line.Begin.X < Line.End.X) return NormalOrientation.Down;
                if (Line.Begin.X > Line.End.X) return NormalOrientation.Up;
            }
            if (IsVertical)
            {
                if (Line.Begin.Y < Line.End.Y) return NormalOrientation.Right;
                if (Line.Begin.Y > Line.End.Y) return NormalOrientation.Left;
            }

            throw new NotSupportedException();
        }
    }
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