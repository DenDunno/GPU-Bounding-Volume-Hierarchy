#include "..//NodesInput.hlsl"
#include "..//..//Utilities/BitManipulation.hlsl"

#ifndef THREADS
#define THREADS 256
#endif
#define UINT_MAX_VALUE -1
#define RADIUS_SHIFT 4
static const uint RADIUS = 1 << RADIUS_SHIFT;
static const uint PLOC_OFFSET = 2 * RADIUS;
static const uint PLOC_RANGE_SIZE = THREADS + 4 * RADIUS;
static const uint ENCODE_MASK = ~((1u << RADIUS_SHIFT + 1) - 1);
groupshared uint Neighbours[PLOC_RANGE_SIZE];
groupshared AABB NeighboursBoxes[PLOC_RANGE_SIZE];

// To store range from [-8, 8] we need 4 bits.
// 3 bits for the (value - 1). Last bit for the sign.
// Can't store 0 with this layout but for offset purpose it is fine
// decode(ABCD) = (toDecimal(ABC) + 1) * sign(D)
// decode(1110) = (7 + 1) *  1 =  8
// decode(1111) = (7 + 1) * -1 = -8
// decode(0000) = (0 + 1) *  1 =  1
// decode(0001) = (0 + 1) * -1 = -1
uint EncodeOffsetIntoLowerBits(int offset)
{
    uint signLastLowerBit = (offset >> 31) + 1;
    uint valueUpperBits = abs(offset) - 1 << 1;
    uint result = valueUpperBits | signLastLowerBit;
    return result;
}

int DecodeOffsetFromLowerBits(uint encodedValue)
{
    int offset = BitUtils::ExtractBits(encodedValue, 1, RADIUS_SHIFT) + 1;
    uint sign = BitUtils::ExtractLowestBit(encodedValue);

    return sign == 0 ? -offset : offset;
}

void InitializeNeighbours(uint threadId, uint blockOffset)
{
    for (uint rangeId = threadId; rangeId < PLOC_RANGE_SIZE; rangeId += THREADS)
    {
        int globalId = rangeId - PLOC_OFFSET + blockOffset;
        Neighbours[rangeId] = UINT_MAX_VALUE;

        if (IsInBounds(globalId))
        {
            NeighboursBoxes[rangeId] = Nodes[globalId].Box;
        }
        else
        {
            NeighboursBoxes[rangeId] = AABB::CreateMaxBox();
        }
    }

    GroupMemoryBarrierWithGroupSync();
}

uint MergeDistanceWithOffset(uint distanceUpperBits, int offset)
{
    return distanceUpperBits | EncodeOffsetIntoLowerBits(offset);
}

void UpdateNeighbourFromTheLeft(uint distanceUpperBits, int offset, uint selfId)
{
    uint neighbourEncodedDistance = MergeDistanceWithOffset(distanceUpperBits, -offset);
    InterlockedMin(Neighbours[selfId + offset], neighbourEncodedDistance);
}

void UpdateMinDistance(uint distanceUpperBits, uint offset, inout uint minEncodedDistance)
{
    uint selfEncodedDistance = MergeDistanceWithOffset(distanceUpperBits, offset);
    minEncodedDistance = min(minEncodedDistance, selfEncodedDistance);
}

void UpdateSelfBasedOnRightNeighbours(uint id, uint minDistanceIndex)
{
    InterlockedMin(Neighbours[id], minDistanceIndex);
}

uint GetDistanceToNeighbourUpperBits(uint neighbourId, AABB box)
{
    float distance = box.Union(NeighboursBoxes[neighbourId]).ComputeSurfaceArea();
    uint distanceInteger = asuint(distance);
    uint positiveDistanceInteger = distanceInteger << 1;
    return positiveDistanceInteger & ENCODE_MASK;
}

void RunSearch(uint threadId)
{
    for (uint rangeId = threadId; rangeId < THREADS + 3 * RADIUS; rangeId += THREADS)
    {
        uint minEncodedDistance = UINT_MAX_VALUE;
        AABB box = NeighboursBoxes[rangeId];

        [unroll(RADIUS)]
        for (uint offset = 1; offset <= RADIUS; offset++)
        {
            uint distanceUpperBits = GetDistanceToNeighbourUpperBits(rangeId + offset, box);
            UpdateMinDistance(distanceUpperBits, offset, minEncodedDistance);
            UpdateNeighbourFromTheLeft(distanceUpperBits, offset, rangeId);
        }

        UpdateSelfBasedOnRightNeighbours(rangeId, minEncodedDistance);
    }

    GroupMemoryBarrierWithGroupSync();
}

uint GetNeighbour(uint threadId)
{
    return DecodeOffsetFromLowerBits(Neighbours[threadId + PLOC_OFFSET]) + threadId;
}

uint FindNearestNeighbour(uint threadId, uint blockOffset)
{
    InitializeNeighbours(threadId, blockOffset);
    RunSearch(threadId);
    return GetNeighbour(threadId);
}