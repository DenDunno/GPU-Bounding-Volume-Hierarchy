groupshared uint SumOfValidNodesInPreviousGroups;
groupshared uint TreeSizeShared;
RWStructuredBuffer<uint> BlockOffset;
RWStructuredBuffer<uint> ValidNodesCount;

void CompressValidNodes(uint isValidNode, uint threadId, uint blockOffset, uint groupCompressIndex)
{
    if (isValidNode)
    {
        uint globalCompressIndex = SumOfValidNodesInPreviousGroups + groupCompressIndex;
        Nodes[globalCompressIndex] = Nodes[blockOffset + threadId];
    }
}