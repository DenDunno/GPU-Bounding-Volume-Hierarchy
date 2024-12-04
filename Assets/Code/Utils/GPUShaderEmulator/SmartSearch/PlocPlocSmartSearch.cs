using System;
using Code.Data;
using Code.Utils.Extensions;

namespace Code.Utils.GPUShaderEmulator
{
    public struct PlocPlocSmartSearch : IBlockTask
    {
        private readonly PlocPlusPlusSmartSearchData _data;

        public PlocPlocSmartSearch(PlocPlusPlusSmartSearchData data)
        {
            _data = data;
        }
        
        uint ExtractBits(uint input, int bitOffset, int extractedBitsCount)
        {
            uint mask = (uint)((1 << extractedBitsCount) - 1);
            uint shiftedInput = input >> bitOffset;
            return shiftedInput & mask;
        }

        uint ExtractLowestBit(uint input)
        {
            return input & (~input - 1);
        }
        
        int DecodeOffsetFromLowerBits(uint encodedValue)
        {
            uint offset = ExtractBits(encodedValue, 1, _data.RadiusShift) + 1;
            uint sign = ExtractLowestBit(encodedValue);

            return (int)(sign == 0 ? -offset : offset);
        }
        
        uint EncodeOffsetIntoLowerBits(uint id, uint neighbor)
        {
            int signedOffset = (int)(neighbor - id);
            uint signLastLowerBit = (uint)((signedOffset >> 31) + 1);
            uint valueUpperBits = (uint)((Math.Abs(signedOffset) - 1) << 1);
            uint result = valueUpperBits | signLastLowerBit;
            return result;
        }
        
        void UpdateNeighbourFromTheLeft(uint selfId, uint i, uint distanceUpperBits)
        {
            uint neighbourEncodedDistance = distanceUpperBits | EncodeOffsetIntoLowerBits(selfId + i, selfId);
            _data.Neighbours.InterlockedMin(selfId + i, neighbourEncodedDistance);
        }

        void UpdateSelfBasedOnRightNeighbours(int id, uint minDistanceIndex)
        {
            _data.Neighbours.InterlockedMin(id, minDistanceIndex);
        }

        uint GetDistanceToNeighbourUpperBits(int neighbourId, AABB box, int encodeMask)
        {
            float distance = box.Union(_data.NeighboursBoxes[neighbourId]).ComputeSurfaceArea();
            uint castedValue = (uint)BitConverter.SingleToInt32Bits(distance);
            uint positiveDistanceInteger = castedValue << 1;
            return positiveDistanceInteger & (uint)encodeMask;
        }

        void UpdateMinDistanceIndex(uint id, uint i, uint distanceUpperBits, ref uint minDistanceIndex)
        {
            uint selfEncodedDistance = distanceUpperBits | EncodeOffsetIntoLowerBits(id, id + i);
            minDistanceIndex = Math.Min(minDistanceIndex, selfEncodedDistance);
        }
        
        public int FindNearestNeighbour(int threadId, int blockOffset)
        {
            return DecodeOffsetFromLowerBits(_data.Neighbours[threadId + 2 * _data.Radius]) + threadId + blockOffset;
        }

        public void Execute(int threadsPerBlock, ThreadId threadId)
        {
            uint minDistanceIndex = uint.MaxValue;

            for (int rangeId = threadId.Local; rangeId < _data.BlockSize + 3 * _data.Radius; rangeId += _data.BlockSize)
            {
                AABB box = _data.NeighboursBoxes[rangeId];
                
                for (int i = 1; i <= _data.Radius; i++)
                {
                    uint distanceUpperBits = GetDistanceToNeighbourUpperBits(rangeId + i, box, _data.EncodeMask);
                    UpdateMinDistanceIndex((uint)rangeId, (uint)i, distanceUpperBits, ref minDistanceIndex);
                    UpdateNeighbourFromTheLeft((uint)rangeId, (uint)i, distanceUpperBits);
                }

                UpdateSelfBasedOnRightNeighbours(rangeId, minDistanceIndex);
            }
        }
    }
}