﻿using System;

namespace FinitDifference.Calculus.Base;

public class DiagonalMatrix
{
    private const int DiagonalsNumber = 5;
    private double[,] _diagonals;
    public int Padding { get; }
    public int BlockSize { get; }
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

    public int CountColumns()
    {
        return _diagonals.GetLength(1);
    }

    //Todo override for Span
    public void AddRow(double[] values, int rowIndex)
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

    public DiagonalMatrix(double[,] diagonals, int padding, int blockSize)
    {
        var x = diagonals.GetLength(0);
        if (diagonals is null || diagonals.GetLength(0) != DiagonalsNumber)
            throw new ArgumentException(nameof(diagonals));
        if (Padding < 0) throw new ArgumentOutOfRangeException(nameof(padding));
        if (diagonals.GetLength(1) % blockSize > 0)
            throw new ArgumentException(nameof(blockSize));
        _diagonals = diagonals;
        Padding = padding;
        BlockSize = blockSize;
        _privateIndexes = GetIndexes();
    }

    public DiagonalMatrix(int size, int padding, int blockSize)
        : this(new double[DiagonalsNumber, size], padding, blockSize)
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

    public void DecomposeLU()
    {
        var n = CountColumns() / BlockSize;
        for (var i = 0; i < n; i++)
        {
            var k0 = i * BlockSize;
            var k1 = (i + 1) * BlockSize;
            for (var j = k0 + 1; j < k1; j++)
            {
                _diagonals[3, j - 1] /= _diagonals[2, j - 1];
                _diagonals[2, j] -= _diagonals[3, j - 1] * _diagonals[1, j];
            }
        }
    }

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