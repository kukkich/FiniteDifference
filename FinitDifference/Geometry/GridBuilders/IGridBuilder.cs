using FinitDifference.Geometry.Areas;

namespace FinitDifference.Geometry.GridBuilders;

public interface IGridBuilder
{
    public Grid Build(IRectangularLikeArea area);
}