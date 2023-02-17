using System;
using System.Collections.Generic;
using System.Data.Common;
using FinitDifference.Calculus.Base;
using FinitDifference.Calculus.BoundaryConditions;
using FinitDifference.Calculus.Function;
using FinitDifference.Geometry;
using FinitDifference.Geometry.GridComponents;

namespace FinitDifference.Calculus;

public class MatrixBuilder
{
    private readonly ISourceFunction _sourceFunction;
    public static double TooBigNumber = 1e14;
    private DiagonalMatrix _matrix;
    private Vector _rightSide;
    private Grid _grid;

    public MatrixBuilder(ISourceFunction sourceFunction)
    {
        _sourceFunction = sourceFunction;
    }

    public MatrixBuilder FromGrid(Grid grid)
    {
        _grid = grid;
        _matrix = new DiagonalMatrix(grid.NodesCount, grid.NodesPerRow);
        _rightSide = Vector.Create(grid.NodesCount);

        for (int i = 0; i < grid.NodesPerRow; i++)
        {
            for (int j = 0; j < grid.NodesPerColumn; j++)
            {
                var node = grid[i, j];
                var globalNodeIndex = GetGlobalIndex(i, j);
                if (node.Type is NodeType.Fictitious)
                {
                    var index = globalNodeIndex;
                    _matrix[2, index] = 1d;
                    _rightSide[index] = 0d;
                    continue;
                }

                _rightSide[globalNodeIndex] = _sourceFunction.CalculateIn(node);
                    
                if (node.Type is NodeType.Edge) continue;

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

                _matrix.SumRow(valuesForInsert, globalNodeIndex);
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
                var value = border.NormalOrientation switch
                {
                    NormalOrientation.Left or NormalOrientation.Right => condition.Func(node.Y),
                    NormalOrientation.Up or NormalOrientation.Down => condition.Func(node.X),
                    _ => throw new NotSupportedException()
                };
                var globalIndex = GetGlobalIndex(index.row, index.column);

                _matrix[2, globalIndex] = TooBigNumber;
                _rightSide[globalIndex] = value * TooBigNumber;
            }
        }

        return this;
    }

    public MatrixBuilder ApplySecondBoundary(IEnumerable<FixedFlow> conditions)
    {
        foreach (var condition in conditions)
        {
            var border = _grid.Borders[condition.BorderIndex];

            foreach (var (row, column) in border.BelongedNodeIndexes)
            {
                var node = _grid[row, column];
                var shift = GetNeighborIndexShift(border.NormalOrientation);
                
                Node neighbor = _grid[row + shift.row, column + shift.column];

                var coefficient = node.Material.Lambda * CalcStep(border.NormalOrientation, neighbor, node);

                var values = new double[5];
                values[2] = coefficient;
                switch (border.NormalOrientation)
                {
                    case NormalOrientation.Left:
                        values[3] = -1d * coefficient;
                        break;
                    case NormalOrientation.Right: 
                        values[1] = -1d * coefficient;
                        break;
                    case NormalOrientation.Down:
                        values[4] = -1d * coefficient;
                        break;
                    case NormalOrientation.Up:
                        values[0] = -1d * coefficient;
                        break;
                    default:
                        throw new NotSupportedException();
                };

                var globalIndex = GetGlobalIndex(row, column);
                _matrix.SumRow(values, globalIndex);
                
                _rightSide[globalIndex] = border.NormalOrientation switch
                {
                    NormalOrientation.Left or NormalOrientation.Right => condition.Func(node.Y),
                    NormalOrientation.Up or NormalOrientation.Down => condition.Func(node.X),
                    _ => throw new NotSupportedException()
                };

            }
        }

        return this;
    }

    public EquationData Build()
    {
        return new EquationData(_matrix, Vector.Create(_rightSide.Length), _rightSide);
    }

    private int GetGlobalIndex(int row, int column) => _grid.NodesPerRow * row + column;

    private (int row, int column) GetNeighborIndexShift(NormalOrientation orientation)
    {
        return orientation switch
        {
            NormalOrientation.Left => new ValueTuple<int, int>(item1: 0, item2: 1),
            NormalOrientation.Right => new ValueTuple<int, int>(item1: 0, item2: -1),
            NormalOrientation.Up => new ValueTuple<int, int>(item1: -1, item2: 0),
            NormalOrientation.Down => new ValueTuple<int, int>(item1: 1, item2: 0),
            _ => throw new NotSupportedException()
        };
    }

    private double CalcStep(NormalOrientation orientation, Node neighbor, Node node)
    {
        return orientation switch
        {
            NormalOrientation.Left => neighbor.X - node.X,
            NormalOrientation.Right => node.X - neighbor.X,
            NormalOrientation.Up => node.Y - neighbor.Y,
            NormalOrientation.Down => neighbor.Y - node.Y,
            _ => throw new NotSupportedException()
        };
    }

    private readonly ref struct AxisCoefficients
    {
        public required double Previous { get; init; }
        public required double Current { get; init; }
        public required double Next { get; init; }
    }
}