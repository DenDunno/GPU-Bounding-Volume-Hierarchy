using Code.Data;

namespace Code.Components.MortonCodeAssignment
{
    public interface IBoundingBoxesInput
    {
        AABB[] Calculate();
        int Count { get; }
    }
}