using Code.Components.MortonCodeAssignment.TestTree;
using EditorWrapper;

namespace Code.Components.MortonCodeAssignment
{
    public class BVHVisualizationFactory
    {
        private readonly BVHGPUBridge _gpuBridge;
        private readonly VisualizationData _data;
        private readonly BVHContent _content;

        public BVHVisualizationFactory(BVHGPUBridge gpuBridge, BVHContent content, VisualizationData data)
        {
            _gpuBridge = gpuBridge;
            _content = content;
            _data = data;
        }

        public IDrawable Create()
        {
            return _data.Show && _content.Count > 0 ?
                CreateVisualization() :
                DummyDrawable.Instance;
        }

        private IDrawable CreateVisualization()
        {
            BVHNode[] innerNodes = _gpuBridge.FetchInnerNodes();
            uint rootIndex = (uint)_gpuBridge.FetchRoot();
            int nodesCount = innerNodes.Length;
            
            TreeNode root = new TreeCalculator(innerNodes, rootIndex).Compute();
            BinaryTreeVisualization binaryTree = new(_data.BinaryTree, root, nodesCount);
            binaryTree.Initialize();

            return new DrawableComposite(new IDrawable[]
            {
                new DrawableIfTrue(binaryTree, _data.BinaryTree.Show),
            });
        }
    }
}