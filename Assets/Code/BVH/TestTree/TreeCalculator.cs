using System.Collections.Generic;

namespace Code.Components.MortonCodeAssignment.TestTree
{
    public class TreeCalculator
    {
        private readonly Dictionary<uint, TreeNode> _subTrees = new();
        private readonly BVHNode[] _nodes;
        private TreeNode _root;

        public TreeCalculator(BVHNode[] nodes)
        {
            _nodes = nodes;
        }

        public TreeNode Compute()
        {
            Build();
            return _root;
        }

        private void Build()
        {
            for (uint i = 0; i < _nodes.Length; ++i)
            {
                Traverse(null, i, false, _nodes);
            }
        }

        private void Traverse(TreeNode parent, uint nodeIndex, bool isLeft, BVHNode[] nodes)
        {
            bool isVisited = _subTrees.ContainsKey(nodeIndex); 
            TreeNode child = isVisited ? _subTrees[nodeIndex] : new TreeNode(nodeIndex);

            ConnectChildToParent(parent, isLeft, child);
            
            if (isVisited && parent != null && Contains(_root, parent) == false)
            {
                _root = parent;
                return;
            }

            VisitChildren(nodeIndex, nodes, child);
            _subTrees[nodeIndex] = child;
        }

        private void ConnectChildToParent(TreeNode parent, bool isLeft, TreeNode child)
        {
            if (parent != null)
            {
                if (isLeft)
                    parent.Left = child;
                else
                    parent.Right = child;
            }
        }

        private void VisitChildren(uint nodeIndex, BVHNode[] nodes, TreeNode child)
        {
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