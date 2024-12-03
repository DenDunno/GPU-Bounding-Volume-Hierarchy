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
        [NativeDisableContainerSafetyRestriction] private NativeArray<int> _neighbours;
        private readonly int _leavesCount;
        private readonly int _encodeMask;
        private readonly int _blockSize;
        private readonly int _radiusShift;
        private readonly int _plocRange;
        private readonly int _radius;

        public PlocPlocSmartSearch(NativeArray<BVHNode> nodes, int leavesCount, int blockSize, int radiusShift)
        {
            _nodes = nodes;
            _blockSize = blockSize;
            _leavesCount = leavesCount;
            _radius = 1 << radiusShift;
            _radiusShift = radiusShift;
            _encodeMask = ~(1 << _radius - 1);
            _plocRange = blockSize * 4 * _radius;
            _neighbours = new NativeArray<int>(blockSize * 4 * _radius, Allocator.TempJob);
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
        
        int ExtractBits(int input, int bitOffset, int extractedBitsCount)
        {
            int mask = (1 << extractedBitsCount) - 1;
            int shiftedInput = input >> bitOffset;
            return shiftedInput & mask;
        }

        int ExtractLowestBit(int input)
        {
            return input & (~input + 1);
        }
        
        int DecodeOffsetFromLowerBits(int encodedValue)
        {
            int offset = ExtractBits(encodedValue, 1, _radiusShift) + 1;
            int sign = ExtractLowestBit(encodedValue);

            return sign == 0 ? -offset : offset;
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
            return DecodeOffsetFromLowerBits(_neighbours[threadId]) + threadId + blockOffset;
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