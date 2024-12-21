
RWStructuredBuffer<int> BlockSum;
uniform int ThreadGroups;

int GetBlockSumId(const int sortValue, const int groupId)
{
    return sortValue * ThreadGroups + groupId;
}

void WriteToBlockSum(const int sortValue, const int groupId, const int input)
{
    BlockSum[GetBlockSumId(sortValue, groupId)] = input;
}

int ReadBlockSumPrefixSum(const int sortValue, const int groupId)
{
    return BlockSum[GetBlockSumId(sortValue, groupId)];
}

void PerformBlockSumPrefixSum(const uint globalId)
{
    uint blockSumLength;
    uint stride;

    BlockSum.GetDimensions(blockSumLength, stride);
}