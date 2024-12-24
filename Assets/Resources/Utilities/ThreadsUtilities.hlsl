RWStructuredBuffer<uint> BlockCounter;

uint GetLastValidIndexInGroup(uint totalElements, uint groupSize, uint groupId)
{
    return min(groupSize, totalElements - groupId * groupSize);
}

void __WaitForPreviousThreadGroups(uint groupId)
{
    uint blockCounter = 0;
    
    while (blockCounter < groupId)
    {
        InterlockedAdd(BlockCounter[0], 0, blockCounter);
    }
}

void __UnlockNextGroup()
{
    InterlockedAdd(BlockCounter[0], 1);
}

#define SYNCHRONIZE(threadId, targetThreadId, groupId, CODE) \
if (threadId == targetThreadId) \
{ \
    __WaitForPreviousThreadGroups(groupId); \
    CODE \
    __UnlockNextGroup(); \
} \
GroupMemoryBarrierWithGroupSync(); \
