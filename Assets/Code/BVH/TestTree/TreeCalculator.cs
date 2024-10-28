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

            nodes.ForEach(x => Debug.Log(x.ToString()));

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

            if (isVisited)
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
    }
}