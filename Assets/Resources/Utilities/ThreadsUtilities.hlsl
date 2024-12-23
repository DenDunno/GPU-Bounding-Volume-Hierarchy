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


void __SynchronizeAllThreads(uint threadId, uint groupSize, uint threadGroups)
{
    GroupMemoryBarrierWithGroupSync();
    
    if (threadId == groupSize - 1)
    {
        uint blockCounter = 0;
        InterlockedAdd(BlockCounter[0], 1, blockCounter);

        while (blockCounter < threadGroups)
        {
            InterlockedAdd(BlockCounter[0], 0, blockCounter);
        }
    }
}
