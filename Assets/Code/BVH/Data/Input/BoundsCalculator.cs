using Code.Data;

namespace Code.Components.MortonCodeAssignment
{
    public class BoundsCalculator
    {
        private readonly IBoundingBoxesInput _input;

        public BoundsCalculator(IBoundingBoxesInput input)
        {
            _input = input;
        }

        public AABB Compute()
        {
            AABB[] boxes = _input.Calculate();
            AABB bounds = boxes[0];

            for (int i = 1; i < boxes.Length; ++i)
            {
                bounds = bounds.Union(boxes[i]);
            }

            return bounds;
        }
    }
}