using System;
using System.Collections.Generic;
using Code.Data;

namespace Code.Components.MortonCodeAssignment
{
    public class BVHContent : IDisposable
    {
        public List<AABB> BoundingBoxes = new();

        public int Count => BoundingBoxes.Count;

        public void Dispose()
        {
            BoundingBoxes.Clear();
        }
    }
}