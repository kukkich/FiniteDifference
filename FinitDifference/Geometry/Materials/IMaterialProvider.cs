namespace FinitDifference.Geometry.Materials;

public interface IMaterialProvider
{
    public Material GetMaterialByIndexes(int row, int column);
}