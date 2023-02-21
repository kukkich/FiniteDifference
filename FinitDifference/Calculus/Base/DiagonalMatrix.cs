using System;

namespace FinitDifference.Calculus.Base;

public class DiagonalMatrix
{
    public const int DiagonalsNumber = 5;
    private readonly double[,] _diagonals;
    public int Padding { get; }
    public int Size => _diagonals.GetLength(1);

    private readonly int[] _privateIndexes;

    public int[] GetIndexes()
    {
        return new[]
        {
            -(1 + Padding),
            -1, 0, 1,
            1 + Padding,
        };
    }

    public int CountColumns => _diagonals.GetLength(1);

    //Todo override for Span
    public void SumRow(double[] values, int rowIndex)
    {
        if (!IsValidIndex(rowIndex))
            throw new ArgumentOutOfRangeException(nameof(rowIndex));
        if (values.Length != DiagonalsNumber)
            throw new ArgumentOutOfRangeException(nameof(values));

        for (var i = 0; i < DiagonalsNumber; i++)
        {
            var indexInMatrix = _privateIndexes[i] + rowIndex;
            if (!IsValidIndex(indexInMatrix))
                continue;

            _diagonals[i, rowIndex] += values[i];
        }

    }

    public DiagonalMatrix(double[,] diagonals, int padding)
    {
        var x = diagonals.GetLength(0);
        if (diagonals is null || diagonals.GetLength(0) != DiagonalsNumber)
            throw new ArgumentException(nameof(diagonals));
        if (Padding < 0) throw new ArgumentOutOfRangeException(nameof(padding));

        _diagonals = diagonals;
        Padding = padding;
        _privateIndexes = GetIndexes();
    }

    public DiagonalMatrix(int size, int padding)
        : this(new double[DiagonalsNumber, size], padding)
    { }

    public double this[int i, int j]
    {
        get => _diagonals[i, j];
        set => _diagonals[i, j] = value;
    }

    private void AssertThatIndexesFallOnDiagonals(int row, int column)
    {
        if (row < 0 || column < 0)
            throw new ArgumentOutOfRangeException(nameof(row) + " and " + nameof(column));
        if (!(Math.Abs(row - column) <= 1 || Math.Abs(row - column) == Padding + 2))
            throw new ArgumentOutOfRangeException(nameof(row) + " and " + nameof(column));
    }

    private bool IsValidIndex(int index) => index >= 0 && index < Size;

    public void Print()
    {
        for (int i = 0; i < DiagonalsNumber; i++)
        {
            for (int j = 0; j < Size; j++)
            {
                Console.Write($"{_diagonals[i, j]:F3} ");
            }
            Console.WriteLine();
        }
    }
}