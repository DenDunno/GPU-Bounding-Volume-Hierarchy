groupshared uint SumOfMergedNodesInPreviousGroups;
RWStructuredBuffer<uint> BlockOffset;

void CompressValidNodes(uint isInvalidNode, uint threadId, uint blockOffset, uint invalidNodeScan)
{
    bool isValidNode = isInvalidNode == 0u;
    
    if (isValidNode)
    {
        uint groupCompressIndex = threadId - invalidNodeScan;
        uint globalCompressIndex = SumOfMergedNodesInPreviousGroups + groupCompressIndex;
        Nodes[globalCompressIndex] = Nodes[blockOffset + threadId];
    }
}