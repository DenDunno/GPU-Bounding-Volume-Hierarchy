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

void ResetBlockCounter()
{
    BlockCounter[0] = 0;
}

#define SYNCHRONIZE(threadId, targetThreadId, groupId, CODE) \
if (threadId == targetThreadId) \
{ \
    __WaitForPreviousThreadGroups(groupId); \
    CODE \
    __UnlockNextGroup(); \
} \
GroupMemoryBarrierWithGroupSync(); \


void __SynchronizeAllThreads(uint threadId, uint groupId, uint groupSize, uint payload)
{
    SYNCHRONIZE(threadId, groupSize - 1, groupId,
        if (groupId * groupSize >= payload)
        {
            BlockCounter[0] = 0;
        })
}
