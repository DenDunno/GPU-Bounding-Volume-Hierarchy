#include "NearestNeighbour.hlsl"
#define PREFIX_SUM_TYPE uint
#include "..//..//PrefixSum/HillisAndSteelePrefixSum.hlsl"
#include "..//..//Utilities/ThreadsUtilities.hlsl"
RWStructuredBuffer<uint> BlockOffset;
groupshared uint SumOfMergedNodesInPreviousGroups;

uint IsValidToMerge(uint nearestNeighbourRangeId, uint threadId)
{
    bool areNodesMutuallyClosest = GetNeighbour(nearestNeighbourRangeId) == threadId;
    bool isNodeFromTheLeft = threadId < nearestNeighbourRangeId;

    return areNodesMutuallyClosest && isNodeFromTheLeft;
}

void Merge(uint nearestNeighbourRangeId, uint threadId, uint blockOffset)
{
    BVHNode node;
    node.Box = NeighboursBoxes[nearestNeighbourRangeId].Union(NeighboursBoxes[threadId]);
    node.SetLeftChild(threadId + blockOffset);
    node.SetRightChild(nearestNeighbourRangeId + blockOffset);
}

uint GetLastValidIndexInGroup(uint totalElements, uint groupId)
{
    return min(THREADS, totalElements - groupId * THREADS);
}

uint TryMerge(uint nearestNeighbourRangeId, uint threadId, uint groupId, uint blockOffset)
{
    bool areNodesMutuallyClosest = GetNeighbour(nearestNeighbourRangeId) == threadId;
    bool isNodeFromTheLeft = threadId < nearestNeighbourRangeId;
    uint isValidToMerge = areNodesMutuallyClosest && isNodeFromTheLeft;
    uint isNodeFromTheRight = isNodeFromTheLeft == false;
    uint isInvalidatedNode = areNodesMutuallyClosest && isNodeFromTheRight;
    uint isInvalidatedNodeScan = ComputeInclusiveScan(isInvalidatedNode, threadId) - isInvalidatedNode;
    uint totalNodesToMerge = GetTotalSum();
    uint groupCompressIndex = isInvalidatedNode
                            ? GetLastValidIndexInGroup(LeavesCount, groupId) - totalNodesToMerge + isInvalidatedNodeScan
                            : threadId - isInvalidatedNodeScan;
    
    WAIT_FOR_PREVIOUS_GROUPS_SINGLE(threadId, THREAD_LAST_INDEX, groupId,
        InterlockedAdd(BlockOffset[0], totalNodesToMerge, SumOfMergedNodesInPreviousGroups);
    )
    
    uint globalCompressIndex = SumOfMergedNodesInPreviousGroups + groupCompressIndex;
    
    if (isValidToMerge)
    {
        Merge(nearestNeighbourRangeId, threadId, blockOffset);
    }

    return globalCompressIndex;
}