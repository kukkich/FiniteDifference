namespace FinitDifference.Geometry.Materials;

public class UnitMaterialProvider : IMaterialProvider
{
    public Material GetMaterialByNodeIndexes(int row, int column)
    {
        return new Material(1, 0);
    }
}