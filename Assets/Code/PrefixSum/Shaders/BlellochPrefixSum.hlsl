#ifndef THREADS
#error "THREADS is not defined. Please define 'THREADS' before including 'BlellochPrefixSum.hlsl'"
#endif

#define THREAD_LAST_INDEX (THREADS - 1)
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

int GetGroupSum()
{
    return InclusiveScan[THREAD_LAST_INDEX];
}

void WriteChunkSum(uint threadId, uint groupId)
{
    if (threadId == THREAD_LAST_INDEX)
    {
        BlockSum[groupId] = InclusiveScan[THREAD_LAST_INDEX];
    }
}

int ComputeInclusivePrefixSum(int inputValue, uint threadId)
{
    MoveDataToSharedMemory(threadId, inputValue);
    Reduce(threadId);
    DownSweep(threadId);
    return GetInclusivePrefixSum(threadId);
}

int ComputeExclusivePrefixSum(int inputValue, uint threadId)
{
    return ComputeInclusivePrefixSum(inputValue, threadId) - inputValue;
}