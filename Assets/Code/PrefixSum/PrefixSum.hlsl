#pragma kernel UpSweep
#pragma kernel DownSweep

#include "..//Common.hlsl"
groupshared int groupPrefixSum[BLOCK_SIZE][PREFIX_SUM_SIZE];

int ComputeExclusivePrefixSumForGroup(int3 threadId, int value)
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

    return groupPrefixSum[threadId.y][threadId.x] - value;
}

uint GetGroupInclusivePrefixSum(int threadIdX, int threadIdY)
{
    return groupPrefixSum[threadIdY][threadIdX];
}

uint GetGroupSum(int threadIdY)
{
    return GetGroupInclusivePrefixSum(LAST_INDEX, threadIdY);
}

[numthreads(THREADS_PER_GROUP,1,1)]
void UpSweep(int3 id : SV_DispatchThreadID)
{
}

[numthreads(THREADS_PER_GROUP,1,1)]
void DownSweep(int3 id : SV_DispatchThreadID)
{
}