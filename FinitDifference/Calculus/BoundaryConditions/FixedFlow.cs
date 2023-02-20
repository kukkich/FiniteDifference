using System;

namespace FinitDifference.Calculus.BoundaryConditions;

public record FixedFlow(int BorderIndex, Func<double, double> Func);