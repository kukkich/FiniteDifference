using System;
using System.Globalization;

namespace FinitDifference.Calculus.SLAESolution;

public class CourseHolder
{
    public static void HoldInCourse(int iteration, double residual)
    {
        Console.Write("Iteration: {0} Residual: {1}   \r", iteration,
            residual.ToString("0.00000000000000e+00", CultureInfo.CreateSpecificCulture("en-US")));
    }
}