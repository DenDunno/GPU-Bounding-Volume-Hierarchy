#include "PrefixSumBase.hlsl"

TYPE ComputeInclusiveScan(TYPE inputValue, int threadId)
{
    MoveDataToSharedMemory(threadId, inputValue);

    [unroll]
    for (int offset = 1; offset < THREADS; offset *= 2)
    {
        bool inBounds = threadId >= offset;
        TYPE childLeftLeafSum = inBounds ? GetPrefixSum(threadId - offset) : 0;
        TYPE childRightLeafSum = GetPrefixSum(threadId);
        
        GroupMemoryBarrierWithGroupSync();
        Scan[threadId] = childLeftLeafSum + childRightLeafSum;
        GroupMemoryBarrierWithGroupSync();
    }

    return GetPrefixSum(threadId);
}