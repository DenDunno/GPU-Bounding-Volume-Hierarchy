using Code.Utils.Extensions;

namespace Code.Components.MortonCodeAssignment
{
    public class BVHGPUBridge
    {
        private readonly BVHBuffers _buffers;
        private readonly int _leavesCount;
        private readonly BVHContent _content;

        public BVHGPUBridge(BVHBuffers buffers, int leavesCount)
        {
            _leavesCount = leavesCount;
            _buffers = buffers;
        }

        public void SendBoxesToGPU() => _buffers.Boxes.SetData(_content.BoundingBoxes);
        public BVHNode[] FetchTree() => _buffers.Tree.FetchData<BVHNode>(2 * _leavesCount);
        public BVHNode[] FetchInnerNodes() => _buffers.Nodes.FetchData<BVHNode>(_leavesCount - 1);
        public int FetchRoot() => _buffers.Root.FetchValue<int>();
    }
}