namespace FinitDifference.Geometry.Materials;

public interface IMaterialProvider
{
    public Material GetMaterialByNodeIndexes(int row, int column);
}