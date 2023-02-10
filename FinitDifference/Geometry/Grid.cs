namespace FinitDifference.Geometry;

public class Grid
{
    public GridNode[,] Nodes { get; }

    public Grid(GridNode[,] nodes)
    {
        Nodes = nodes;
    }
}