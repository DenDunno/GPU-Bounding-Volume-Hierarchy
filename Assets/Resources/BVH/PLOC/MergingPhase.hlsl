#include "NearestNeighbour.hlsl"
#define PREFIX_SUM_TYPE uint2
#include "..//..//PrefixSum/HillisAndSteelePrefixSum.hlsl"
#include "..//..//Utilities/ThreadsUtilities.hlsl"
#include "CompressPhase.hlsl"
RWStructuredBuffer<uint> TreeSize;

void MergeInternal(uint nearestNeighbour, uint threadId, uint blockOffset)
{
    Nodes[threadId + blockOffset] = BVHNode::Create(
        threadId + blockOffset,
        nearestNeighbour + blockOffset,
        NeighboursBoxes[threadId + PLOC_OFFSET].Union(NeighboursBoxes[nearestNeighbour + PLOC_OFFSET]));
}

groupshared uint2 scan[THREADS];

void ScanRadius(uint threadId, uint nearestNeighbour)
{
    uint2 areNodesMutuallyClosest;
    areNodesMutuallyClosest.x = GetNeighbour(nearestNeighbour) == threadId;
    areNodesMutuallyClosest.y = 0;

    if (threadId < RADIUS)
    {
        areNodesMutuallyClosest.y = GetNeighbour(GetNeighbour(threadId + THREADS)) == threadId + THREADS;
    }

    ComputeInclusiveScan(areNodesMutuallyClosest, threadId);
    Scan[threadId].y += GetTotalSum().x;
    GroupMemoryBarrierWithGroupSync();
}

uint GetPrefixSumFromScan(uint neighbourId)
{
    return neighbourId < THREADS ? Scan[neighbourId].x : Scan[neighbourId % THREADS].y;
}

void Merge(uint nearestNeighbour, uint threadId, uint groupId, uint blockOffset)
{
    uint areNodesMutuallyClosest = GetNeighbour(nearestNeighbour) == threadId;
    uint isNodeFromTheLeft = threadId < nearestNeighbour;
    uint mergeConditionIsMet = areNodesMutuallyClosest * isNodeFromTheLeft;
    uint isInvalidNode = areNodesMutuallyClosest * (1 - isNodeFromTheLeft);
    
    BVHNode leftNode = Nodes[threadId + blockOffset];
    BVHNode rightNode = Nodes[nearestNeighbour + blockOffset];

    ScanRadius(threadId, nearestNeighbour);
    
    WAIT_FOR_PREVIOUS_GROUPS_SINGLE(threadId, THREAD_LAST_INDEX, groupId,
        InterlockedAdd(BlockOffset[0], GetTotalSum().x, SumOfMergedNodesInPreviousGroups);
    )
    //
    // uint childrenScan = ComputeExclusiveScan(areNodesMutuallyClosest, threadId);
    // if (mergeConditionIsMet)
    // {
    //     uint leftChildIndex = childrenScan + SumOfMergedNodesInPreviousGroups * 2 + TreeSize[0];
    //     uint rightChildIndex = Scan[nearestNeighbour] + SumOfMergedNodesInPreviousGroups * 2 + TreeSize[0];;
    //     AABB mergedBox = NeighboursBoxes[threadId + PLOC_OFFSET].Union(NeighboursBoxes[nearestNeighbour + PLOC_OFFSET]);
    //     
    //     Nodes[threadId + blockOffset] = BVHNode::Create(leftChildIndex, rightChildIndex, mergedBox);
    //     Tree[leftChildIndex] = leftNode;
    //     Tree[rightChildIndex] = rightNode;
    // }
    //
    // CompressValidNodes(isInvalidNode, threadId, blockOffset, invalidNodeScan);
}