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
                            new UniformSplitter(Steps: 2),
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

        var equation = SquareFuncTest(grid);

        var solution = equation.Solution;
        for (int i = 0; i < grid.NodesPerColumn; i++)
        {
            for (int j = 0; j < grid.NodesPerRow; j++)
            {
                var node = grid[i, j];
                var globalIndex = i * grid.NodesPerRow + j;
                if (node.Type is NodeType.Inner)
                    Console.WriteLine($"({node.X:F1}, {node.Y:F1}) {solution[globalIndex]:E14}");
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
            .ApplyFirstBoundary(new FixedValue[]
            {
                new (0, x => x + 2d),
                new (1, y => y + 6d),
                new (2, x => x + 4d),
                new (3, y => y + 9d),
                new (4, x => x + 6d),
                new (5, y => y + 3d),
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
                new (1, y => y*y + 6d*6),
                new (2, x => x*x + 4d*4),
                new (3, y => y*y + 9d*9),
                new (4, x => x*x + 6d*6),
                new (5, y => y*y + 3d*3),
            })
            .Build();

        var blockRelaxation = new BlockRelaxation(1.5d);
        blockRelaxation.GetSolution(equation.Matrix, equation.RightSide, equation.Solution, equation.Matrix.Padding,
            10000, 1e-20d);

        Console.WriteLine();

        return equation;
    }

}