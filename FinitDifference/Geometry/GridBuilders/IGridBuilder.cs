using FinitDifference.Geometry.Areas;
using FinitDifference.Geometry.GridComponents;

namespace FinitDifference.Geometry.GridBuilders;

public interface IGridBuilder
{
    public Grid Build(IRectangularLikeArea area);
}