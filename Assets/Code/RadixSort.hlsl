#include "PrefixSum.hlsl"

int GetBitAtPosition(int value, int bitOffset)
{
    const int mask = 1 << bitOffset;
    return (value & mask) == mask;
}

int ConvertLocalIndexToGlobal(int index, int groupId)
{
    return groupId * THREADS_PER_GROUP + index;
}

int GetGroupClampedLastIndex(int groupId, int arrayLength)
{
    const int groupLastIndex = THREADS_PER_GROUP - 1;
    int globalIndex = ConvertLocalIndexToGlobal(groupLastIndex, groupId);
    int offset = min(arrayLength - globalIndex - 1, 0);

    return groupLastIndex + offset;
}

int ComputeScatterIndex(int threadId, int groupId, int bit, int prefixSum, int arrayLength)
{
    const int groupLastIndex = GetGroupClampedLastIndex(groupId, arrayLength);
    const int totalFalseBits = GetGroupInclusivePrefixSum(groupLastIndex);
    const int indexIfBitIsFalse = prefixSum;
    const int indexIfBitIsTrue = threadId - indexIfBitIsFalse + totalFalseBits;
    const int localScatterIndex = bit ? indexIfBitIsTrue : indexIfBitIsFalse;
    const int globalScatterIndex = groupId * THREADS_PER_GROUP + localScatterIndex;
    
    return globalScatterIndex;
}