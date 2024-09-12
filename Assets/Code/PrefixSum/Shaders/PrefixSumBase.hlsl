
#ifndef THREADS
#error "THREADS is not defined. Please define 'THREADS' before including Scan library"
#endif

#ifndef TYPE
#define TYPE int
#endif

#define THREAD_LAST_INDEX (THREADS - 1)
groupshared TYPE Scan[THREADS];
RWStructuredBuffer<TYPE> ScanBlockSum;

TYPE ComputeInclusiveScan(TYPE inputValue, int threadId);

void MoveDataToSharedMemory(int threadId, TYPE value)
{
    Scan[threadId] = value;
    GroupMemoryBarrierWithGroupSync();
}

TYPE GetPrefixSum(int threadId)
{
    return Scan[threadId];
}

TYPE ComputeExclusiveScan(TYPE inputValue, int threadId)
{
    const TYPE inclusivePrefixSum = ComputeInclusiveScan(inputValue, threadId);
    const TYPE exclusivePrefixSum = inclusivePrefixSum - inputValue;
    Scan[threadId] = exclusivePrefixSum;
    GroupMemoryBarrierWithGroupSync();
    
    return exclusivePrefixSum;
}