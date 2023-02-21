using FinitDifference.Calculus;
using FinitDifference.Calculus.Function;
using FinitDifference.Geometry.Areas;
using FinitDifference.Geometry.Base;
using FinitDifference.Geometry.GridBuilders;
using FinitDifference.Geometry.GridComponents;
using FinitDifference.Geometry.Materials;
using System;
using System.Globalization;
using System.Threading;
using FinitDifference.Geometry.GridBuilders.Splitting;

namespace FinitDifference;

internal class Program
{
    private static void Main(string[] args)
    {
        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("EN-US");
        var area = new ГArea(new Point2D[]
        {
            new (3d, 2d),
            new (6d, 2d),
            new (6d, 4d),
            new (9d, 4d),
            new (9d, 6d),
            new (3d, 6d)
        });

        Grid grid = new RectangularGridBuilder(
                new Point2D<AxisSplitParameter>(
                    X: new AxisSplitParameter(
                        new[] { 3d, 6d, 9d },
                        new IIntervalSplitter[]
                        {
                            new ProportionalSplitter(steps: 4, dischargeRatio:0.4d),
                            new UniformSplitter(Steps: 2),
                        }
                    ),
                    Y: new AxisSplitParameter(
                        new[] { 2d, 4d, 6d },
                        new IIntervalSplitter[]
                        {
                            new UniformSplitter(Steps: 2),
                            new UniformSplitter(Steps: 2),
                        }
                    )
                ),
                new UnitMaterialProvider())
            .Build(area);

        //new AnalyticSourceFunction(p =>
        //    Math.Pow(p.X, 4) + Math.Pow(p.Y, 4) - Math.Pow(12 * p.X, 2) - Math.Pow(12 * p.Y, 6)

        var matrix = new MatrixBuilder(new AnalyticSourceFunction(p =>
                Math.Exp(p.X * p.Y) * (-Math.Pow(p.X, 2) - Math.Pow(p.Y, 2) + 1)))
            .FromGrid(grid);
    }
}