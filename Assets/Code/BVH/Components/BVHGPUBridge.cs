using Code.Utils.Extensions;

namespace Code.Components.MortonCodeAssignment
{
    public class BVHGPUBridge
    {
        private readonly TreeConstructionBuffers _buffers;
        private readonly int _leavesCount;

        public BVHGPUBridge(TreeConstructionBuffers buffers, int leavesCount)
        {
            _leavesCount = leavesCount;
            _buffers = buffers;
        }

        public BVHNode[] FetchTree()
        {
            return _buffers.Nodes.FetchData<BVHNode>(_leavesCount + _leavesCount - 1);;
        }
    }
}