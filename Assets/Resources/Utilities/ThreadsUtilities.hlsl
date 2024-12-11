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

#define IF_TARGET_THREAD(threadId, targetThreadId, CODE) \
if (threadId == targetThreadId) \
{ \
    CODE \
} \

#define WAIT_FOR_PREVIOUS_GROUPS_SINGLE(threadId, targetThreadId, groupId, CODE) \
IF_TARGET_THREAD(threadId, targetThreadId, \
    __WaitForPreviousThreadGroups(groupId); \
    CODE \
    __UnlockNextGroup(); \
) \
GroupMemoryBarrierWithGroupSync();

#define WAIT_FOR_PREVIOUS_GROUPS(threadId, targetThreadId, groupId, CODE) \
IF_TARGET_THREAD(threadId, targetThreadId, __WaitForPreviousThreadGroups(groupId);) \
GroupMemoryBarrierWithGroupSync(); \
CODE \
GroupMemoryBarrierWithGroupSync(); \
IF_TARGET_THREAD(threadId, targetThreadId, __UnlockNextGroup();)