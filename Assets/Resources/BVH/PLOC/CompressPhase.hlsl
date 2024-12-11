#define PREFIX_SUM_TYPE uint
#include "..//..//Utilities/ThreadsUtilities.hlsl"
#include "..//..//PrefixSum/HillisAndSteelePrefixSum.hlsl"
groupshared uint SumOfMergedNodesInPreviousGroups;
RWStructuredBuffer<uint> BlockOffset;

void WaitForSumOfMergedNodesInPreviousGroups(uint threadId, uint groupId)
{
    uint totalNodesToMerge = GetTotalSum();
    
    WAIT_FOR_PREVIOUS_GROUPS_SINGLE(threadId, THREAD_LAST_INDEX, groupId,
        InterlockedAdd(BlockOffset[0], totalNodesToMerge, SumOfMergedNodesInPreviousGroups);
    )
}

void CompressValidNodes(uint isInvalidatedNode, uint threadId, uint blockOffset, uint isInvalidatedNodeScan)
{
    bool isValidNode = isInvalidatedNode == 0u;
    
    if (isValidNode)
    {
        uint groupCompressIndex = threadId - isInvalidatedNodeScan;
        uint globalCompressIndex = SumOfMergedNodesInPreviousGroups + groupCompressIndex;
        Nodes[globalCompressIndex] = Nodes[blockOffset + threadId];
    }
}

void Compress(uint isInvalidNode, uint threadId, uint groupId)
{
    uint invalidNodeScan = ComputeInclusiveScan(isInvalidNode, threadId) - isInvalidNode;
    WaitForSumOfMergedNodesInPreviousGroups(threadId, groupId);
    CompressValidNodes(isInvalidNode, threadId, groupId * THREADS, invalidNodeScan);
}