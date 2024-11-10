#ifndef THREADS
#define THREADS 256
#endif

#include "..//NodesInput.hlsl"
#define UINT_MAX_VALUE -1
#define SEARCH_RADIUS_SHIFT 4
#define SEARCH_RADIUS 1 << SEARCH_RADIUS_SHIFT
#define ENCODE_MASK ~(1 << (SEARCH_RADIUS_SHIFT + 1) - 1)
groupshared uint Neighbours[THREADS];

uint EncodeOffsetIntoLowerBits(const uint id, const uint neighbor)
{
    const int signedOffset = neighbor - id;
    const uint signLastLowerBit = signedOffset >> 31;
    const uint valueUpperBits = (abs(signedOffset) - 1) << 1;
    return valueUpperBits | signLastLowerBit;
}

void DecodeOffset()
{
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

uint FindNearestNeighbour(uint id)
{
    InitializeNeighbours(id);
    uint minAreaIndex = UINT_MAX_VALUE;

    [unroll(SEARCH_RADIUS)]
    for (int i = 1; i <= SEARCH_RADIUS; i++)
    {
        float area = ComputeDistanceBetweenNodes(id, id + 1);
        uint positiveAreaInteger = asuint(area) << 1;
        uint areaUpperBits = positiveAreaInteger & ENCODE_MASK;
        uint encode0 = areaUpperBits | EncodeOffsetIntoLowerBits(id, id + i);
        uint encode1 = areaUpperBits | EncodeOffsetIntoLowerBits(id + i, id);
    }

    return Neighbours[id];
}