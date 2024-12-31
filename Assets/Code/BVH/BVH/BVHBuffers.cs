using System;
using UnityEngine;

namespace Code.Components.MortonCodeAssignment
{
    public class BVHBuffers : IDisposable
    {
        public readonly ComputeBuffer RootNodeIndices;
        public readonly ComputeBuffer ModelMatrices;
        public readonly ComputeBuffer BottomLevel;

        public BVHBuffers(ComputeBuffer bottomLevel, ComputeBuffer rootNodeIndices, ComputeBuffer modelMatrices)
        {
            RootNodeIndices = rootNodeIndices;
            ModelMatrices = modelMatrices;
            BottomLevel = bottomLevel;
        }

        public void Dispose()
        {
            RootNodeIndices.Dispose();
            ModelMatrices.Dispose();
            BottomLevel.Dispose();
        }
    }
}