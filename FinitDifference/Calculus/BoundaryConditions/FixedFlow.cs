using FinitDifference.Geometry.Base;
using System;

namespace FinitDifference.Calculus.BoundaryConditions;

public record FixedFlow(int BorderIndex, Func<Point2D, double> Func);