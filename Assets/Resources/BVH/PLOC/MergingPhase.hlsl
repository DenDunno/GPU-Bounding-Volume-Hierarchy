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

groupshared uint2 Output[THREADS];
void ScanRadius(uint threadId, uint nearestNeighbour)
{
    uint2 areNodesMutuallyClosest;
    areNodesMutuallyClosest.x = GetNeighbour(nearestNeighbour) == threadId;
    areNodesMutuallyClosest.y = 0;

    if (threadId < RADIUS)
    {
        areNodesMutuallyClosest.y = GetNeighbour(GetNeighbour(threadId + THREADS)) == threadId + THREADS;
    }

    uint2 scan = ComputeInclusiveScan(areNodesMutuallyClosest, threadId) - areNodesMutuallyClosest;
    Output[threadId] = scan + uint2(0, GetTotalSum().x);
    GroupMemoryBarrierWithGroupSync();
}

uint GetPrefixSumFromScan(uint neighbourId)
{
    return neighbourId < THREADS ? Output[neighbourId].x : Output[neighbourId % THREADS].y;
}

void Merge(uint nearestNeighbour, uint threadId, uint groupId, uint blockOffset)
{
    uint areNodesMutuallyClosest = GetNeighbour(nearestNeighbour) == threadId;
    uint isNodeFromTheLeft = (int)threadId < (int)nearestNeighbour;
    uint mergeConditionIsMet = areNodesMutuallyClosest * isNodeFromTheLeft;
    uint isValidNode = (areNodesMutuallyClosest == 0 || isNodeFromTheLeft) && threadId + blockOffset < LeavesCount;
    
    BVHNode leftNode = Nodes[threadId + blockOffset];
    BVHNode rightNode = Nodes[nearestNeighbour + blockOffset];

    uint validNodesInclusiveScan = ComputeInclusiveScan(isValidNode, threadId).x;
    ScanRadius(threadId, nearestNeighbour);
    
    WAIT_FOR_PREVIOUS_GROUPS_SINGLE(threadId, THREAD_LAST_INDEX, groupId,
        InterlockedAdd(BlockOffset[0], GetTotalSum().x, TreeSizeShared);
        InterlockedAdd(ValidNodesCount[0], validNodesInclusiveScan, SumOfValidNodesInPreviousGroups);
    )
    
    if (mergeConditionIsMet)
    {
        uint leftChildIndex = GetPrefixSumFromScan(threadId) + TreeSizeShared + TreeSize[0];
        uint rightChildIndex = GetPrefixSumFromScan(nearestNeighbour) + TreeSizeShared + TreeSize[0];;
        AABB mergedBox = NeighboursBoxes[threadId + PLOC_OFFSET].Union(NeighboursBoxes[nearestNeighbour + PLOC_OFFSET]);
        
        Nodes[threadId + blockOffset] = BVHNode::Create(leftChildIndex, rightChildIndex, mergedBox);
        Tree[leftChildIndex] = leftNode;
        Tree[rightChildIndex] = rightNode;
    }
    
    if (isValidNode)
    {
        uint groupCompressIndex = validNodesInclusiveScan - isValidNode;
        uint globalCompressIndex = SumOfValidNodesInPreviousGroups + groupCompressIndex;
        Nodes[globalCompressIndex] = Nodes[blockOffset + threadId];
    }
}