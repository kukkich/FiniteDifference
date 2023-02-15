namespace FinitDifference.Calculus;

public class DiagonalMatrix
{
    private const int DiagonalsNumber = 5;
    public double[,] Diagonals { get; }
    public int Padding { get; }

    public DiagonalMatrix(double[,] diagonals, int padding)
    {
        if (diagonals is null || diagonals.GetLength(0) != DiagonalsNumber)
            throw new ArgumentException();
        Diagonals = diagonals;
        Padding = padding;
    }

    public double this[int i, int j]
    {
        get => Diagonals[i, j];
        set => Diagonals[i, j] = value;
    }

    public int CountColumns()
    {
        return Diagonals.GetLength(1);
    }
}