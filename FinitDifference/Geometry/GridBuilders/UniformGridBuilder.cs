using System;
using FinitDifference.Geometry.Areas;
using FinitDifference.Geometry.Base;
using FinitDifference.Geometry.Materials;

namespace FinitDifference.Geometry.GridBuilders;

public class UniformGridBuilder : GridBuilderBase
{
    public UniformGridBuilder(AxisSplitParameter splitParameter, IMaterialProvider materialProvider)
        : base(splitParameter, materialProvider)
        { }

    protected override Node[,] SplitOnNodes(IRectangularLikeArea area)
    {
        var stepSize = new Point2D(
            CalcStep(Area.LeftBottom.X, Area.RightBottom.X, SplitParameter.XSteps),
            CalcStep(Area.LeftBottom.Y, Area.LeftTop.Y, SplitParameter.YSteps)
        );

        var nodes = new Node[SplitParameter.YSteps + 1, SplitParameter.XSteps + 1];
        for (var i = 0; i <= SplitParameter.YSteps; i++)
        {
            for (var j = 0; j <= SplitParameter.XSteps; j++)
            {
                var x = Area.LeftBottom.X + stepSize.X * j;
                var y = Area.LeftBottom.Y + stepSize.Y * i;
                var point = new Point2D(x, y);

                var material = MaterialProvider.GetMaterialByNodeIndexes(i, j);

                nodes[i, j] = new Node(point, NodeType.Undefined, material);
            }
        }

        throw new NotImplementedException();
    }

    private static double CalcStep(double loweBound, double upperBound, int stepsCount)
    {
        return (upperBound - loweBound) / stepsCount;
    }
}