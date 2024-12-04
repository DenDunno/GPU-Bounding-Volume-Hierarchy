using System;
using Code.Data;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Code.Utils.GPUShaderEmulator
{
    public struct PlocPlusPlusSmartSearchData : IDisposable
    {
        [NativeDisableContainerSafetyRestriction]
        public NativeArray<BVHNode> Nodes;

        [NativeDisableContainerSafetyRestriction]
        public NativeArray<uint> Neighbours;

        [NativeDisableContainerSafetyRestriction]
        public NativeArray<AABB> NeighboursBoxes;

        public readonly int LeavesCount;
        public readonly int EncodeMask;
        public readonly int BlockSize;
        public readonly int RadiusShift;
        public readonly int PLOCRange;
        public readonly int Radius;

        public PlocPlusPlusSmartSearchData(NativeArray<BVHNode> nodes, int leavesCount, int blockSize, int radiusShift)
        {
            Nodes = nodes;
            BlockSize = blockSize;
            LeavesCount = leavesCount;
            Radius = 1 << radiusShift;
            RadiusShift = radiusShift;
            EncodeMask = ~(1 << Radius - 1);
            PLOCRange = blockSize + 4 * Radius;
            Neighbours = new NativeArray<uint>(PLOCRange, Allocator.TempJob);
            NeighboursBoxes = new NativeArray<AABB>(PLOCRange, Allocator.TempJob);
        }

        public bool IsInBounds(int id)
        {
            return id >= 0 && id < LeavesCount;
        }

        public void Dispose()
        {
            Neighbours.Dispose();
            NeighboursBoxes.Dispose();
        }

        public int ComputeLeafIndex(int threadId)
        {
            return LeavesCount + threadId;
        }
    }
}