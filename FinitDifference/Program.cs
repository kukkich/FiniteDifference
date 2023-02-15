using System;
using FinitDifference.Calculus;
using FinitDifference.Calculus.Base;
using FinitDifference.Calculus.Equation;
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
        var area = new ГArea(new Point2D[]
        {
            new (3d, 2d),
            new (6d, 2d),
            new (6d, 4d),
            new (9d, 4d),
            new (9d, 6d),
            new (3d, 6d)
        });

        Grid grid = new UniformGridBuilder(new AxisSplitParameter(2, 2), new UnitMaterialProvider())
            .Build(area);

        //var matrix = new MatrixBuilder().FromGrid(grid)
        //    .Build();

        //Console.WriteLine();
        //matrix.Print();

        //var x = new BinaryEquationSolver().Solve(
        //    1 + 1e-14,
        //    15,
        //    x => Math.Pow(x, 6) - 8d * x + 8d - 1d
        //);
        //Console.WriteLine($"{x:F5}");

        //var diagonalMatrix = new DiagonalMatrix(new[,]
        //    {
        //        { 0d, 0d, 0d, 1d, 1d, 1d },
        //        { 0d, 1d, 1d, 1d, 1d, 1d },
        //        { 10d, 10d, 10d, 10d, 10d, 10d },
        //        { 1d, 1d, 1d, 1d, 1d, 0d },
        //        { 1d, 1d, 1d, 0d, 0d, 0d }
        //    },
        //    2);

        //var blockRelaxation = new BlockRelaxation(1d);
        //var result = blockRelaxation.GetSolution(diagonalMatrix, new Vector(12d, 13d, 13d, 13d, 13d, 12d), new Vector(0d, 0d, 0d, 0d, 0d, 0d), 2,
        //    10000, 1e-20d);
    }
}