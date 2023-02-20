using FinitDifference.Calculus;
using FinitDifference.Calculus.Function;
using FinitDifference.Geometry;
using FinitDifference.Geometry.Areas;
using FinitDifference.Geometry.Base;
using FinitDifference.Geometry.GridBuilders;
using FinitDifference.Geometry.GridComponents;
using FinitDifference.Geometry.Materials;
using System;
using System.Globalization;
using System.Threading;

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

        Grid grid = new UniformGridBuilder(new AxisSplitParameter(64, 64), new UnitMaterialProvider())
            .Build(area);

        //new AnalyticSourceFunction(p =>
        //    Math.Pow(p.X, 4) + Math.Pow(p.Y, 4) - Math.Pow(12 * p.X, 2) - Math.Pow(12 * p.Y, 6)

        var matrix = new MatrixBuilder(new AnalyticSourceFunction(p =>
                Math.Exp(p.X * p.Y) * (-Math.Pow(p.X, 2) - Math.Pow(p.Y, 2) + 1)))
            .FromGrid(grid);

    }
}