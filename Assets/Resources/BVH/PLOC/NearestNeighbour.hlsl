#include "..//NodesInput.hlsl"

#ifndef THREADS
#define THREADS 256
#endif
#define UINT_MAX_VALUE -1
#define RADIUS_SHIFT 4
static const uint RADIUS = 1 << RADIUS_SHIFT;
static const uint PLOC_RANGE_SIZE = THREADS + 4 * RADIUS;
static const uint ENCODE_MASK = ~((1u << (RADIUS_SHIFT + 1)) - 1);
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
uint EncodeOffsetIntoLowerBits(uint id, uint neighbor)
{
    int signedOffset = neighbor - id;
    uint signLastLowerBit = (signedOffset >> 31) + 1;
    uint valueUpperBits = abs(signedOffset) - 1 << 1;
    uint result = valueUpperBits | signLastLowerBit;
    return result;
}

int DecodeOffsetFromLowerBits(uint encodedValue)
{
    uint offset = ExtractBits(encodedValue, 1, RADIUS_SHIFT) + 1;
    uint sign = ExtractLowestBit(encodedValue);

    return sign == 0 ? -offset : offset;
}

void InitializeNeighbours(int threadId, int blockOffset)
{
    for (int rangeId = threadId; rangeId < PLOC_RANGE_SIZE; rangeId += THREADS)
    {
        int globalId = rangeId - 2 * RADIUS + blockOffset;
        Neighbours[rangeId] = UINT_MAX_VALUE;

        if (IsInBounds(globalId))
        {
            NeighboursBoxes[rangeId] = Nodes[ComputeLeafIndex(globalId)].Box;
        }
        else
        {
            NeighboursBoxes[rangeId] = AABB::CreateMaxBox();
        }
    }

    GroupMemoryBarrierWithGroupSync();
}

void UpdateNeighbourFromTheLeft(uint selfId, uint i, uint distanceUpperBits)
{
    uint neighbourEncodedDistance = distanceUpperBits | EncodeOffsetIntoLowerBits(selfId + i, selfId);
    InterlockedMin(Neighbours[selfId + i], neighbourEncodedDistance);
}

void UpdateSelfBasedOnRightNeighbours(uint id, uint minDistanceIndex)
{
    InterlockedMin(Neighbours[id], minDistanceIndex);
}

uint GetDistanceToNeighbourUpperBits(int neighbourId, AABB box)
{
    float distance = box.Union(NeighboursBoxes[neighbourId]).ComputeSurfaceArea();
    uint castedValue = asuint(distance);
    uint positiveDistanceInteger = castedValue << 1;
    return positiveDistanceInteger & ENCODE_MASK;
}

void UpdateMinDistance(uint id, uint i, uint distanceUpperBits, inout uint minDistanceIndex)
{
    uint selfEncodedDistance = distanceUpperBits | EncodeOffsetIntoLowerBits(id, id + i);
    minDistanceIndex = min(minDistanceIndex, selfEncodedDistance);
}

void RunSearch(int threadId)
{
    uint minDistanceIndex = UINT_MAX_VALUE;

    for (int rangeId = threadId; rangeId < THREADS + 3 * RADIUS; rangeId += THREADS)
    {
        AABB box = NeighboursBoxes[rangeId];
                
        for (int i = 1; i <= RADIUS; i++)
        {
            uint distanceUpperBits = GetDistanceToNeighbourUpperBits(rangeId + i, box);
            UpdateMinDistance(rangeId, i, distanceUpperBits, minDistanceIndex);
            UpdateNeighbourFromTheLeft(rangeId, i, distanceUpperBits);
        }

        UpdateSelfBasedOnRightNeighbours(rangeId, minDistanceIndex);
    }

    GroupMemoryBarrierWithGroupSync();
}

uint FindNearestNeighbour(int threadId, int blockOffset)
{
    InitializeNeighbours(threadId, blockOffset);
    RunSearch(threadId);
    return DecodeOffsetFromLowerBits(Neighbours[threadId + 2 * RADIUS]) + threadId + blockOffset;
}