#include "Common.hlsl"

groupshared uint groupPrefixSum[THREADS_PER_GROUP];

int ComputeExclusivePrefixSumForGroup(int threadId, int value)
{
    groupPrefixSum[threadId] = value;
    GroupMemoryBarrierWithGroupSync();

    for (int offset = 1; offset < THREADS_PER_GROUP; offset *= 2)
    {
        bool inBounds = threadId >= offset;
        int childLeftLeafSum = inBounds ? groupPrefixSum[threadId - offset] : 0;
        int childRightLeafSum = groupPrefixSum[threadId];
        
        GroupMemoryBarrierWithGroupSync();
        groupPrefixSum[threadId] = childLeftLeafSum + childRightLeafSum;
    }

    return groupPrefixSum[threadId] - value;
}

int GetGroupInclusivePrefixSum(int threadId)
{
    return groupPrefixSum[threadId];
}