using System;
using FinitDifference.Geometry.Base;

namespace FinitDifference.Calculus.BoundaryConditions;

public record FixedValue(int BorderIndex, Func<Point2D, double> Func);