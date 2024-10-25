
RWStructuredBuffer<int> BlockSum;
uniform int ThreadGroups;

int GetBlockSumId(int sortValue, int groupId)
{
    return sortValue * ThreadGroups + groupId;
}

void WriteToBlockSum(int sortValue, int groupId, int input)
{
    BlockSum[GetBlockSumId(sortValue, groupId)] = input;
}

int ReadBlockSumPrefixSum(int sortValue, int groupId)
{
    return BlockSum[GetBlockSumId(sortValue, groupId)];
}