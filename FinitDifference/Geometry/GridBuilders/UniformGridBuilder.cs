using FinitDifference.Geometry.Areas;
using FinitDifference.Geometry.Materials;

namespace FinitDifference.Geometry.GridBuilders;

public class UniformGridBuilder : IGridBuilder
{
    private readonly AxisSplitParameter _splitParameter;
    private readonly IMaterialProvider _materialProvider;

    public UniformGridBuilder(AxisSplitParameter splitParameter, IMaterialProvider materialProvider)
    {
        _splitParameter = splitParameter;
        _materialProvider = materialProvider;
    }

    public Grid Build(IRectangularLikeArea area)
    {
        var stepSize = new Point2D(
            CalcStep(area.LeftBottom.X, area.RightBottom.X, _splitParameter.XSteps),
            CalcStep(area.LeftBottom.Y, area.LeftTop.Y, _splitParameter.YSteps)
        );

        return new Grid(GenerateNodes(area, stepSize));
    }

    private static double CalcStep(double loweBound, double upperBound, int stepsCount)
    {
        return (upperBound - loweBound) / stepsCount;
    }

    private GridNode[,] GenerateNodes(IRectangularLikeArea area, Point2D stepSize)
    {
        var nodes = new GridNode[_splitParameter.YSteps + 1, _splitParameter.XSteps + 1];
        for (var i = 0; i <= _splitParameter.YSteps; i++)
        {
            for (var j = 0; j <= _splitParameter.XSteps; j++)
            {
                var x = area.LeftBottom.X + stepSize.X * j;
                var y = area.LeftBottom.Y + stepSize.Y * i;
                var point = new Point2D(x, y);

                var isFictitious = area.Contains(point);
                var material = _materialProvider.GetMaterialByNodeIndexes(i, j);

                nodes[i, j] = new GridNode(point, isFictitious, material);
            }
        }

        return nodes;
    }
}