using FinitDifference.Geometry;
using FinitDifference.Geometry.Materials;

namespace FinitDifference.Calculus;

public class MatrixBuilder
{
    private DiagonalMatrix _matrix;
    private Grid _processedGrid;

    public MatrixBuilder FromGrid(Grid grid)
    {
        _processedGrid = grid;
        _matrix = new DiagonalMatrix(grid.NodesCount, grid.NodesPerRow);

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

                _matrix.AddRow(valuesForInsert, grid.NodesPerRow * i + j);
            }
        }
        
        return this;
    }

    private void GetCoefficients(
        int row, int column,
        out AxisCoefficients xCoefficients, out AxisCoefficients yCoefficients
        )
    {
        var hxPrev = _processedGrid[row, column].X - _processedGrid[row, column - 1].X;
        var hxNext = _processedGrid[row, column + 1].X - _processedGrid[row, column].X;

        xCoefficients = new AxisCoefficients
        {
            Previous = 2d / (hxPrev * (hxPrev + hxNext)),
            Current = 2d / (hxNext * hxPrev),
            Next = 2d / (hxNext * (hxPrev + hxNext))
        };

        var hyPrev = _processedGrid[row, column].Y - _processedGrid[row - 1, column].Y;
        var hyNext = _processedGrid[row + 1, column].Y - _processedGrid[row, column].Y;

        yCoefficients = new AxisCoefficients
        {
            Previous = 2d / (hyPrev * (hyPrev + hyNext)),
            Current = 2d / (hyNext * hyPrev),
            Next = 2d / (hyNext * (hyPrev + hyNext))
        };
    }

    public MatrixBuilder ApplyBoundary()
    {
        throw new NotImplementedException();
    }

    public DiagonalMatrix Build()
    {
        return _matrix;
    }

    private readonly ref struct AxisCoefficients
    {
        public required double Previous { get; init; }
        public required double Current { get; init; }
        public required double Next { get; init; }
    }
}