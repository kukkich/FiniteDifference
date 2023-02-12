using System;

namespace FinitDifference.Calculus.Equation;

public class BinaryEquationSolver
{
    public static double Eps = 1e-15;
    public double Solve(double left, double right, Func<double, double> f)
    {
        if (f(left) < 0d && f(right) > 0d)
        {
            while (right - left > Eps)
            {
                var middle = (right + left) / 2d;
                if (f(middle) > 0)
                    right = middle;
                else
                    left = middle;
            }
        }
        else if (f(left) > 0d && f(right) < 0d)
        {
            while (right - left > Eps)
            {
                var middle = (right + left) / 2d;
                if (f(middle) > 0)
                    left = middle;
                else
                    right = middle;
            }
        }
        else
        {
            throw new Exception("No Roots");
        }
        return (left + right) / 2d;
    }
}