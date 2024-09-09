
#ifndef THREADS
#error "THREADS is not defined. Please define 'THREADS' before including 'BlellochPrefixSum.hlsl'"
#endif

#ifndef PREFIX_SUM_ROWS
#define PREFIX_SUM_ROWS 1
#endif

#define THREAD_LAST_INDEX (THREADS - 1)
groupshared int Scan[PREFIX_SUM_ROWS][THREADS];
RWStructuredBuffer<int> ScanBlockSum;

int ComputeInclusivePrefixSum(int inputValue, int threadId, int rowId = 0);

void MoveDataToSharedMemory(int rowId, int threadId, int value)
{
    Scan[rowId][threadId] = value;
    GroupMemoryBarrierWithGroupSync();
}

int GetPrefixSum(int rowId, int threadId)
{
    return Scan[rowId][threadId];
}

int ComputeExclusivePrefixSum(int inputValue, int threadId, int rowId = 0)
{
    const int inclusivePrefixSum = ComputeInclusivePrefixSum(inputValue, threadId, rowId);
    const int exclusivePrefixSum = inclusivePrefixSum - inputValue;
    Scan[rowId][threadId] = exclusivePrefixSum;
    GroupMemoryBarrierWithGroupSync();
    
    return exclusivePrefixSum;
}