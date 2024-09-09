#include "PrefixSumBase.hlsl"

int ComputeInclusivePrefixSum(int inputValue, int threadId, int rowId)
{
    MoveDataToSharedMemory(rowId, threadId, inputValue);

    [unroll]
    for (int offset = 1; offset < THREADS; offset *= 2)
    {
        bool inBounds = threadId >= offset;
        int childLeftLeafSum = inBounds ? GetPrefixSum(rowId, threadId - offset) : 0;
        int childRightLeafSum = GetPrefixSum(rowId, threadId);
        
        GroupMemoryBarrierWithGroupSync();
        Scan[rowId][threadId] = childLeftLeafSum + childRightLeafSum;
        GroupMemoryBarrierWithGroupSync();
    }

    return GetPrefixSum(rowId, threadId);
}