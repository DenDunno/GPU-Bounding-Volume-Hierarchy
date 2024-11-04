using System.Collections.Generic;
using Code.Data;

namespace Code.Components.MortonCodeAssignment
{
    public class BVHContent
    {
        public List<AABB> BoundingBoxes = new();

        public int Count => BoundingBoxes.Count;
    }
}