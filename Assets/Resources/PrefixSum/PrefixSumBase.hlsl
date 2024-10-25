
#ifndef THREADS
#error "THREADS is not defined. Please define 'THREADS' before including Scan library"
#endif

#ifndef PREFIX_SUM_TYPE
#define PREFIX_SUM_TYPE int
#endif

#define THREAD_LAST_INDEX (THREADS - 1)
groupshared PREFIX_SUM_TYPE Scan[THREADS];
RWStructuredBuffer<PREFIX_SUM_TYPE> ScanBlockSum;

PREFIX_SUM_TYPE ComputeInclusiveScan(const PREFIX_SUM_TYPE inputValue, int threadId);

void MoveDataToSharedMemory(int threadId, PREFIX_SUM_TYPE value)
{
    Scan[threadId] = value;
    GroupMemoryBarrierWithGroupSync();
}

PREFIX_SUM_TYPE GetPrefixSum(int threadId)
{
    return Scan[threadId];
}

PREFIX_SUM_TYPE ComputeExclusiveScan(const PREFIX_SUM_TYPE inputValue, int threadId)
{
    const PREFIX_SUM_TYPE inclusivePrefixSum = ComputeInclusiveScan(inputValue, threadId);
    const PREFIX_SUM_TYPE exclusivePrefixSum = inclusivePrefixSum - inputValue;
    Scan[threadId] = exclusivePrefixSum;
    GroupMemoryBarrierWithGroupSync();
    
    return exclusivePrefixSum;
}