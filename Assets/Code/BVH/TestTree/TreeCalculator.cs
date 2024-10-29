using System.Collections.Generic;
using Sirenix.Utilities;
using UnityEngine;

namespace Code.Components.MortonCodeAssignment.TestTree
{
    public class TreeCalculator
    {
        private Dictionary<uint, TreeNode> _subTrees = new();
        private TreeNode _root;

        public TreeNode Compute(ComputeBuffer nodeBuffer, int length)
        {
            BVHNode[] nodes = new BVHNode[length];
            nodeBuffer.GetData(nodes);

            for (uint i = 0; i < nodes.Length; ++i)
            {
                Traverse(null, i, false, nodes);
            }

            for (int i = 0; i < nodes.Length; ++i)
            {
                Debug.Log($"Index = {i} {nodes[i].ToString()}");
            }

            return _root;
        }

        private void Traverse(TreeNode parent, uint nodeIndex, bool isLeft, BVHNode[] nodes)
        {
            bool isVisited = _subTrees.ContainsKey(nodeIndex); 
            TreeNode child = isVisited ? _subTrees[nodeIndex] : new TreeNode(nodeIndex);

            if (parent != null)
            {
                if (isLeft)
                    parent.Left = child;
                else
                    parent.Right = child;
            }
            
            if (isVisited && parent != null && Contains(_root, parent) == false)
            {
                _root = parent;
                return;
            }
            
            _subTrees[nodeIndex] = child;
            
            if (nodeIndex < nodes.Length)
                Traverse(child, nodes[nodeIndex].LeftChild(), true, nodes);
            
            if (nodeIndex < nodes.Length)
                Traverse(child, nodes[nodeIndex].RightChild(), false, nodes);
        }

        private bool Contains(TreeNode node, TreeNode target)
        {
            bool result = false;
            Contains(node, target, ref result);
            return result;
        }
        
        private void Contains(TreeNode node, TreeNode target, ref bool contains)
        {
            if (contains)
                return;
            
            if (node == null)
                return;
            
            contains = node.Left == target || node.Right == target;
            Contains(node.Left, target, ref contains);
            Contains(node.Right, target, ref contains);
        }
    }
}