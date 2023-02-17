using System;

namespace FinitDifference.Calculus.BoundaryConditions;

public record FixedValue(int BorderIndex, Func<double, double> Func);