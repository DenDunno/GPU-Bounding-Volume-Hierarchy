#include "NearestNeighbour.hlsl"

void MergeInternal(uint nearestNeighbourRangeId, uint threadId, uint blockOffset)
{
    BVHNode node;
    node.Box = NeighboursBoxes[nearestNeighbourRangeId].Union(NeighboursBoxes[threadId]);
    node.SetLeftChild(threadId + blockOffset);
    node.SetRightChild(nearestNeighbourRangeId + blockOffset);
}

uint Merge(uint nearestNeighbourRangeId, uint threadId, uint blockOffset)
{
    bool areNodesMutuallyClosest = GetNeighbour(nearestNeighbourRangeId) == threadId;
    bool isNodeFromTheLeft = threadId < nearestNeighbourRangeId;
    uint mergeConditionIsMet = areNodesMutuallyClosest && isNodeFromTheLeft;
    
    if (mergeConditionIsMet)
    {
        MergeInternal(nearestNeighbourRangeId, threadId, blockOffset);
    }

    return areNodesMutuallyClosest && isNodeFromTheLeft == false;
}