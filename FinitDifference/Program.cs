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

        var matrix = new MatrixBuilder().FromGrid(grid)
            .ApplySecondBoundary()
            .ApplyFirstBoundary()
            .Build();

        var blockRelaxation = new BlockRelaxation(1d);
        var result = blockRelaxation.GetSolution(matrix.Matrix, new Vector(matrix.RightSide), new Vector(matrix.Solution), 2,
            10000, 1e-20d);
    }
}