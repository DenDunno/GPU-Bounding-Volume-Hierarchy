#ifndef BLOCKS
#error "BLOCKS is not defined. Please define 'BLOCKS' before including 'BlellochPrefixSum.hlsl'"
#endif

#ifndef THREADS
#error "THREADS is not defined. Please define 'THREADS' before including 'BlellochPrefixSum.hlsl'"
#endif

#define THREAD_LAST_INDEX (THREADS - 1)
groupshared int InclusiveScan[BLOCKS][THREADS];
RWStructuredBuffer<int> BlockSum;

void Reduce(uint threadId, uint blockId)
{
    for (uint step = 1, threadsTotal = THREADS / 2; step < THREADS; step *= 2, threadsTotal >>= 1)
    {
        GroupMemoryBarrierWithGroupSync();

        if (threadId < threadsTotal)
        {
            int rightIndex = 2 * step * (threadId + 1) - 1;
            int leftIndex = rightIndex - step;

            InclusiveScan[blockId][rightIndex] += InclusiveScan[blockId][leftIndex];
        }
    }
}

void DownSweep(uint threadId, uint blockId)
{
    for (uint threadsTotal = 2, step = THREADS / 2; threadsTotal < THREADS; threadsTotal <<= 1, step >>= 1)
    {
        GroupMemoryBarrierWithGroupSync();
        if (threadId < threadsTotal - 1)
        {
            int leftIndex = step * (threadId + 1) - 1;
            int rightIndex = leftIndex + step / 2;
            
            InclusiveScan[blockId][rightIndex] += InclusiveScan[blockId][leftIndex];
        }
    }

    GroupMemoryBarrierWithGroupSync();
}

void MoveDataToSharedMemory(uint threadId, uint blockId, int value)
{
    InclusiveScan[blockId][threadId] = value;
}

int GetInclusivePrefixSum(uint threadId, uint blockId)
{
    return InclusiveScan[blockId][threadId];
}

int GetGroupSum(uint blockId = 0)
{
    return InclusiveScan[blockId][THREAD_LAST_INDEX];
}

void WriteChunkSum(uint threadId, uint groupId)
{
    if (threadId == THREAD_LAST_INDEX)
    {
        BlockSum[groupId] = InclusiveScan[0][THREAD_LAST_INDEX];
    }
}

int ComputeInclusivePrefixSum(int inputValue, uint threadId, uint blockId = 0)
{
    MoveDataToSharedMemory(threadId, blockId, inputValue);
    Reduce(threadId, blockId);
    DownSweep(threadId, blockId);
    return GetInclusivePrefixSum(threadId, blockId);
}

int ComputeExclusivePrefixSum(int inputValue, uint threadId, uint blockId = 0)
{
    return ComputeInclusivePrefixSum(inputValue, threadId, blockId) - inputValue;
}