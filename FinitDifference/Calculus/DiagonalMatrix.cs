namespace FinitDifference.Calculus;

public class DiagonalMatrix
{
    private const int DiagonalsNumber = 5;
    public double[,] Diagonals { get; }
    public int Padding { get; }

    public DiagonalMatrix(double[,] diagonals, int padding)
    {
        if (Diagonals is null || Diagonals.GetLength(0) != DiagonalsNumber)
            throw new ArgumentException();
        Diagonals = diagonals;
        Padding = padding;
    }
}