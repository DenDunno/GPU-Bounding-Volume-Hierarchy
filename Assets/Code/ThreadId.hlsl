
struct ThreadId
{
    uint Local;
    uint Group;
    uint Global;

    static ThreadId Create(uint globalId, uint threadId, uint groupId)
    {
        ThreadId result;
        result.Local = threadId;
        result.Group = groupId;
        result.Global = globalId;

        return result;
    }
};