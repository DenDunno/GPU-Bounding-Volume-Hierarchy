using Code.Components.MortonCodeAssignment.TestTree;
using Code.Utils.Extensions;
using EditorWrapper;
using UnityEngine;

namespace Code.Components.MortonCodeAssignment
{
    public class BVHTreeVisualization : IDrawable
    {
        private readonly BVHTreeVisualizationData _data;
        private readonly BVHNode[] _nodes;
        private readonly TreeNode _root;
        private readonly int _height;

        public BVHTreeVisualization(BVHNode[] nodes, int height, TreeNode root, BVHTreeVisualizationData data)
        {
            _height = height;
            _nodes = nodes;
            _data = data;
            _root = root;
        }

        public void Draw()
        {
            GizmosUtils.SetColor(_data.Color);
            DrawBVH(_root, 0);
            GizmosUtils.RestoreColor();
            RandomUtils.RestoreState();
        }

        private void DrawBVH(TreeNode node, int depth)
        {
            if (node == null)
                return;

            if (_data.ShowAll && _height - depth < _data.Depth || depth == _data.VisibleDepth)
            {
                RandomUtils.InitState(depth);
                Color color = RandomUtils.GenerateBrightColor();
                color.a *= _data.ShowAll ? Mathf.Lerp(0.1f, 1f, (float)depth / _height) : 1f;
                Gizmos.color = color;
                _nodes[node.Id].Box.Draw();
            }

            DrawBVH(node.Left, depth + 1);
            DrawBVH(node.Right, depth + 1);
        }
    }
}