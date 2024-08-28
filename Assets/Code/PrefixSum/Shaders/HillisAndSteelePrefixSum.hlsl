#include "..//Threads.hlsl"
groupshared int groupPrefixSum[BLOCKS][PREFIX_SUM_SIZE];

int ComputeInclusivePrefixSumForGroup(int3 threadId, int value)
{
    groupPrefixSum[threadId.y][threadId.x] = value;
    GroupMemoryBarrierWithGroupSync();

    for (int offset = 1; offset < PREFIX_SUM_SIZE; offset *= 2)
    {
        bool inBounds = threadId.x >= offset;
        int childLeftLeafSum = inBounds ? groupPrefixSum[threadId.y][threadId.x - offset] : 0;
        int childRightLeafSum = groupPrefixSum[threadId.y][threadId.x];
        
        GroupMemoryBarrierWithGroupSync();
        groupPrefixSum[threadId.y][threadId.x] = childLeftLeafSum + childRightLeafSum;
    }

    return groupPrefixSum[threadId.y][threadId.x];
}

int ComputeExclusivePrefixSumForGroup(int3 threadId, int value)
{
    return ComputeInclusivePrefixSumForGroup(threadId, value) - value;
}

uint GetGroupInclusivePrefixSum(int threadIdX, int threadIdY)
{
    return groupPrefixSum[threadIdY][threadIdX];
}

uint GetGroupSum(int threadIdY)
{
    return GetGroupInclusivePrefixSum(THREAD_LAST_INDEX, threadIdY);
}