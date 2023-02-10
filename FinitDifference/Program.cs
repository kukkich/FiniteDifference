using FinitDifference.Geometry;

namespace FinitDifference;

internal class Program
{
    static void Main(string[] args)
    {
        var area = new ГArea(new Point2D[]
        {
            new (1d, 3d),
            new (2d, 3d),
            new (2d, 4d),
            new (4d, 4d),
            new (1d, 7d),
            new (4d, 7d)
        });

        Grid grid = new UniformGridBuilder(new AxisSplitParameter(2, 2))
            .Build(area);

        foreach (var node in grid.Nodes)
        {
            Console.WriteLine($"{node.X} {node.Y} {area.Contains(new Point2D(node.X, node.Y))}");
        }
    }
}
