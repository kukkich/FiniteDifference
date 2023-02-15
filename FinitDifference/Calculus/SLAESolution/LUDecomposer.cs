using FinitDifference.Calculus.Base;

namespace FinitDifference.Calculus.SLAESolution;

public class LUDecomposer
{
    public static void DecomposeLU(DiagonalMatrix diagonalMatrix, int blockSize)
    {
        var n = diagonalMatrix.CountColumns() / blockSize;
        for (var i = 0; i < n; i++)
        {
            var k0 = i * blockSize;
            var k1 = (i + 1) * blockSize;
            for (var j = k0 + 1; j < k1; j++)
            {
                diagonalMatrix[3, j - 1] /= diagonalMatrix[2, j - 1];
                diagonalMatrix[2, j] -= diagonalMatrix[3, j - 1] * diagonalMatrix[1, j];
            }
        }
    }
}