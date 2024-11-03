using Code.Utils.Extensions;

namespace Code.Components.MortonCodeAssignment
{
    public class BVHGPUBridge
    {
        private readonly BVHBuffers _buffers;
        private readonly BVHContent _content;

        public BVHGPUBridge(BVHBuffers buffers, BVHContent content)
        {
            _buffers = buffers;
            _content = content;
        }

        public void SendBoxesToGPU() => _buffers.Boxes.SetData(_content.BoundingBoxes);
        public BVHNode[] FetchInnerNodes() => _buffers.Nodes.FetchData<BVHNode>(_content.Count - 1);
        public int FetchRoot() => _buffers.Root.FetchValue<int>();
    }
}