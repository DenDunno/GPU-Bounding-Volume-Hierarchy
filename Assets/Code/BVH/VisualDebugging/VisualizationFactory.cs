using Code.Components.MortonCodeAssignment.TestTree;
using EditorWrapper;

namespace Code.Components.MortonCodeAssignment
{
    public class VisualizationFactory
    {
        private readonly BVHData _data;

        public VisualizationFactory(BVHData data)
        {
            _data = data;
        }

        private VisualizationData Visualization => _data.Visualization;

        public IDrawable Create()
        {
            return _data.Nodes.Length == 0 ? DummyDrawable.Instance : CreateVisualization();
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
                new DrawableIfTrue(_data.SceneSize.Box, _data.SceneSize.Show)
            });
        }
    }
}