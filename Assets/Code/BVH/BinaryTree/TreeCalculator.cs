
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
            Traverse(root);
            return root;
        }

        private bool IsInnerNode(uint index)
        {
            return (int)_nodes[index].LeftChild() >= 0 &&
                   (int)_nodes[index].RightChild() >= 0;
        }

        private void Traverse(TreeNode parent)
        {
            if (IsInnerNode(parent.Id))
            {
                parent.Left = new TreeNode(_nodes[parent.Id].LeftChild());
                parent.Right = new TreeNode(_nodes[parent.Id].RightChild());
            
                Traverse(parent.Left);
                Traverse(parent.Right);
            }
        }
    }
}