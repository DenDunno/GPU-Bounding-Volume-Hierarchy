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

        public IDrawable Create(BVHNode[] nodes, int rootIndex)
        {
            int leavesCount = (nodes.Length + 1) / 2;
            TreeNode root = new TreeCalculator(nodes, (uint)rootIndex).Compute();
            BinaryTreeSize treeSize = new(root);
            treeSize.Initialize();

            return new DrawableComposite(new IDrawable[]
            {
                new DrawableIfTrue(
                    new BinaryTreeVisualization(Visualization.BinaryTree, root, leavesCount - 1, treeSize),
                    Visualization.BinaryTree.Show),
                new DrawableIfTrue(new BVHTreeVisualization(nodes, treeSize.Height, root, Visualization.BVHTree),
                    Visualization.BVHTree.Show),
                new DrawableIfTrue(_data.SceneSize.Box, _data.SceneSize.Show),
            });
        }
    }
}