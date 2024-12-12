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

void Compress(uint isInvalidNode, uint threadId, uint groupId)
{
    uint invalidNodeScan = ComputeInclusiveScan(isInvalidNode, threadId) - isInvalidNode;
    WaitForSumOfMergedNodesInPreviousGroups(threadId, groupId);
    CompressValidNodes(isInvalidNode, threadId, groupId * THREADS, invalidNodeScan);
}