using System;
using System.Collections.Generic;
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
        var area = new ГArea(new Point2D[]
        {
            new (3d, 2d),
            new (6d, 2d),
            new (6d, 4d),
            new (9d, 4d),
            new (9d, 6d),
            new (3d, 6d)
        });

        Grid grid = new UniformGridBuilder(new AxisSplitParameter(4, 4), new UnitMaterialProvider())
            .Build(area);

        var matrix = new MatrixBuilder(new AnalyticSourceFunction(p => p.X + p.Y))
            .FromGrid(grid)
            //.ApplySecondBoundary(new List<FixedFlow>
            //{
            //    new(0, x => x),
            //})
            .ApplyFirstBoundary(new FixedValue[]
            {
                new (0, x => x + 2),
                new (1, y => 6 + y),
                new (2, x => x + 4),
                new (3, y => 9 + y),
                new (4, x => x + 6),
                new (5, y => 3 + y)
            })
            .Build();

        var blockRelaxation = new BlockRelaxation(1.0d);

        blockRelaxation.GetSolution(matrix.Matrix, matrix.RightSide, matrix.Solution, matrix.Matrix.Padding,
            10000, 1e-20d);
    }
}