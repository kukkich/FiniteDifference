namespace FinitDifference.Calculus;

public struct Vector: ICloneable
{
    public double[] VectorArray { get; set; }

    public Vector(double[] vectorArray)
    {
        VectorArray = vectorArray;
    }

    public Vector(int size) : this(new double[size]) { }

    public double this[int index]
    {
        get => VectorArray[index];
        set => VectorArray[index] = value;
    }

    public int Count => VectorArray.Length;

    public object Clone()
    {
        var clone = new double[Count];
        Array.Copy(VectorArray, clone, Count);

        return new Vector(clone);
    }
}