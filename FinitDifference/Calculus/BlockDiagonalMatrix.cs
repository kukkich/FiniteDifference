namespace FinitDifference.Calculus;

public class BlockDiagonalMatrix : DiagonalMatrix
{
    public int BlockSize { get; }

    public BlockDiagonalMatrix(int blockSize, double[,] diagonals, int padding) : base(diagonals, padding)
    {
        if(diagonals.GetLength(1) % blockSize > 0)
            throw new ArgumentException();
        BlockSize = blockSize;
    }

    public void LUDecomposition()
    {
        var n = Diagonals.GetLength(1) / BlockSize;
        for (var i = 0; i < n; i++)
        {
            var k0 = i * BlockSize;
            var k1 = (i + 1) * BlockSize;
            for (var j = k0 + 1; j < k1; j++)
            {
                Diagonals[3, j - 1] /= Diagonals[2, j - 1];
                Diagonals[2, j] -= Diagonals[3, j - 1] * Diagonals[1, j];
            }
        }
    }
}