using System;
using System.Collections.Generic;
using FinitDifference.Geometry.Base;

namespace FinitDifference.Geometry.GridBuilders.Splitting;

public readonly  record struct ProportionalSplitter : IIntervalSplitter
{
    public int Steps { get; }
    public double DischargeRatio { get; }

    private readonly double _lengthCoefficient;
    
    public ProportionalSplitter(int steps, double dischargeRatio)
    {
        if (Math.Abs(DischargeRatio - 1d) < CalculusConfig.Eps)
            throw new NotSupportedException();

        Steps = steps;
        DischargeRatio = dischargeRatio;
        _lengthCoefficient = (DischargeRatio - 1d) / (Math.Pow(DischargeRatio, Steps) - 1d);
    }

    public IEnumerable<double> EnumerateValues(Interval interval)
    {
        var step = interval.Length * _lengthCoefficient;

        var stepNumber = 0;
        var value = interval.Begin;

        while (interval.Has(value))
        {
            yield return value;

            value = interval.Begin + Math.Pow(stepNumber, step);
            stepNumber++;
        }
    }
}