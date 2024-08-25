#include "..//Common.hlsl"
groupshared int InclusiveScan[THREADS];
RWStructuredBuffer<int> BlockSum;

void Reduce(uint threadId)
{
    for (uint step = 1, threadsTotal = THREADS / 2; step < THREADS; step *= 2, threadsTotal >>= 1)
    {
        GroupMemoryBarrierWithGroupSync();

        if (threadId < threadsTotal)
        {
            int rightIndex = 2 * step * (threadId + 1) - 1;
            int leftIndex = rightIndex - step;

            InclusiveScan[rightIndex] += InclusiveScan[leftIndex];
        }
    }
}

void DownSweep(uint threadId)
{
    for (uint threadsTotal = 2, step = THREADS / 2; threadsTotal < THREADS; threadsTotal <<= 1, step >>= 1)
    {
        GroupMemoryBarrierWithGroupSync();
        if (threadId < threadsTotal - 1)
        {
            int leftIndex = step * (threadId + 1) - 1;
            int rightIndex = leftIndex + step / 2;
            
            InclusiveScan[rightIndex] += InclusiveScan[leftIndex];
        }
    }

    GroupMemoryBarrierWithGroupSync();
}

void MoveDataToSharedMemory(uint threadId, int value)
{
    InclusiveScan[threadId] = value;
}

int GetInclusivePrefixSum(uint threadId)
{
    return InclusiveScan[threadId];
}

void WriteChunkSum(uint threadId, uint groupId)
{
    if (threadId == THREAD_LAST_INDEX)
    {
        BlockSum[groupId] = InclusiveScan[THREAD_LAST_INDEX];
    }
}

int ComputeInclusivePrefixSum(uint threadId, int inputValue)
{
    MoveDataToSharedMemory(threadId, inputValue);
    Reduce(threadId);
    DownSweep(threadId);
    return GetInclusivePrefixSum(threadId);
}

int ComputeExclusivePrefixSum(uint threadId, int inputValue)
{
    return ComputeInclusivePrefixSum(threadId, inputValue) - inputValue;
}