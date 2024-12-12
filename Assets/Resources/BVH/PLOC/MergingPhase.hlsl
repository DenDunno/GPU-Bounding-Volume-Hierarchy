#include "NearestNeighbour.hlsl"
#include "CompressPhase.hlsl"

RWStructuredBuffer<uint> TreeSize;

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
    uint isNodeFromTheLeft = threadId < nearestNeighbour;
    uint mergeConditionIsMet = areNodesMutuallyClosest * isNodeFromTheLeft;
    uint isInvalidNode = areNodesMutuallyClosest * (1 - isNodeFromTheLeft);

    uint invalidNodeScan = ComputeInclusiveScan(isInvalidNode, threadId) - isInvalidNode;
    uint mergedNodesInGroup = GetTotalSum();

    BVHNode leftNode = Nodes[threadId + blockOffset];
    BVHNode rightNode = Nodes[nearestNeighbour + blockOffset];
    
    WAIT_FOR_PREVIOUS_GROUPS_SINGLE(threadId, THREAD_LAST_INDEX, groupId,
        InterlockedAdd(BlockOffset[0], mergedNodesInGroup, SumOfMergedNodesInPreviousGroups);
    )
    
    uint childrenScan = ComputeExclusiveScan(areNodesMutuallyClosest, threadId);
    if (mergeConditionIsMet)
    {
        uint leftChildIndex = childrenScan + SumOfMergedNodesInPreviousGroups * 2 + TreeSize[0];
        uint rightChildIndex = Scan[nearestNeighbour] + SumOfMergedNodesInPreviousGroups * 2 + TreeSize[0];;
        AABB mergedBox = NeighboursBoxes[threadId + PLOC_OFFSET].Union(NeighboursBoxes[nearestNeighbour + PLOC_OFFSET]);
        
        Nodes[threadId + blockOffset] = BVHNode::Create(leftChildIndex, rightChildIndex, mergedBox);
        Tree[leftChildIndex] = leftNode;
        Tree[rightChildIndex] = rightNode;
    }

    CompressValidNodes(isInvalidNode, threadId, blockOffset, invalidNodeScan);
}