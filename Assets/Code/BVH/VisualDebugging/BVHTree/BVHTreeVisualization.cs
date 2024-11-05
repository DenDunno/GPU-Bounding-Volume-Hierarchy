using Code.Components.MortonCodeAssignment.TestTree;
using EditorWrapper;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.Components.MortonCodeAssignment
{
    public class BVHTreeVisualization : IDrawable
    {
        private readonly BVHTreeVisualizationData _data;
        private readonly BVHNode[] _innerNodes;
        private readonly TreeNode _root;
        private readonly int _height;

        public BVHTreeVisualization(BVHNode[] innerNodes, int height, TreeNode root, BVHTreeVisualizationData data)
        {
            _innerNodes = innerNodes;
            _height = height;
            _data = data;
            _root = root;
        }

        public void Draw()
        {
            GizmosUtils.SetColor(_data.Color);
            DrawBVH(_root, 0);
            GizmosUtils.RestoreColor();
        }

        private void DrawBVH(TreeNode node, int depth)
        {
            if (node == null || node.Id >= _innerNodes.Length)
                return;

            if (_data.ShowAll && _height - depth < _data.Depth || depth == _data.VisibleDepth)
            {
                Random.InitState(depth);
                Color color = RandomUtils.GenerateBrightColor();
                color.a = _data.ShowAll ? Mathf.Pow((float)depth / _height, _data.Alpha)  : 0.25f; 
                Gizmos.color = color; 
                _innerNodes[node.Id].Box.Draw();
            }

            DrawBVH(node.Left, depth + 1);
            DrawBVH(node.Right, depth + 1);
        }
    }
}