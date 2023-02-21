namespace FinitDifference.Geometry.GridComponents;

public class Grid
{
    public Node this[int row, int column]
    {
        get => _nodes[row, column];
        set => _nodes[row, column] = value;
    }

    public int NodesPerRow => _nodes.GetLength(1);
    public int NodesPerColumn => _nodes.GetLength(0);
    public int NodesCount => _nodes.Length;
    public Border[] Borders { get; }

    private readonly Node[,] _nodes;

    public Grid(Node[,] nodes, Border[] borders)
    {
        _nodes = nodes;
        Borders = borders;
    }

}