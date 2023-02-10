using FinitDifference.Geometry;

namespace FinitDifference.Calculus;

public class MatrixBuilder
{
    public DiagonalMatrix Build(Grid grid)
    {
        for (int i = 1; i < grid.Nodes.GetLength(0) - 1; i++)
        {
            for (int j = 0; j < grid.Nodes.GetLength(1); j++)
            {
                
            }
        }
        throw new NotImplementedException();
    }
}