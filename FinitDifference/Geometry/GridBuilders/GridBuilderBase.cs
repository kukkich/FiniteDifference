using FinitDifference.Geometry.Areas;
using FinitDifference.Geometry.GridComponents;
using FinitDifference.Geometry.Materials;
using System;
using System.Linq;
using FinitDifference.Geometry.Base;
using FinitDifference.Geometry.GridBuilders.Splitting;

namespace FinitDifference.Geometry.GridBuilders;

public class RectangularGridBuilder : IGridBuilder
{
    protected readonly Point2D<AxisSplitParameter> SplitParameter;
    protected readonly IMaterialProvider MaterialProvider;
    protected IRectangularLikeArea Area { get; private set; }

    public RectangularGridBuilder(Point2D<AxisSplitParameter> splitParameter, IMaterialProvider materialProvider)
    {
        SplitParameter = splitParameter;
        MaterialProvider = materialProvider;
    }

    public Grid Build(IRectangularLikeArea area)
    {
        if (!CanBeSplitted(area))
            throw new ArgumentException();
        Area = area;
        var grid = MakeGrid(area);
        MarkInnerAndOuter(grid);
        MarkBorderNodes(grid);

        return grid;
    }

    public bool CanBeSplitted(IRectangularLikeArea area)
    {
        if (Math.Abs(area.LeftBottom.X - SplitParameter.X.Sections[0].Begin) > CalculusConfig.Eps)
            return false;
        if (Math.Abs(area.RightBottom.X - SplitParameter.X.Sections[^1].End) > CalculusConfig.Eps)
            return false;

        if (Math.Abs(area.LeftBottom.Y - SplitParameter.Y.Sections[0].Begin) > CalculusConfig.Eps)
            return false;
        if (Math.Abs(area.LeftTop.Y - SplitParameter.Y.Sections[^1].End) > CalculusConfig.Eps)
            return false;

        return true;
    }

    private Grid MakeGrid(IRectangularLikeArea area)
    {
        Point2D<int> totalNodes = GetTotalNodes(area);

        var nodes = new Node[totalNodes.Y, totalNodes.X];
        var (i, j) = (0, 0);

        foreach (var (ySection, ySplitter) in SplitParameter.Y.SectionWithParameter)
        {
            var yValues = ySplitter.EnumerateValues(ySection);
            if (i > 0) yValues = yValues.Skip(1);

            foreach (var y in yValues)
            {
                foreach (var (xSection, xSplitter) in SplitParameter.X.SectionWithParameter)
                {
                    var xValues = xSplitter.EnumerateValues(xSection);
                    if (j > 0) xValues = xValues.Skip(1);

                    foreach (var x in xValues)
                    {
                        var point = new Point2D(x, y);

                        var material = MaterialProvider.GetMaterialByIndexes(j, j);
                        nodes[i, j] = new Node(point, NodeType.Undefined, material);

                        j++;
                    }
                }

                j = 0;
                i++;
            }
        }

        return new Grid(nodes, area.Lines.Select(x => new Border(x)).ToArray());
    }

    private void MarkInnerAndOuter(Grid grid)
    {
        var verticalBorders = grid.Borders.Where(border => border.IsVertical);
        var horizontalBorders = grid.Borders.Where(border => border.IsHorizontal);

        for (var i = 0; i < grid.NodesPerColumn; i++)
        {
            for (var j = 0; j < grid.NodesPerRow; j++)
            {
                var intersectionsNumber = verticalBorders
                    .Where(border => border.Line.YProjection.Has(grid[i, j].Y))
                    .Where(border =>
                        Math.Abs(grid[i, j].Y - border.Line.Begin.Y) > CalculusConfig.Eps &&
                        Math.Abs(grid[i, j].Y - border.Line.End.Y) > CalculusConfig.Eps)
                    .Count(border => grid[i, j].X <= border.Line.Begin.X);

                intersectionsNumber += verticalBorders
                    .Where(border => border.Line.YProjection.Has(grid[i, j].Y))
                    .Where(border =>
                        Math.Abs(grid[i, j].Y - border.Line.Begin.Y) < CalculusConfig.Eps ||
                        Math.Abs(grid[i, j].Y - border.Line.End.Y) < CalculusConfig.Eps)
                    .Count(border => grid[i, j].X <= border.Line.Begin.X) / 2;

                if (intersectionsNumber % 2 == 1)
                {
                    grid[i, j] = grid[i, j] with { Type = NodeType.Inner };

                }
                else
                {
                    var liesOnBorder = horizontalBorders
                        .Where(border =>
                            Math.Abs(border.Line.Begin.Y - grid[i, j].Y) < CalculusConfig.Eps ||
                            Math.Abs(border.Line.End.Y - grid[i, j].Y) < CalculusConfig.Eps)
                        .Any(border => border.Line.XProjection.Has(grid[i, j].X));

                    liesOnBorder |= verticalBorders
                        .Where(border =>
                            Math.Abs(border.Line.Begin.X - grid[i, j].X) < CalculusConfig.Eps ||
                            Math.Abs(border.Line.End.X - grid[i, j].X) < CalculusConfig.Eps)
                        .Any(border => border.Line.YProjection.Has(grid[i, j].Y));

                    if (liesOnBorder) grid[i, j] = grid[i, j] with { Type = NodeType.Inner };
                    else grid[i, j] = grid[i, j] with { Type = NodeType.Fictitious };
                }
            }
        }
    }

    private void MarkBorderNodes(Grid grid)
    {
        for (var i = 0; i < grid.NodesPerColumn; i++)
        {
            for (var j = 0; j < grid.NodesPerRow; j++)
            {
                var node = grid[i, j];

                if (node.Type is not NodeType.Inner)
                {
                    continue;
                }

                foreach (var border in grid.Borders
                             .Where(x => x.Line.XProjection.Has(node.X))
                             .Where(x => x.IsHorizontal))
                {
                    if (Math.Abs(node.Y - border.Line.Begin.Y) > CalculusConfig.Eps)
                        continue;

                    border.BelongedNodeIndexes.Add(new ValueTuple<int, int>(item1: i, item2: j));
                    if (grid[i, j].Type is not NodeType.Edge)
                        grid[i, j] = node with { Type = NodeType.Edge };
                }

                foreach (var border in grid.Borders
                             .Where(x => x.Line.YProjection.Has(node.Y))
                             .Where(x => x.IsVertical))
                {
                    if (Math.Abs(node.X - border.Line.Begin.X) > CalculusConfig.Eps)
                        continue;

                    border.BelongedNodeIndexes.Add(new ValueTuple<int, int>(item1: i, item2: j));
                    if (grid[i, j].Type is not NodeType.Edge)
                        grid[i, j] = node with { Type = NodeType.Edge };
                }
            }
        }
    }

    private Point2D<int> GetTotalNodes(IRectangularLikeArea area)
    {
        return new Point2D<int>(
            SplitParameter.X.Splitters.Sum(x => x.Steps) + 1,
            SplitParameter.Y.Splitters.Sum(y => y.Steps) + 1
        );
    }

}