using Code.Utils.Extensions;
using Unity.Jobs;

namespace Code.Utils.GPUShaderEmulator
{
    public class GPUShaderEmulator<TBlockTask> where TBlockTask : struct, IBlockTask
    {
        private readonly TBlockTask _task;
        private readonly int _blockSize;
        private readonly int _groups;

        public GPUShaderEmulator(int blockSize, int groups, TBlockTask task)
        {
            _blockSize = blockSize;
            _groups = groups;
            _task = task;
        }

        public void Execute(int groupId)
        {
            ThreadBlockBatch<TBlockTask> batch = new(_blockSize, groupId, _task);
            batch.Schedule(_blockSize, 32).Complete();
        }

        public void Execute()
        {
            int[] groups = EnumerableExtensions.GetRandomOrderedArray(_groups);

            foreach (int groupId in groups)
            {
                Execute(groupId);
            }
        }
    }
}