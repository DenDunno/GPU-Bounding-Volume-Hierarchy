
RWStructuredBuffer<int> BlockSum;
uniform uint ThreadGroups;

int GetBlockSumId(int sortValue, uint groupId)
{
    return sortValue * ThreadGroups + groupId;
}

void WriteToBlockSum(int sortValue, uint groupId, int input)
{
    BlockSum[GetBlockSumId(sortValue, groupId)] = input;
}

int ReadBlockSumPrefixSum(int sortValue, uint groupId)
{
    return BlockSum[GetBlockSumId(sortValue, groupId)];
}