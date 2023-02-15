using System;
using System.Linq;
using FinitDifference.Geometry.Areas;
using FinitDifference.Geometry.GridComponents;
using FinitDifference.Geometry.Materials;

namespace FinitDifference.Geometry.GridBuilders;

public abstract class GridBuilderBase : IGridBuilder
{
    protected readonly AxisSplitParameter SplitParameter;
    protected readonly IMaterialProvider MaterialProvider;
    protected IRectangularLikeArea Area { get; private set; }

    protected GridBuilderBase(AxisSplitParameter splitParameter, IMaterialProvider materialProvider)
    {
        SplitParameter = splitParameter;
        MaterialProvider = materialProvider;
    }

    public Grid Build(IRectangularLikeArea area)
    {
        Area = area;
        var grid = MakeGrid(area);
        MarkInnerAndOuter(grid);
        MarkBorderNodes(grid);

        return grid;
    }

    protected abstract Grid MakeGrid(IRectangularLikeArea area);

    private void MarkInnerAndOuter(Grid grid)
    {
        var verticalBorders = grid.Borders.Where(border => border.IsVertical);

        for (var i = 0; i < grid.NodesCount; i++)
        {
            for (var j = 0; j < grid.NodesPerColumn; j++)
            {
                var intersectionsNumber = verticalBorders.Count(border => grid[i, j].X < border.Line.Begin.X);
                if (intersectionsNumber % 2 == 0) grid[i, j] = grid[i, j] with { Type = NodeType.Fictitious };
                else grid[i, j] = grid[i, j] with { Type = NodeType.Inner };
            }
        }
    }

    private Grid MarkBorderNodes(Grid grid)
    {
        for (var i = 0; i < grid.NodesPerRow; i++)
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

        throw new NotImplementedException();
    }
}