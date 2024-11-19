using Unity.Burst;
using Unity.Jobs;

namespace Code.Utils.GPUShaderEmulator
{
    [BurstCompile]
    public struct ThreadBlockBatch<TBlockTask> : IJobParallelFor where TBlockTask : struct, IBlockTask
    {
        private readonly int _threadsPerGroup;
        private readonly int _groupId;
        private TBlockTask _blockTask;

        public ThreadBlockBatch(int threadsPerGroup, int groupId, TBlockTask blockTask)
        {
            _threadsPerGroup = threadsPerGroup;
            _blockTask = blockTask;
            _groupId = groupId;
        }

        [BurstCompile]
        public void Execute(int index)
        {
            _blockTask.Execute(_threadsPerGroup, 
                new ThreadId(index, _groupId, index + _groupId * _threadsPerGroup));
        }
    }
}