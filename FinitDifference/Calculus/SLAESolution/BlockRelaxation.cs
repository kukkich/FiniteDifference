using FinitDifference.Calculus.Base;
using System;
using System.Threading;

namespace FinitDifference.Calculus.SLAESolution;

public class BlockRelaxation
{
    private readonly double _relaxation;

    public BlockRelaxation(double relaxation)
    {
        _relaxation = relaxation;
    }

    public Vector GetSolution(DiagonalMatrix diagonalMatrix, Vector f, Vector startX, int blockSize, int maxIterations, double eps)
    {
        if (diagonalMatrix.CountColumns % blockSize > 0)
            throw new ArgumentException(nameof(blockSize));

        LUDecomposer.DecomposeLU(diagonalMatrix, blockSize);
        var x = startX;
        var residual = 1d;

        for (var iteration = 1; iteration <= maxIterations && residual > eps; iteration++)
        {
            x = Iterate(diagonalMatrix, x, f, blockSize, out residual);
            CourseHolder.HoldInCourse(iteration, residual);
        }

        return x;
    }

    private Vector Iterate(DiagonalMatrix diagonalMatrix, Vector currentX, Vector f, int blockSize, out double residual)
    {
        var n = diagonalMatrix.CountColumns;
        var indexes = diagonalMatrix.GetIndexes();

        residual = 0d;

        var sumOfSqVecF = 0d;
        var blocksNumber = n / blockSize;

        for (var i = 0; i < blocksNumber; i++)
        {
            var k0 = i * blockSize;
            var k1 = (i + 1) * blockSize;
            var r = CalcBlockPart(diagonalMatrix, currentX, k0, k1, blockSize);
            var bi = 0;
            for (var j = k0; j < k1; j++, bi++)
            {
                var sum = 0.0;
                for (var k = 0; k < 5; k++)
                {
                    if (indexes[k] + j < 0 || indexes[k] + j >= n) continue;
                    if (indexes[k] + j < k0 || indexes[k] + j >= k1)
                    {
                        sum += diagonalMatrix[k, j] * currentX[indexes[k] + j];
                    }
                }
                r[bi] = f[j] - (r[bi] + sum);
                residual += r[bi] * r[bi];
                r[bi] *= _relaxation;
                sumOfSqVecF += f[j] * f[j];
            }
            var x = SLAESolver.SolveSLAE(diagonalMatrix, r, k0, k1);
            bi = 0;
            for (var j = k0; j < k1; j++, bi++)
            {
                currentX[j] += x[bi];
            }
        }

        residual = Math.Sqrt(residual) / Math.Sqrt(sumOfSqVecF);
        return currentX;
    }

    private Vector CalcBlockPart(DiagonalMatrix diagonalMatrix, Vector x, int k0, int k1, int blockSize)
    {
        var n = diagonalMatrix.CountColumns;
        var indexes = diagonalMatrix.GetIndexes();

        var r = Vector.Create(blockSize);
        var k = 0;
        for (var i = k0; i < k1; i++, k++)
        {
            var sum = 0.0;
            for (var j = 2; j < 4; j++)
            {
                if (indexes[j] + i < 0 || indexes[j] + i >= n) continue;
                if (indexes[j] + i < k0 || indexes[j] + i >= k1) continue;
                if (j == 2)
                {
                    sum += 1.0 * x[indexes[j] + i];
                }
                else
                {
                    sum += diagonalMatrix[j, i] * x[indexes[j] + i];
                }
            }
            r[k] = sum;
        }

        var buf = r.Copy();
        k = 0;
        for (var i = k0; i < k1; i++, k++)
        {
            var sum = 0.0;
            for (var j = 1; j < 3; j++)
            {
                if (indexes[j] + i < 0 || indexes[j] + i >= n) continue;
                if (indexes[j] + i >= k0 && indexes[j] + i < k1)
                {
                    sum += diagonalMatrix[j, i] * buf[indexes[j] + k];
                }
            }
            r[k] = sum;
        }

        return r;
    }
}