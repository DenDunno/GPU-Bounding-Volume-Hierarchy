using System.Collections.Generic;
using Code.Utils.Extensions;
using UnityEngine;

namespace Code.Components.MortonCodeAssignment.TestTree
{
    public class TreeCalculator
    {
        private readonly Dictionary<uint, TreeNode> _subTrees = new();
        private TreeNode _root;

        public TreeNode Compute(BVHBuffers buffers, int length, bool showNodes)
        {
            BVHNode[] nodes = new BVHNode[length];
            buffers.Nodes.GetData(nodes);

            Build(nodes);
            TryPrint(buffers, showNodes, nodes);

            return _root;
        }

        private void Build(BVHNode[] nodes)
        {
            for (uint i = 0; i < nodes.Length; ++i)
            {
                Traverse(null, i, false, nodes);
            }
        }

        private static void TryPrint(BVHBuffers buffers, bool showNodes, BVHNode[] nodes)
        {
            if (showNodes)
            {
                for (int i = 0; i < nodes.Length; ++i)
                {
                    Debug.Log($"Index = {i} {nodes[i].ToString()}");
                }
                
                buffers.Root.PrintInt("Root = ");
            }
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