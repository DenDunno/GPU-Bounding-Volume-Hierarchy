using System;
using Code.Data;
using Code.Utils.Extensions;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Code.Utils.GPUShaderEmulator
{
    public struct PlocPlocSmartSearch : IBlockTask
    {
        [NativeDisableContainerSafetyRestriction] private NativeArray<BVHNode> _nodes;
        private NativeArray<int> _neighbours;
        private readonly int _leavesCount;
        private readonly int _encodeMask;
        private readonly int _blockSize;
        private readonly int _plocRange;
        private readonly int _radius;

        public PlocPlocSmartSearch(NativeArray<BVHNode> nodes, int leavesCount, int blockSize, int radius)
        {
            _neighbours = new NativeArray<int>(blockSize * 4 * radius, Allocator.TempJob);
            _plocRange = blockSize * 4 * radius;
            _encodeMask = ~(1 << radius - 1);
            _leavesCount = leavesCount;
            _blockSize = blockSize;
            _radius = radius;
            _nodes = nodes;
        }

        private bool IsInBounds(int id)
        {
            return id >= 0 && id < _leavesCount;
        }

        void InitializeNeighbours(int threadId)
        {
            for (int id = threadId; id < _plocRange; id += _blockSize)
            {
                _neighbours[id] = int.MaxValue;
            }
        }
        
        int EncodeOffsetIntoLowerBits(int id, int neighbor)
        {
            int signedOffset = neighbor - id;
            int signLastLowerBit = signedOffset >> 31;
            int valueUpperBits = (Math.Abs(signedOffset) - 1) << 1;
            return valueUpperBits | signLastLowerBit;
        }
        
        void UpdateNeighbourFromTheLeft(int selfId, int i, int distanceUpperBits)
        {
            int neighbourEncodedDistance = distanceUpperBits | EncodeOffsetIntoLowerBits(selfId + i, selfId);
            _neighbours.InterlockedMin(selfId + i, neighbourEncodedDistance);
        }

        void UpdateSelfBasedOnRightNeighbours(int id, int minDistanceIndex)
        {
            _neighbours.InterlockedMin(id, minDistanceIndex);
        }

        int GetDistanceToNeighbourUpperBits(int neighbourId, AABB box, int encodeMask)
        {
            float distance = box.Union(_nodes[neighbourId].Box).ComputeSurfaceArea();
            int positiveDistanceInteger = distance.UnsafeCast<int>() << 1;
            return positiveDistanceInteger & encodeMask;
        }

        void UpdateMinDistanceIndex(int id, int i, int distanceUpperBits, ref int minDistanceIndex)
        {
            int selfEncodedDistance = distanceUpperBits | EncodeOffsetIntoLowerBits(id, id + i);
            minDistanceIndex = Math.Min(minDistanceIndex, selfEncodedDistance);
        }

        void RunSearch(int threadId, int blockOffset)
        {
            int minDistanceIndex = int.MaxValue;

            for (int id = threadId; id < _blockSize + 3 * _radius; id += _blockSize)
            {
                AABB box = _nodes[id + blockOffset].Box;
                
                for (int i = 1; i <= _radius; i++)
                {
                    if (IsInBounds(id))
                    {
                        int distanceUpperBits = GetDistanceToNeighbourUpperBits(id + blockOffset, box, _encodeMask);
                        UpdateMinDistanceIndex(distanceUpperBits, id, i, ref minDistanceIndex);
                        UpdateNeighbourFromTheLeft(id, i, distanceUpperBits);
                    }
                }

                UpdateSelfBasedOnRightNeighbours(threadId, minDistanceIndex);
            }
        }
        
        int FindNearestNeighbour(int threadId, int blockOffset)
        {
            InitializeNeighbours(threadId);
            RunSearch(threadId, blockOffset);
            return _neighbours[threadId];
        }

        public void Execute(int threadsPerBlock, ThreadId threadId)
        {
            if (IsInBounds(threadId.Global))
            {
                BVHNode bvhNode = _nodes[threadId.Global];
                bvhNode.X = (uint)FindNearestNeighbour(threadId.Local, threadId.Group * threadsPerBlock);
                _nodes[threadId.Global] = bvhNode;   
            }
        }

        public void Dispose()
        {
            _neighbours.Dispose();
        }
    }
}