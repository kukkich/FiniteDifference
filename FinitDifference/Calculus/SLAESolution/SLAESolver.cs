namespace FinitDifference.Calculus.SLAESolution;

public class SLAESolver
{
    public static Vector SolveSLAE(BlockDiagonalMatrix blockDiagMatrix, Vector r, int k0, int k1)
    {
        var z = CalcZ(blockDiagMatrix, r, k0, k1);
        return CalcY(blockDiagMatrix, z, k0, k1);
    }

    private static Vector CalcZ(BlockDiagonalMatrix blockDiagMatrix, Vector r, int k0, int k1)
    {
        var z = r;
        var j = 1;

        z[0] = r[0] / blockDiagMatrix[2, k0];

        for (var i = k0 + 1; i < k1; i++, j++)
        {
            z[j] = (r[j] - blockDiagMatrix[1, i] * z[j - 1]) / blockDiagMatrix[2, i];
        }

        return z;
    }

    private static Vector CalcY(BlockDiagonalMatrix blockDiagMatrix, Vector z, int k0, int k1)
    {
        var j = blockDiagMatrix.BlockSize;
        var y = z;

        y[j - 1] = z[j - 1];
        j -= 2;

        for (var i = k1 - 2; i >= k0; i--, j--)
        {
            y[j] -= blockDiagMatrix[3, i] * y[j + 1];
        }

        return y;
    }
}