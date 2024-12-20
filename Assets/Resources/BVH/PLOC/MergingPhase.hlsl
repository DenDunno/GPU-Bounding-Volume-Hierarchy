#include "NearestNeighbour.hlsl"
#define PREFIX_SUM_TYPE uint2
#include "..//..//PrefixSum/HillisAndSteelePrefixSum.hlsl"
#include "..//..//Utilities/ThreadsUtilities.hlsl"
#include "CompressPhase.hlsl"
RWStructuredBuffer<uint> TreeSize;
RWStructuredBuffer<uint2> Test;

void MergeInternal(uint nearestNeighbour, uint threadId, uint blockOffset)
{
    Nodes[threadId + blockOffset] = BVHNode::Create(
        threadId + blockOffset,
        nearestNeighbour + blockOffset,
        NeighboursBoxes[threadId + PLOC_OFFSET].Union(NeighboursBoxes[nearestNeighbour + PLOC_OFFSET]));
}

void Merge(uint nearestNeighbour, uint threadId, uint groupId, uint blockOffset)
{
    uint areNodesMutuallyClosest = GetNeighbour(nearestNeighbour) == threadId;
    uint isNodeFromTheLeft = (int)threadId < (int)nearestNeighbour;
    uint mergeConditionIsMet = areNodesMutuallyClosest && isNodeFromTheLeft;
    uint isValidNode = (areNodesMutuallyClosest == 0 || isNodeFromTheLeft) && IsInBounds(threadId + blockOffset);
    
    BVHNode leftNode = Nodes[threadId + blockOffset];
    BVHNode rightNode = Nodes[nearestNeighbour + blockOffset];
    BVHNode result = leftNode;
    
    uint2 scan = ComputeInclusiveScan(uint2(isValidNode, mergeConditionIsMet), threadId);
    uint validNodesInclusiveScan = scan.x;
    uint mergedNodesInclusiveScan = scan.y;
    
    WAIT_FOR_PREVIOUS_GROUPS_SINGLE(threadId, THREAD_LAST_INDEX, groupId,
        InterlockedAdd(BlockOffset[0], mergedNodesInclusiveScan, SumOfMergedNodesInPreviousGroups);
        InterlockedAdd(ValidNodesCount[0], validNodesInclusiveScan, SumOfValidNodesInPreviousGroups);
    )
    
    if (mergeConditionIsMet)
    {
        uint groupCompressIndex = (mergedNodesInclusiveScan - 1) * 2;
        uint globalOffset = SumOfMergedNodesInPreviousGroups * 2;
        uint leftChildIndex = groupCompressIndex + globalOffset + TreeSize[0];
        uint rightChildIndex = leftChildIndex + 1;
        AABB mergedBox = NeighboursBoxes[threadId + PLOC_OFFSET].Union(NeighboursBoxes[nearestNeighbour + PLOC_OFFSET]);
        
        result = BVHNode::Create(leftChildIndex, rightChildIndex, mergedBox);
        Tree[leftChildIndex] = leftNode;
        Tree[rightChildIndex] = rightNode;
    }
    
    if (isValidNode)
    {
        uint groupCompressIndex = validNodesInclusiveScan - isValidNode;
        uint globalCompressIndex = SumOfValidNodesInPreviousGroups + groupCompressIndex;
        Nodes[globalCompressIndex] = result;
    }
}