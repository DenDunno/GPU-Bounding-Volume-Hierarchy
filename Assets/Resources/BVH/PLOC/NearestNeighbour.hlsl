#ifndef THREADS
#define THREADS 256
#endif

#include "..//NodesInput.hlsl"
#define UINT_MAX_VALUE -1
#define SEARCH_RADIUS_SHIFT 4
#define SEARCH_RADIUS 1 << SEARCH_RADIUS_SHIFT
#define ENCODE_MASK (1 << (SEARCH_RADIUS_SHIFT + 1) - 1)
groupshared uint Neighbours[THREADS];


uint EncodeRelativeOffset(const uint id, const uint neighbor)
{
    const uint uOffset = neighbor - id - 1;
    return uOffset << 1;
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

void FindNearestNeighbour(uint threadId)
{
    InitializeNeighbours(threadId);
    uint minAreaIndex = UINT_MAX_VALUE;

    [unroll(SEARCH_RADIUS)]
    for (int i = 1; i <= SEARCH_RADIUS; i++)
    {
        float area = ComputeDistanceBetweenNodes(threadId, threadId + 1);
        uint areaInteger = asuint(area);
    }
}