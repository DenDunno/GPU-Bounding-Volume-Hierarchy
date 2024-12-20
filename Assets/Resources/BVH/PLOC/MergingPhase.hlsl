#include "NearestNeighbour.hlsl"
#include "ScanPhase.hlsl"
RWStructuredBuffer<uint> TreeSize;

void Merge(uint nearestNeighbour, uint threadId, uint groupId, uint blockOffset)
{
    uint areNodesMutuallyClosest = GetNeighbour(nearestNeighbour) == threadId;
    uint isNodeFromTheLeft = (int)threadId < (int)nearestNeighbour;
    uint mergeConditionIsMet = areNodesMutuallyClosest && isNodeFromTheLeft;
    uint isValidNode = (areNodesMutuallyClosest == 0 || isNodeFromTheLeft) && IsInBounds(threadId + blockOffset);
    BVHNode leftNode = Nodes[threadId + blockOffset];
    BVHNode rightNode = Nodes[nearestNeighbour + blockOffset];
    BVHNode resultNode = leftNode;
    
    ScanResult scan = PerformInclusiveGlobalScan(threadId, groupId, isValidNode, mergeConditionIsMet);
    
    if (mergeConditionIsMet)
    {
        uint groupCompressIndex = scan.MergedNodes * 2;
        uint globalOffset = SumOfMergedNodesInPreviousGroups * 2;
        uint leftChildIndex = groupCompressIndex + globalOffset + TreeSize[0];
        uint rightChildIndex = leftChildIndex + 1;
        AABB mergedBox = NeighboursBoxes[threadId + PLOC_OFFSET].Union(NeighboursBoxes[nearestNeighbour + PLOC_OFFSET]);
        
        resultNode = BVHNode::Create(leftChildIndex, rightChildIndex, mergedBox);
        Tree[leftChildIndex] = leftNode;
        Tree[rightChildIndex] = rightNode;
    }
    
    if (isValidNode)
    {
        uint globalCompressIndex = scan.ValidNodes + SumOfValidNodesInPreviousGroups;
        Nodes[globalCompressIndex] = resultNode;
    }
}