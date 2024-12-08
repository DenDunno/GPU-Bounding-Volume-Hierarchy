#include "NearestNeighbour.hlsl"

bool IsValidToMerge(uint nearestNeighbourRangeId, uint threadId)
{
    bool areNodesMutuallyClosest = Neighbours[nearestNeighbourRangeId] == threadId;
    bool isNodeFromTheLeft = threadId < nearestNeighbourRangeId;

    return areNodesMutuallyClosest && isNodeFromTheLeft;
}

void InvalidateRightNode(uint nodeGlobalIndex)
{
}

void Merge(uint nearestNeighbourRangeId, uint threadId, uint blockOffset)
{
    BVHNode node;
    node.Box = NeighboursBoxes[nearestNeighbourRangeId].Union(NeighboursBoxes[threadId]);
    node.SetLeftChild(threadId + blockOffset);
    node.SetRightChild(nearestNeighbourRangeId + blockOffset);
}

void TryMerge(uint nearestNeighbourRangeId, uint threadId, uint blockOffset)
{
    if (IsValidToMerge(nearestNeighbourRangeId, threadId))
    {
        Merge(nearestNeighbourRangeId, threadId, blockOffset);
        InvalidateRightNode(nearestNeighbourRangeId + blockOffset);
    }
}
