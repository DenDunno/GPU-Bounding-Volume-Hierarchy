#include "PrefixSumBase.hlsl"

void Reduce(int rowId, int threadId)
{
    [unroll]
    for (int step = 1, threadsTotal = THREADS / 2; step < THREADS; step *= 2, threadsTotal >>= 1)
    {
        GroupMemoryBarrierWithGroupSync();

        if (threadId < threadsTotal)
        {
            int rightIndex = 2 * step * (threadId + 1) - 1;
            int leftIndex = rightIndex - step;

            Scan[rowId][rightIndex] += Scan[rowId][leftIndex];
        }
    }
}

void DownSweep(int rowId, int threadId)
{
    [unroll]
    for (int threadsTotal = 2, step = THREADS / 2; threadsTotal < THREADS; threadsTotal <<= 1, step >>= 1)
    {
        GroupMemoryBarrierWithGroupSync();
        
        if (threadId < threadsTotal - 1)
        {
            int leftIndex = step * (threadId + 1) - 1;
            int rightIndex = leftIndex + step / 2;
            
            Scan[rowId][rightIndex] += Scan[rowId][leftIndex];
        }
    }

    GroupMemoryBarrierWithGroupSync();
}

int ComputeInclusivePrefixSum(int inputValue, int threadId, int rowId)
{
    MoveDataToSharedMemory(rowId, threadId, inputValue);
    Reduce(rowId, threadId);
    DownSweep(rowId, threadId);
    return GetPrefixSum(rowId, threadId);
}