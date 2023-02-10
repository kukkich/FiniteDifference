namespace FinitDifference.Geometry;

public class UniformGridBuilder
{
    private readonly AxisSplitParameter _splitParameter;

    public UniformGridBuilder(AxisSplitParameter splitParameter)
    {
        _splitParameter = splitParameter;
    }

    public Grid Build(ГArea area)
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

    private GridNode[,] GenerateNodes(ГArea area, Point2D stepSize)
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
                
                nodes[i, j] = new GridNode(point, isFictitious);
            }
        }

        return nodes;
    }
}