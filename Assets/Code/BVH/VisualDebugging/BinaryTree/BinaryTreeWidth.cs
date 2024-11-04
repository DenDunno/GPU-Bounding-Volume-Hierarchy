using System.Collections.Generic;
using Code.Components.MortonCodeAssignment.TestTree;

namespace Code.Components.MortonCodeAssignment
{
    public class BinaryTreeWidth
    {
        private readonly Dictionary<uint, int> _widthPerNode = new();
        private readonly TreeNode _root;

        public BinaryTreeWidth(TreeNode root)
        {
            _root = root;
        }

        public void Initialize()
        {
            CalculateWidthPerNodeDFS(_root, _widthPerNode);
        }

        public int GetWidthForNode(uint node)
        {
            return _widthPerNode[node];
        }
        
        private int CalculateWidthPerNodeDFS(TreeNode node, Dictionary<uint, int> widthPerNode)
        {
            if (node == null) return 0;

            return widthPerNode[node.Id] = 1 +
                                           CalculateWidthPerNodeDFS(node.Left, widthPerNode) +
                                           CalculateWidthPerNodeDFS(node.Right, widthPerNode);
        }
    }
}