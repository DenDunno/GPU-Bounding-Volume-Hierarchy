using System;
using Code.Components.MortonCodeAssignment.TestTree;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.Components.MortonCodeAssignment
{
    [Serializable]
    public class BVHTreeDebug : DebugElement
    {
        [SerializeField] [Range(0, 10)] private int _visibleDepth;
        [SerializeField] [Range(0, 10)] private int _depth;
        [SerializeField] private bool _showAll;
        [SerializeField] private float _alpha;
        private BVHNode[] _innerNodes;
        private BinaryTree _tree;
        private int _height;

        public void Initialize(BinaryTree tree, BVHNode[] innerNodes)
        {
            _height = tree.ComputeHeight();
            _innerNodes = innerNodes;
            _tree = tree;
        }

        protected override void OnDraw()
        {
            GizmosUtils.SaveColor();
            _tree?.Traverse(DrawBVH);
            GizmosUtils.RestoreColor();
        }

        private void DrawBVH(TreeNode node, int depth)
        {
            if (node.Id >= _innerNodes.Length || depth > _depth)
                return;

            if (_showAll || depth == _visibleDepth)
            {
                Random.InitState(depth);
                Color color = RandomUtils.GenerateBrightColor();
                color.a = _showAll ? (depth + _alpha + 1f) / _height : 0.25f; 
                Gizmos.color = color; 
                _innerNodes[node.Id].Box.Draw();
            }
            
            DrawBVH(node.Left, depth + 1);
            DrawBVH(node.Right, depth + 1);
        }
    }
}