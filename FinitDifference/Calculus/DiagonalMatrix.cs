namespace FinitDifference.Calculus;

public class DiagonalMatrix
{
    private const int DiagonalsNumber = 5;
    public double[,] Diagonals { get; }
    public int Padding { get; }
    public int Size => Diagonals.GetLength(1);

    private readonly int[] _privateIndexes;

    public int[] GetIndexes()
    {
        return new []
        {
            -(1 + Padding),
            -1, 0, 1,
            1 + Padding,
        };
    }

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

            Diagonals[i, rowIndex] += values[i];
        }

    }

    public DiagonalMatrix(double[,] diagonals, int padding)
    {
        var x = diagonals.GetLength(0);
        if (diagonals is null || diagonals.GetLength(0) != DiagonalsNumber)
            throw new ArgumentException(nameof(diagonals));
        if (Padding < 0) throw new ArgumentOutOfRangeException(nameof(padding));
        Diagonals = diagonals;
        Padding = padding;
        _privateIndexes = GetIndexes();
    }

    public DiagonalMatrix(int size, int padding)
        : this(new double[DiagonalsNumber, size], padding)
        { }

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
                Console.Write($"{Diagonals[i, j]:F3} ");
            }
            Console.WriteLine();
        }
    }
}