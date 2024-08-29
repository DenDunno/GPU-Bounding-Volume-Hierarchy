
struct ThreadId
{
    uint Local;
    uint Group;
    uint Global;
};

ThreadId CreateThreadId(uint globalId, uint threadId, uint groupId)
{
    ThreadId result;
    result.Local = threadId;
    result.Group = groupId;
    result.Global = globalId;

    return result;
}