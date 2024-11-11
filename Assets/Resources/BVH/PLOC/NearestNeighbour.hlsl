#ifndef THREADS
#define THREADS 256
#endif

#include "..//..//Utilities/BitManipulation.hlsl"
#include "..//NodesInput.hlsl"
#define UINT_MAX_VALUE -1
#define SEARCH_RADIUS_SHIFT 3
#define SEARCH_RADIUS 1 << SEARCH_RADIUS_SHIFT
#define ENCODE_MASK ~(1 << (SEARCH_RADIUS_SHIFT + 1) - 1)
groupshared uint Neighbours[THREADS];

// To store range from [-8, 8] we need 4 bits.
// 3 bits for the (value - 1). Last bit for the sign.
// Can't store 0 with this layout but for offset purpose it is fine
// decode(ABCD) = (toDecimal(ABC) + 1) * sign(D)
// decode(1110) = (7 + 1) *  1 =  8
// decode(1111) = (7 + 1) * -1 = -8
// decode(0000) = (0 + 1) *  1 =  1
// decode(0001) = (0 + 1) * -1 = -1
uint EncodeOffsetIntoLowerBits(const uint id, const uint neighbor)
{
    const int signedOffset = neighbor - id;
    const uint signLastLowerBit = signedOffset >> 31;
    const uint valueUpperBits = (abs(signedOffset) - 1) << 1;
    return valueUpperBits | signLastLowerBit;
}

int DecodeOffsetFromLowerBits(const uint encodedValue)
{
    uint offset = ExtractBits(encodedValue, 1, SEARCH_RADIUS_SHIFT) + 1;
    uint sign = ExtractLowestBit(encodedValue);
    
    return sign ? -offset : offset;
}

void InitializeNeighbours(uint threadId)
{
    Neighbours[threadId] = UINT_MAX_VALUE;
    GroupMemoryBarrierWithGroupSync();
}

float ComputeDistanceBetweenNodes(uint first, uint second)
{
    return Nodes[first].Box.Union(Nodes[second].Box).ComputeSurfaceArea();
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

uint GetDistanceToNeighbourUpperBits(uint id, uint encodeMask)
{
    float distance = ComputeDistanceBetweenNodes(id, id + 1);
    uint positiveDistanceInteger = asuint(distance) << 1;
    return positiveDistanceInteger & encodeMask;
}

void UpdateMinDistanceIndex(uint id, uint i, uint distanceUpperBits, inout uint minDistanceIndex)
{
    uint selfEncodedDistance = distanceUpperBits | EncodeOffsetIntoLowerBits(id, id + i);
    minDistanceIndex = min(minDistanceIndex, selfEncodedDistance);
}

uint UpdateRightNeighboursBasedOnSelf(uint id)
{
    uint minDistanceIndex = UINT_MAX_VALUE;
    uint encodeMask = ENCODE_MASK;
    
    [unroll(SEARCH_RADIUS)]
    for (uint i = 1; i <= SEARCH_RADIUS; i++)
    {
        uint distanceUpperBits = GetDistanceToNeighbourUpperBits(id, encodeMask); 
        UpdateMinDistanceIndex(distanceUpperBits, id, i, minDistanceIndex);
        UpdateNeighbourFromTheLeft(id, i, distanceUpperBits);
    }

    return minDistanceIndex;
}

uint FindNearestNeighbour(uint id)
{
    InitializeNeighbours(id);
    uint minDistanceIndex = UpdateRightNeighboursBasedOnSelf(id);
    UpdateSelfBasedOnRightNeighbours(id, minDistanceIndex);
    return Neighbours[id];
}