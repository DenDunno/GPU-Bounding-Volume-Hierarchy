#include "..//..//Utilities/ThreadsUtilities.hlsl"
#define PREFIX_SUM_TYPE uint2
#include "..//..//PrefixSum/HillisAndSteelePrefixSum.hlsl"
groupshared uint SumOfValidNodesInPreviousGroups;
groupshared uint SumOfMergedNodesInPreviousGroups;
RWStructuredBuffer<uint> MergedNodesCount;
RWStructuredBuffer<uint> ValidNodesCount;

struct ScanResult
{
    uint ValidNodes;
    uint MergedNodes;
};

ScanResult PerformInclusiveGlobalScan(uint threadId, uint groupId, uint isValidNode, uint mergeConditionIsMet)
{
    uint2 scan = ComputeInclusiveScan(uint2(isValidNode, mergeConditionIsMet), threadId);
    uint validNodesInclusiveScan = scan.x;
    uint mergedNodesInclusiveScan = scan.y;
    
    SYNCHRONIZE(threadId, THREAD_LAST_INDEX, groupId,
        InterlockedAdd(MergedNodesCount[0], mergedNodesInclusiveScan, SumOfMergedNodesInPreviousGroups);
        InterlockedAdd(ValidNodesCount[0], validNodesInclusiveScan, SumOfValidNodesInPreviousGroups);
    )

    ScanResult result;
    result.ValidNodes = validNodesInclusiveScan - isValidNode;
    result.MergedNodes = mergedNodesInclusiveScan - mergeConditionIsMet;

    return result;
}