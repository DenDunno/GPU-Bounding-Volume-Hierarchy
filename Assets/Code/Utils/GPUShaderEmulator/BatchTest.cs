using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Code.Utils.GPUShaderEmulator
{
    public struct BatchTest : IBlockTask
    {
        [NativeDisableContainerSafetyRestriction] private NativeArray<int> _buffer;

        public BatchTest(NativeArray<int> buffer)
        {
            _buffer = buffer;
        }

        public void Execute(int threadsPerBlock, ThreadId threadId)
        {
        }
    }
}