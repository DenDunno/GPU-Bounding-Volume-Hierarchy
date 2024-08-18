#include "LeafIndices.hlsl"
RWStructuredBuffer<int> BlockSum;
groupshared int ExclusiveScan[THREADS];

void UpSweep(uint threadId)
{
    for (uint step = 1; step < THREADS; step *= 2)
    {
        GroupMemoryBarrierWithGroupSync();
        uint threadsTotal = ceil(THREADS / (step * 2.0f));

        if (threadId < threadsTotal)
        {
            LeafIndices indices = LeafIndices::Create(threadId, step);
            ExclusiveScan[indices.Right] += ExclusiveScan[indices.Left];
        }
    }
}

void SetLastElementToZero(const uint threadId)
{
    if (threadId == THREAD_LAST_INDEX)
    {
        ExclusiveScan[THREAD_LAST_INDEX] = 0;
    }
}

void DownSweep(uint threadId)
{
    SetLastElementToZero(threadId);
    
    for (uint threadsTotal = 1, step = THREADS / 2; threadsTotal < THREADS; threadsTotal *= 2, step /= 2)
    {
        GroupMemoryBarrierWithGroupSync();
        if (threadId < threadsTotal)
        {
            LeafIndices indices = LeafIndices::Create(threadId, step);
            const int rightLeaf = ExclusiveScan[indices.Right];
            const int leftLeaf = ExclusiveScan[indices.Left];
            
            ExclusiveScan[indices.Left] = rightLeaf;
            ExclusiveScan[indices.Right] = rightLeaf + leftLeaf;
        }
    }

    GroupMemoryBarrierWithGroupSync();
}

void MoveDataToSharedMemory(uint threadId, int value)
{
    ExclusiveScan[threadId] = value;
}

int GetExclusivePrefixSum(uint threadId)
{
    return ExclusiveScan[threadId];
}

int GetExclusivePrefixSumForGroup()
{
    return GetExclusivePrefixSum(THREAD_LAST_INDEX);
}

int GetInclusivePrefixSum(uint threadId, int originalValue)
{
    return GetExclusivePrefixSum(threadId) + originalValue;
}

int GetInclusivePrefixSumForGroup(int originalValue)
{
    return GetInclusivePrefixSum(THREAD_LAST_INDEX, originalValue);
}

void TryWriteToBlockSum(uint threadId, uint groupId, int value)
{
    if (threadId == THREAD_LAST_INDEX)
    {
        BlockSum[groupId] = GetInclusivePrefixSumForGroup(value);
    }
}

int ComputeExclusivePrefixSum(uint threadId, int inputValue)
{
    MoveDataToSharedMemory(threadId, inputValue);
    UpSweep(threadId);
    DownSweep(threadId);
    return GetExclusivePrefixSum(threadId);
}