using System;
using FinitDifference.Calculus.Base;
using FinitDifference.Calculus.BoundaryConditions;
using FinitDifference.Geometry.Base;
using FinitDifference.Geometry.GridComponents;

namespace FinitDifference.Calculus;

public class MatrixBuilder
{
    public static double TooBigNumber = 1e14;
    private DiagonalMatrix _matrix;
    private Vector _rightSide;
    private Grid _grid;

    public MatrixBuilder FromGrid(Grid grid)
    {
        _grid = grid;
        _matrix = new DiagonalMatrix(grid.NodesCount, grid.NodesPerRow);
        _rightSide = Vector.Create(grid.NodesCount);

        for (int i = 1; i < grid.NodesPerRow - 1; i++)
        {
            for (int j = 1; j < grid.NodesPerColumn - 1; j++)
            {
                var node = grid[i, j];
                var material = node.Material;
                GetCoefficients(i, j, out var Kx, out var Ky );

                var valuesForInsert = new []
                {
                    -1 * material.Lambda * Ky.Previous,
                    
                    -1 * material.Lambda * Kx.Previous,
                    material.Lambda * (Kx.Current + Ky.Current) + material.Gamma,
                    -1 * material.Lambda * Kx.Next,

                    -1 * material.Lambda * Ky.Next,
                };

                _matrix.AddRow(valuesForInsert, GetGlobalIndex(i, j));
            }
        }
        
        return this;
    }

    private void GetCoefficients(
        int row, int column,
        out AxisCoefficients xCoefficients, out AxisCoefficients yCoefficients
        )
    {
        var hxPrev = _grid[row, column].X - _grid[row, column - 1].X;
        var hxNext = _grid[row, column + 1].X - _grid[row, column].X;

        xCoefficients = new AxisCoefficients
        {
            Previous = 2d / (hxPrev * (hxPrev + hxNext)),
            Current = 2d / (hxNext * hxPrev),
            Next = 2d / (hxNext * (hxPrev + hxNext))
        };

        var hyPrev = _grid[row, column].Y - _grid[row - 1, column].Y;
        var hyNext = _grid[row + 1, column].Y - _grid[row, column].Y;

        yCoefficients = new AxisCoefficients
        {
            Previous = 2d / (hyPrev * (hyPrev + hyNext)),
            Current = 2d / (hyNext * hyPrev),
            Next = 2d / (hyNext * (hyPrev + hyNext))
        };
    }

    public MatrixBuilder ApplyFirstBoundary(FixedValue[] conditions)
    {
        foreach (var condition in conditions)
        {
            var border = _grid.Borders[condition.BorderIndex];

            foreach (var index in border.BelongedNodeIndexes)
            {
                var node = _grid[index.row, index.column];
                var value = condition.Func(node);
                var globalIndex = GetGlobalIndex(index.row, index.column);

                _matrix[2, globalIndex] = TooBigNumber;
                _rightSide[globalIndex] = value * TooBigNumber;
            }
        }

        return this;
    }

    public MatrixBuilder ApplySecondBoundary(FixedFlow[] conditions)
    {
        foreach (var condition in conditions)
        {
            var border = _grid.Borders[condition.BorderIndex];

            foreach (var index in border.BelongedNodeIndexes)
            {
                var node = _grid[index.row, index.column];

                Node neighborNode = border.NormalOrientation switch
                {
                    NormalOrientation.Left => _grid[index.row, index.column + 1],
                    NormalOrientation.Right => _grid[index.row, index.column - 1],
                    NormalOrientation.Up => _grid[index.row - 1, index.column],
                    NormalOrientation.Down => _grid[index.row + 1, index.column],
                    _ => throw new NotSupportedException()
                };

                // Todo
                // в зависимости от ореинтации границы
                // получить значение потока (передать в функцию x или y)
                // и применить краевое

            }
        }
        throw new NotImplementedException();
    }

    public EquationData Build()
    {
        return new EquationData(_matrix, null, null);
    }

    private int GetGlobalIndex(int row, int column) => _grid.NodesPerRow * row + column;

    private readonly ref struct AxisCoefficients
    {
        public required double Previous { get; init; }
        public required double Current { get; init; }
        public required double Next { get; init; }
    }
}