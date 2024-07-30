using System.Collections.Generic;
using Code.Data;

namespace Code.Components.MortonCodeAssignment
{
    public class BVH
    {
        public readonly IReadOnlyList<AABB> _boxes;

        public BVH(IReadOnlyList<AABB> boxes)
        {
            _boxes = boxes;
        }

        public void Rebuild()
        {
            
        }
    }
}