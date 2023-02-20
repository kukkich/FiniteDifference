using System.Collections.Generic;
using FinitDifference.Geometry.Base;

namespace FinitDifference.Geometry.GridBuilders.Splitting;

public interface IIntervalSplitter
{
    public IEnumerable<double> EnumerateValues(Interval interval);
    public int Steps { get; }
}