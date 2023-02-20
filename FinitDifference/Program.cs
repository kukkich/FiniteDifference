using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using FinitDifference.Calculus;
using FinitDifference.Calculus.Base;
using FinitDifference.Calculus.BoundaryConditions;
using FinitDifference.Calculus.Equation;
using FinitDifference.Calculus.Function;
using FinitDifference.Calculus.SLAESolution;
using FinitDifference.Geometry;
using FinitDifference.Geometry.Areas;
using FinitDifference.Geometry.Base;
using FinitDifference.Geometry.GridBuilders;
using FinitDifference.Geometry.GridComponents;
using FinitDifference.Geometry.Materials;

namespace FinitDifference;

internal class Program
{
    static void Main(string[] args)
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
            .FromGrid(grid)
            //.ApplySecondBoundary(new List<FixedFlow>
            //{
            //    new(4, y => 1),
            //})
            //.ApplyFirstBoundary(new FixedValue[]
            //{
            //    new(0, x => Math.Pow(x, 4) + 16),
            //    new(1, y => 1296 + Math.Pow(y, 4)),
            //    new(2, x => Math.Pow(x, 4) + 256),
            //    new(3, y => 6561 + Math.Pow(y, 4)),
            //    new(4, x => Math.Pow(x, 4) + 1296),
            //    new(5, y => 81 + Math.Pow(y, 4))
            //})
            .ApplyFirstBoundary(new FixedValue[]
                {
                    new(0, x => Math.Exp(x * 2)),
                    new(1, y => Math.Exp(y * 6)),
                    new(2, x => Math.Exp(x * 4)),
                    new(3, y => Math.Exp(y * 9)),
                    new(4, x => Math.Exp(x * 6)),
                    new(5, y => Math.Exp(y * 3))
                })
            .Build();

        var blockRelaxation = new BlockRelaxation(1.5d);

        blockRelaxation.GetSolution(matrix.Matrix, matrix.RightSide, matrix.Solution, matrix.Matrix.Padding,
            10000, 1e-20d);

        Console.WriteLine();

        //for (var i = 0; i < matrix.Solution.Length; i++)
        //{
        //    if (Math.Abs(matrix.Solution[i]) > CalculusConfig.Eps) Console.WriteLine(matrix.Solution[i].ToString("E14", CultureInfo.CreateSpecificCulture("en-us")));
        //}

        for (int i = 0; i < grid.NodesPerRow; i++)
        {
            for (int j = 0; j < grid.NodesPerColumn; j++)
            {
                var node = grid[i, j];
                var globalIndex = i * grid.NodesPerRow + j;

                if (node.X == 4.5d && node.Y == 5d)
                {
                    Console.WriteLine($"(4.5, 5) {matrix.Solution[globalIndex]:E14}");
                }

                if (node.X == 6d && node.Y == 5d)
                {
                    Console.WriteLine($"(6, 5) {matrix.Solution[globalIndex]:E14}");
                }
            }
        }
    }
}