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
using FinitDifference.Calculus.BoundaryConditions;
using FinitDifference.Calculus.SLAESolution;
using FinitDifference.Geometry;
using FinitDifference.Geometry.GridBuilders.Splitting;
using System.Collections.Generic;
using FinitDifference.Calculus.Base;

namespace FinitDifference;

internal class Program
{
    private static void Main(string[] args)
    {
        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("EN-US");
        IRectangularLikeArea area = new ГArea(new Point2D[]
        {
            new (3d, 2d),
            new (6d, 2d),
            new (6d, 4d),
            new (9d, 4d),
            new (9d, 6d),
            new (3d, 6d)
        });
        area = new Rectangle(new Point2D[]
        {
            new (3d, 2d),
            new (9d, 2d),
            new (9d, 6d),
            new (3d, 6d)
        });
        Grid grid = new RectangularGridBuilder(
                new Point2D<AxisSplitParameter>(
                    X: new AxisSplitParameter(
                        new[] { 3d, 6d, 9d },
                        new IIntervalSplitter[]
                        {
                            new UniformSplitter(Steps: 1),
                            new UniformSplitter(Steps: 1),
                        }
                    ),
                    Y: new AxisSplitParameter(
                        new[] { 2d, 4d, 6d },
                        new IIntervalSplitter[]
                        {
                            new UniformSplitter(Steps: 1),
                            new UniformSplitter(Steps: 1),
                        }
                    )
                ),
                new UnitMaterialProvider())
            .Build(area);

        var equation = SquareFuncTest(grid);

        var solution = equation.Solution;
        for (int i = 0; i < grid.NodesPerColumn; i++)
        {
            for (int j = 0; j < grid.NodesPerRow; j++)
            {
                var node = grid[i, j];
                var globalIndex = i * grid.NodesPerRow + j;
                //if (node.Type is NodeType.Inner)
                    Console.WriteLine($"({node.X:F4}, {node.Y:F4}) {solution[globalIndex]:E14}");
            }
        }
    }

    private static EquationData LinearTest(Grid grid)
    {
        var f = new AnalyticSourceFunction(p =>
            p.X + p.Y
        );
        //Console.WriteLine();
        var equation = new MatrixBuilder(f)
            .FromGrid(grid)
            //.ApplyFirstBoundary(new FixedValue[]
            //{
            //    new (0, x => x + 2d),
            //    new (1, y => y + 6d),
            //    new (2, x => x + 4d),
            //    new (3, y => y + 9d),
            //    new (4, x => x + 6d),
            //    new (5, y => y + 3d),
            //})
            .ApplySecondBoundary(new FixedFlow[]
            {
                new(0, x => -1d),
                new(1, y => 1d),
                new(2, x => 1d),
                new(3, y => -1d),
            })
            .Build();

        var blockRelaxation = new BlockRelaxation(1.5d);
        blockRelaxation.GetSolution(equation.Matrix, equation.RightSide, equation.Solution, equation.Matrix.Padding,
            10000, 1e-20d);

        Console.WriteLine();

        return equation;
    }

    private static EquationData SquareFuncTest(Grid grid)
    {
        var f = new AnalyticSourceFunction(p =>
            (p.X * p.X + p.Y * p.Y) - 4d
        );
        
        //Console.WriteLine();
        var equation = new MatrixBuilder(f)
            .FromGrid(grid)
            .ApplyFirstBoundary(new FixedValue[]
            {
                new (0, x => x*x + 2d*2),
                new (1, y => y*y + 9d*9),
                new (2, x => x*x + 6d*6),
                new (3, y => y*y + 3d*3),
            })
            .Build();

        var blockRelaxation = new BlockRelaxation(1.5d);

        blockRelaxation.GetSolution(equation.Matrix, equation.RightSide, equation.Solution, equation.Matrix.Padding + 1,
            10000, 1e-20d);

        //var solver = new IterationSolver();
        //solver.GaussMethod(equation.Matrix, equation.Solution, equation.RightSide,
        //    new GaussMethodParams(1e-15, 1.2, 8000));

        Console.WriteLine();

        return equation;
    }

}

public record GaussMethodParams(
    double Accuracy,
    double Relaxation,
    int MaxIteration
);

public class IterationSolver
{
    private static readonly int[] IndexesMemory;

    static IterationSolver()
    {
        IndexesMemory = new int[DiagonalMatrix.DiagonalsNumber];
    }

    public Vector GaussMethod(DiagonalMatrix matrix, Vector x, Vector f, GaussMethodParams parameters)
    {
        var fNorm = VectorMath.GetNorm(f);
        var discrepancyNorm = fNorm;
        var relativeDiscrepancy = discrepancyNorm / fNorm;

        var indexes = matrix.GetIndexes();

        int k;
        for (k = 0; (k < parameters.MaxIteration) && (relativeDiscrepancy > parameters.Accuracy); ++k)
        {
            Iterate(matrix, x, f, x, parameters, indexes, out discrepancyNorm);

            relativeDiscrepancy = discrepancyNorm / fNorm;
        }

        return x;
    }

    private static void Iterate(
        DiagonalMatrix matrix,
        Vector x,
        Vector f,
        Vector prevX,
        GaussMethodParams parameters,
        IReadOnlyList<int> indexes,
        out double discrepancyNorm
        )
    {
        discrepancyNorm = 0;
        var n = x.Length;

        for (var i = 0; i < n; ++i)
        {
            double sum = 0;
            for (var j = 0; j < indexes.Count; ++j)
            {
                var index = indexes[j] + i;
                if (index < 0 || index >= n)
                    continue;

                sum += prevX[index] * matrix[j, i];
            }

            discrepancyNorm += (f[i] - sum) * (f[i] - sum);
            x[i] = prevX[i] + (f[i] - sum) * parameters.Relaxation / matrix[2, i];
        }

        discrepancyNorm = Math.Sqrt(discrepancyNorm);
    }
}

public static class VectorMath
{
    public static double GetNorm(Vector x)
    {
        var norm = 0d;

        for (int i = 0; i < x.Length; i++)
        {
            norm += x[i] * x[i];
        }

        norm = Math.Sqrt(norm);

        return norm;
    }
}