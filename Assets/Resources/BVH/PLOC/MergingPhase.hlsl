RWStructuredBuffer<uint> TreeSize;

PreMergeResult PreComputeMergeResult(uint nearestNeighbour, uint threadId, uint blockOffset)
{
    uint areNodesMutuallyClosest = GetNeighbour(nearestNeighbour) == threadId;
    uint isNodeFromTheLeft = (int)threadId < (int)nearestNeighbour;
    
    PreMergeResult preMergeResult;
    preMergeResult.IsValidNode = (areNodesMutuallyClosest == 0 || isNodeFromTheLeft) && IsInBounds(threadId + blockOffset);
    preMergeResult.CanMerge = areNodesMutuallyClosest && isNodeFromTheLeft;
    return preMergeResult;
}

BVHNode TryMerge(uint nearestNeighbour, uint threadId, uint blockOffset, uint isMergeConditionMet, uint mergedNodesScan)
{
    BVHNode rightNode = Nodes[nearestNeighbour + blockOffset];
    BVHNode leftNode = Nodes[threadId + blockOffset];
    BVHNode resultNode = leftNode;

    if (isMergeConditionMet)
    {
        uint groupCompressIndex = mergedNodesScan * 2;
        uint globalOffset = SumOfMergedNodesInPreviousGroups * 2;
        uint leftChildIndex = groupCompressIndex + globalOffset + TreeSize[0];
        uint rightChildIndex = leftChildIndex + 1;
        AABB mergedBox = NeighboursBoxes[threadId + PLOC_OFFSET].Union(NeighboursBoxes[nearestNeighbour + PLOC_OFFSET]);
        
        resultNode = BVHNode::Create(leftChildIndex, rightChildIndex, mergedBox);
        Tree[leftChildIndex] = leftNode;
        Tree[rightChildIndex] = rightNode;
    }

    return resultNode;
}

void CompressValidNodes(uint isValidNode, uint validNodesLocalScan, BVHNode resultNode)
{
    if (isValidNode)
    {
        uint globalCompressIndex = validNodesLocalScan + SumOfValidNodesInPreviousGroups;
        Nodes[globalCompressIndex] = resultNode;
    }
}