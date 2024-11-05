
namespace Code.Components.MortonCodeAssignment.TestTree
{
    public class TreeCalculator
    {
        private readonly BVHNode[] _nodes;
        private readonly uint _rootIndex;

        public TreeCalculator(BVHNode[] nodes, uint rootIndex)
        {
            _rootIndex = rootIndex;
            _nodes = nodes;
        }

        public TreeNode Compute()
        {
            TreeNode root = new(_rootIndex);
            Traverse(root, _nodes[_rootIndex].LeftChild(), true);
            Traverse(root, _nodes[_rootIndex].RightChild(), false);
            return root;
        }

        private bool IsInnerNode(uint nodeIndex)
        {
            int leavesCount = (_nodes.Length + 1) / 2;
            return nodeIndex < leavesCount - 1;
        }
        
        private void Traverse(TreeNode parent, uint nodeIndex, bool isLeft)
        {
            TreeNode child = new(nodeIndex);
            if (parent != null)
            {
                if (isLeft)
                    parent.Left = child;
                else
                    parent.Right = child;
            }

            if (IsInnerNode(nodeIndex))
            {
                Traverse(child, _nodes[nodeIndex].LeftChild(), true);
                Traverse(child, _nodes[nodeIndex].RightChild(), false);
            }
        }
    }
}