
namespace Code.Utils.GPUShaderEmulator
{
    public readonly ref struct ThreadId
    {
        public readonly int Global;
        public readonly int Local;
        public readonly int Group;

        public ThreadId(int local, int group, int global)
        {
            Global = global;
            Local = local;
            Group = group;
        }
    }
}