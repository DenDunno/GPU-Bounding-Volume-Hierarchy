
struct ThreadId
{
    int Local;
    int Group;
    int Global;

    static ThreadId Create(int globalId, int threadId, int groupId)
    {
        ThreadId result;
        result.Local = threadId;
        result.Group = groupId;
        result.Global = globalId;

        return result;
    }
};