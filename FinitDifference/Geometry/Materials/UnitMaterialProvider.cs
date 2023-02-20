namespace FinitDifference.Geometry.Materials;

public class UnitMaterialProvider : IMaterialProvider
{
    public Material GetMaterialByIndexes(int row, int column)
    {
        return new Material(1, 1);
    }
}