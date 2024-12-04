using Code.Components.MortonCodeAssignment.TestTree;
using EditorWrapper;

namespace Code.Components.MortonCodeAssignment
{
    public class VisualizationFactory
    {
        private readonly BVHGPUBridge _gpuBridge;
        private readonly BVHData _data;

        public VisualizationFactory(BVHData data, BVHGPUBridge gpuBridge)
        {
            _gpuBridge = gpuBridge;
            _data = data;
        }

        private VisualizationData Visualization => _data.Visualization;

        public IDrawable Create()
        {
            BVHNode[] tree = _gpuBridge.FetchTree();
            int leavesCount = (tree.Length + 1) / 2;
            return new DrawableIfTrue(new NearestNeighbourVisualization(tree, leavesCount),
                _data.Visualization.ShowNearestNeighbours);
            //return _data.Nodes.Length == 0 ? DummyDrawable.Instance : CreateVisualization();
        }

        private IDrawable CreateVisualization()
        {
            TreeNode root = new TreeCalculator(_data.Nodes, (uint)_data.Root).Compute();
            BinaryTreeSize treeSize = new(root);
            treeSize.Initialize();

            int leavesCount = (_data.Nodes.Length + 1) / 2;

            return new DrawableComposite(new IDrawable[]
            {
                new DrawableIfTrue(
                    new BinaryTreeVisualization(Visualization.BinaryTree, root, leavesCount - 1, treeSize),
                    Visualization.BinaryTree.Show),
                new DrawableIfTrue(new BVHTreeVisualization(_data.Nodes, treeSize.Height, root, Visualization.BVHTree),
                    Visualization.BVHTree.Show),
                new DrawableIfTrue(_data.SceneSize.Box, _data.SceneSize.Show),
                new NearestNeighbourVisualization(_data.Nodes, leavesCount)
            });
        }
    }
}