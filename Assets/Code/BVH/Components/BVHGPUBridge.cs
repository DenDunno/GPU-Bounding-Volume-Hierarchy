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
        
        public BVHNode[] FetchTree()
        {
            BVHNode[] tree = _buffers.Tree.FetchData<BVHNode>(_leavesCount + _leavesCount - 1);
            BVHNode[] root = _buffers.Nodes.FetchData<BVHNode>(1);
            tree[^1] = root[0];
            
            return tree;
        }

        public BVHNode[] FetchInnerNodes() => _buffers.Nodes.FetchData<BVHNode>(_leavesCount - 1);
        public int FetchRoot() => _buffers.Root.FetchValue<int>();
    }
}