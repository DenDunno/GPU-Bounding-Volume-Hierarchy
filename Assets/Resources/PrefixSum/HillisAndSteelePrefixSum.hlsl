#include "PrefixSumBase.hlsl"

PREFIX_SUM_TYPE ComputeInclusiveScan(const PREFIX_SUM_TYPE inputValue, int threadId)
{
    MoveDataToSharedMemory(threadId, inputValue);

    [unroll]
    for (int offset = 1; offset < THREADS; offset *= 2)
    {
        bool inBounds = threadId >= offset;
        PREFIX_SUM_TYPE childLeftLeafSum = inBounds ? GetPrefixSum(threadId - offset) : 0;
        PREFIX_SUM_TYPE childRightLeafSum = GetPrefixSum(threadId);
        
        GroupMemoryBarrierWithGroupSync();
        Scan[threadId] = childLeftLeafSum + childRightLeafSum;
        GroupMemoryBarrierWithGroupSync();
    }

    return GetPrefixSum(threadId);
}