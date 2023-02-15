using FinitDifference.Calculus.Base;

namespace FinitDifference.Calculus.SLAESolution;

public class SLAESolver
{
    public static Vector SolveSLAE(DiagonalMatrix diagonalMatrix, Vector r, int k0, int k1)
    {
        var z = CalcZ(diagonalMatrix, r, k0, k1);
        return CalcY(diagonalMatrix, z, k0, k1);
    }

    private static Vector CalcZ(DiagonalMatrix diagonalMatrix, Vector r, int k0, int k1)
    {
        var z = r;
        var j = 1;

        z[0] = r[0] / diagonalMatrix[2, k0];

        for (var i = k0 + 1; i < k1; i++, j++)
        {
            z[j] = (r[j] - diagonalMatrix[1, i] * z[j - 1]) / diagonalMatrix[2, i];
        }

        return z;
    }

    private static Vector CalcY(DiagonalMatrix diagonalMatrix, Vector z, int k0, int k1)
    {
        var j = diagonalMatrix.BlockSize;
        var y = z;

        y[j - 1] = z[j - 1];
        j -= 2;

        for (var i = k1 - 2; i >= k0; i--, j--)
        {
            y[j] -= diagonalMatrix[3, i] * y[j + 1];
        }

        return y;
    }
}