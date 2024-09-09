#ifndef THREADS
#error "THREADS is not defined. Please define 'THREADS' before including 'BlellochPrefixSum.hlsl'"
#endif

#ifndef PREFIX_SUM_ROWS
#error "PREFIX_SUM_ROWS is not defined. Please define 'PREFIX_SUM_ROWS' before including 'BlellochPrefixSum.hlsl'"
#endif

#define THREAD_LAST_INDEX (THREADS - 1)
groupshared int Scan[PREFIX_SUM_ROWS][THREADS];
RWStructuredBuffer<int> ScanBlockSum;

void Reduce(uint rowId, uint threadId)
{
    [unroll]
    for (uint step = 1, threadsTotal = THREADS / 2; step < THREADS; step *= 2, threadsTotal >>= 1)
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

void DownSweep(uint rowId, uint threadId)
{
    [unroll]
    for (uint threadsTotal = 2, step = THREADS / 2; threadsTotal < THREADS; threadsTotal <<= 1, step >>= 1)
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

void MoveDataToSharedMemory(uint rowId, uint threadId, int value)
{
    Scan[rowId][threadId] = value;
}

int GetInclusivePrefixSum(uint rowId, uint threadId)
{
    return Scan[rowId][threadId];
}

int ComputeInclusivePrefixSum(int inputValue, uint threadId, uint rowId = 0)
{
    MoveDataToSharedMemory(rowId, threadId, inputValue);
    Reduce(rowId, threadId);
    DownSweep(rowId, threadId);
    return GetInclusivePrefixSum(rowId, threadId);
}

int ComputeExclusivePrefixSum(int inputValue, uint threadId, uint rowId = 0)
{
    const int inclusivePrefixSum = ComputeInclusivePrefixSum(inputValue, threadId, rowId);
    const int exclusivePrefixSum = inclusivePrefixSum - inputValue;
    Scan[rowId][threadId] = exclusivePrefixSum;
    GroupMemoryBarrierWithGroupSync();
    
    return exclusivePrefixSum;
}