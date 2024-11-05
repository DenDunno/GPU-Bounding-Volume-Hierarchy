using System.Collections.Generic;
using UnityEngine;

namespace Code.Components.MortonCodeAssignment.TestTree
{
    public class BinaryTreeSize
    {
        private readonly Dictionary<uint, int> _widthPerNode = new();
        private readonly TreeNode _root;

        public BinaryTreeSize(TreeNode root)
        {
            _root = root;
        }
        
        public int Height { get; private set; }
        
        public void Initialize()
        {
            Height = ComputeHeight(_root, 0) - 1;
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

        private int ComputeHeight(TreeNode node, int depth)
        {
            if (node == null) 
                return depth;
            
            return Mathf.Max(
                ComputeHeight(node.Left, depth + 1),
                ComputeHeight(node.Right, depth + 1));
        }
    }
}