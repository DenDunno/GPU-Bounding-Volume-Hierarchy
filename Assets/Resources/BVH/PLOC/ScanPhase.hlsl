#include "..//..//Utilities/ThreadsUtilities.hlsl"
#define PREFIX_SUM_TYPE uint2
#include "..//..//PrefixSum/HillisAndSteelePrefixSum.hlsl"
groupshared uint SumOfValidNodesInPreviousGroups;
groupshared uint SumOfMergedNodesInPreviousGroups;
RWStructuredBuffer<uint> MergedNodesCount;
RWStructuredBuffer<uint> ValidNodesCount;

struct PreMergeResult
{
    uint CanMerge;
    uint IsValidNode;
};

struct PreMergeScanResult
{
    uint MergedNodes;
    uint ValidNodes;
};

PreMergeScanResult PerformInclusiveGlobalScan(uint threadId, uint groupId, PreMergeResult preMergeResult)
{
    uint2 scanInput = uint2(preMergeResult.IsValidNode, preMergeResult.CanMerge);
    uint2 scan = ComputeInclusiveScan(scanInput, threadId);
    uint validNodesInclusiveScan = scan.x;
    uint mergedNodesInclusiveScan = scan.y;
    
    SYNCHRONIZE(threadId, THREAD_LAST_INDEX, groupId,
        InterlockedAdd(MergedNodesCount[0], mergedNodesInclusiveScan, SumOfMergedNodesInPreviousGroups);
        InterlockedAdd(ValidNodesCount[0], validNodesInclusiveScan, SumOfValidNodesInPreviousGroups);
    )

    PreMergeScanResult result;
    result.ValidNodes = validNodesInclusiveScan - preMergeResult.IsValidNode;
    result.MergedNodes = mergedNodesInclusiveScan - preMergeResult.CanMerge;
    return result;
}