﻿namespace FinitDifference.Geometry;

public class Grid
{

    public GridNode this[int row, int column] => _nodes[row, column];
    public int NodesPerRow => _nodes.GetLength(0);
    public int NodesPerColumn => _nodes.GetLength(1);
    public int NodesCount => _nodes.Length;

    private readonly GridNode[,] _nodes;


    public Grid(GridNode[,] nodes)
    {
        _nodes = nodes;
    }


}