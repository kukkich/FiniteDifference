namespace FinitDifference.Geometry.Base;

public class Interval
{
    public double Begin { get; }
    public double End { get; }

    public double Length => End - Begin;

    public Interval(double begin, double end)
    {
        Begin = begin;
        End = end;
    }

    public bool Has(double value)
    {
        return value >= Begin && value <= End;
    }
}