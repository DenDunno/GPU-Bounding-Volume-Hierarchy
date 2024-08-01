#include "PrefixSum.hlsl"

int GetBitAtPosition(int value, int bitOffset)
{
    const int mask = 1 << bitOffset;
    return (value & mask) == mask;
}

int ComputeScatterIndex(const int threadId, const int groupId, const int bit, const int prefixSum)
{
    const int lastIndex = THREADS_PER_GROUP - 1;
    const int totalFalseBits = GetGroupInclusivePrefixSum(lastIndex);
    const int indexIfBitIsFalse = prefixSum;
    const int indexIfBitIsTrue = threadId - indexIfBitIsFalse + totalFalseBits;
    const int localScatterIndex = bit ? indexIfBitIsTrue : indexIfBitIsFalse;
    const int globalScatterIndex = groupId * THREADS_PER_GROUP + localScatterIndex;
    
    return globalScatterIndex;
}