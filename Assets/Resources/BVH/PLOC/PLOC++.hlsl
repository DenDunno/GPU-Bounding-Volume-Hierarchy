#include "NearestNeighbour.hlsl"
#include "..//Range.hlsl"

void Merge()
{
}

void Compress()
{
}

void PLOCMerge(Range range, uint globalId, uint threadId, uint parentId)
{
    uint nearestNeighbour = FindNearestNeighbour(threadId);
}