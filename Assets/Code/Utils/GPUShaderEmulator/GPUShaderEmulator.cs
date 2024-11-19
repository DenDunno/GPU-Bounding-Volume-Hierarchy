using Code.Utils.Extensions;
using Unity.Jobs;

namespace Code.Utils.GPUShaderEmulator
{
    public class GPUShaderEmulator<TBlockTask> where TBlockTask : struct, IBlockTask
    {
        private readonly TBlockTask _task;
        private readonly int _blockSize;
        private readonly int _blocks;

        public GPUShaderEmulator(int blockSize, int blocks, TBlockTask task)
        {
            _blockSize = blockSize;
            _blocks = blocks;
            _task = task;
        }

        public void Execute()
        {
            int[] groups = EnumerableExtensions.GetRandomOrderedArray(_blocks);

            foreach (int groupId in groups)
            {
                new ThreadBlockBatch<TBlockTask>(_blockSize, groupId, _task)
                    .Schedule(_blockSize, 32)
                    .Complete();
            }
        }
    }
}