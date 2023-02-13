using System;
using FinitDifference.Geometry.Areas;
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
        var nodes = SplitOnNodes(area);
        MarkInnerAndOuter(nodes);
        MarkBorderNodes(nodes);

        return new Grid(nodes);
    }

    protected abstract Node[,] SplitOnNodes(IRectangularLikeArea area);

    private void MarkInnerAndOuter(Node[,] nodes)
    {
        // Todo Сделать трассировкой лучей
        throw new NotImplementedException();
    }

    private void MarkBorderNodes(Node[,] nodes)
    {
        throw new NotImplementedException();
    }
}