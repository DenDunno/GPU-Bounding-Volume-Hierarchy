using Code.Components.MortonCodeAssignment.TestTree;
using Code.Utils.ShaderUtils;
using EditorWrapper;
using TerraformingTerrain2d;
using UnityEngine;

namespace Code.Components.MortonCodeAssignment
{
    public class BinaryTreeVisualization : IDrawable
    {
        private readonly IShaderBridge<string> _materialBridge;
        private readonly BinaryTreeVisualizationData _data;
        private readonly BinaryTreeSize _treeSize;
        private readonly Material _material;
        private readonly int _nodesCount;
        private readonly TreeNode _root;
        private readonly Mesh _mesh;

        public BinaryTreeVisualization(BinaryTreeVisualizationData data,
            TreeNode root, int nodesCount, BinaryTreeSize treeSize)
        {
            _material = new Material(Resources.Load<Material>("Utilities/Digit"));
            _materialBridge = new MaterialBridge(_material);
            _mesh = MeshExtensions.BuildQuad(1f);
            _nodesCount = nodesCount;
            _treeSize = treeSize;
            _data = data;
            _root = root;
        }

        public void Draw()
        {
            DrawDFS(_root, Vector2.zero, 0);
        }

        private void DrawDFS(TreeNode node, Vector2 parentPosition, float horizontalOffset)
        {
            if (node == null)
                return;

            Vector2 offset = new(horizontalOffset * _data.Offset.x, _data.Offset.y);
            Vector2 childPosition = parentPosition + offset;
            DrawLine(parentPosition, childPosition, _data.LineLength);
            DrawNumber(node, childPosition);
            DrawDFS(node.Left, childPosition, -_treeSize.GetWidthForNode(node.Id));
            DrawDFS(node.Right, childPosition, _treeSize.GetWidthForNode(node.Id));
        }

        private void DrawNumber(TreeNode node, Vector2 childPosition)
        {
            bool isLeaf = node.Left == null || node.Right == null;
            _materialBridge.SetInt("_Value", (int)node.Id);
            _materialBridge.SetColor("_Color", isLeaf ? _data.LeafColor : _data.InternalNodeColor);

            if (node.Id > _nodesCount + _nodesCount + 1)
            {
                _materialBridge.SetColor("_Color", Color.red);
            }

            _material.SetPass(0);
            Graphics.DrawMeshNow(_mesh, childPosition, Quaternion.identity, 0);
        }

        private void DrawLine(Vector2 parentPosition, Vector2 childPosition, float lineLength)
        {
            Vector2 direction = (parentPosition - childPosition).normalized;
            parentPosition -= direction * lineLength;
            childPosition += direction * lineLength;
            Debug.DrawLine(parentPosition, childPosition);
        }
    }
}