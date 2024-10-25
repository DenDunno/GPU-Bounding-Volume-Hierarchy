
struct ThreadId
{
    int Local;
    int Group;
    int Global;

    static ThreadId Create(const int globalId, const int threadId, const int groupId)
    {
        ThreadId result;
        result.Local = threadId;
        result.Group = groupId;
        result.Global = globalId;

        return result;
    }
};