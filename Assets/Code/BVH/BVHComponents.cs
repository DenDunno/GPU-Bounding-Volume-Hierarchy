using System;
using System.Collections.Generic;
using Code.Data;
using Unity.Collections;

namespace Code.Components.MortonCodeAssignment
{
    public class BVHComponents : IDisposable
    {
        public List<AABB> BoundingBoxes = new();
        public readonly BVHAlgorithm Algorithm;
        public readonly BVHGPUBridge GPUBridge;

        public BVHComponents(int bufferSize)
        {
            BVHBuffers buffers = new(bufferSize);
            Algorithm = new BVHAlgorithm(BVHShaders.Load(), buffers);
            GPUBridge = new BVHGPUBridge(buffers.Nodes, buffers.Root, BoundingBoxes.Count - 1, BoundingBoxes);
        }

        public void Initialize()
        {
            if (BoundingBoxes.Count > 0)
            {
                Algorithm.Initialize();
                GPUBridge.SendBoxesToGPU();
            }
        }

        public void Dispose()
        {
            Algorithm.Dispose();
        }
    }
}