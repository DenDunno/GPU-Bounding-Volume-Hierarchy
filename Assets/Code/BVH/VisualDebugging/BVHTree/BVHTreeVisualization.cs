using Code.Components.MortonCodeAssignment.TestTree;
using EditorWrapper;
using UnityEngine;
using Random = UnityEngine.Random;

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
                Gizmos.color = _data.Color; 
                _nodes[node.Id].Box.Draw();
            }

            DrawBVH(node.Left, depth + 1);
            DrawBVH(node.Right, depth + 1);
        }
    }
}