#include "PrefixSumBase.hlsl"

void Reduce(int threadId)
{
    [unroll]
    for (int step = 1, threadsTotal = THREADS / 2; step < THREADS; step *= 2, threadsTotal >>= 1)
    {
        GroupMemoryBarrierWithGroupSync();

        if (threadId < threadsTotal)
        {
            int rightIndex = 2 * step * (threadId + 1) - 1;
            int leftIndex = rightIndex - step;

            Scan[rightIndex] += Scan[leftIndex];
        }
    }
}

void DownSweep(int threadId)
{
    [unroll]
    for (int threadsTotal = 2, step = THREADS / 2; threadsTotal < THREADS; threadsTotal <<= 1, step >>= 1)
    {
        GroupMemoryBarrierWithGroupSync();
        
        if (threadId < threadsTotal - 1)
        {
            int leftIndex = step * (threadId + 1) - 1;
            int rightIndex = leftIndex + step / 2;
            
            Scan[rightIndex] += Scan[leftIndex];
        }
    }

    GroupMemoryBarrierWithGroupSync();
}

PREFIX_SUM_TYPE ComputeInclusiveScan(const PREFIX_SUM_TYPE inputValue, int threadId)
{
    MoveDataToSharedMemory(threadId, inputValue);
    Reduce(threadId);
    DownSweep(threadId);
    return GetPrefixSum(threadId);
}