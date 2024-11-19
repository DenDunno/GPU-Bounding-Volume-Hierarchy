namespace Code.Utils.GPUShaderEmulator
{
    public interface IBlockTask
    {
        void Execute(int threadsPerBlock, ThreadId threadId);
    }
}