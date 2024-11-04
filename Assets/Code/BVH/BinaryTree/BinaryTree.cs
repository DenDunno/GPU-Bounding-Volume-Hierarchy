using System;
using UnityEngine;

namespace Code.Components.MortonCodeAssignment.TestTree
{
    public class BinaryTree
    {
        public readonly TreeNode Root;

        public BinaryTree(TreeNode root)
        {
            Root = root;
        }

        public void Traverse(Action<TreeNode, int> action)
        {
            Traverse(action, Root, 0);
        }
        
        public int ComputeHeight()
        {
            return ComputeHeight(Root, 0);
        }
        
        private void Traverse(Action<TreeNode, int> action, TreeNode node, int depth)
        {
            if (node == null)
                return;
            
            action(node, depth);
            Traverse(action, node.Left, depth + 1);
            Traverse(action, node.Right, depth + 1);
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