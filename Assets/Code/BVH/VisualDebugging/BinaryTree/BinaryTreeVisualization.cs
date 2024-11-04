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
        private readonly BinaryTreeVisualizationInput _input;
        private readonly BinaryTreeWidth _width;
        private readonly Material _material;
        private readonly int _nodesCount;
        private readonly TreeNode _root;
        private readonly Mesh _mesh;

        public BinaryTreeVisualization(BinaryTreeVisualizationInput input,
            TreeNode root, int nodesCount)
        {
            _material = new Material(Resources.Load<Material>("Utilities/Digit"));
            _materialBridge = new MaterialBridge(_material);
            _mesh = MeshExtensions.BuildQuad(1f);
            _width = new BinaryTreeWidth(root);
            _nodesCount = nodesCount;
            _input = input;
            _root = root;
        }

        public void Initialize()
        {
            _width.Initialize();
        }

        public void Draw()
        {
            DrawDFS(_root, Vector2.zero, 0);
        }

        private void DrawDFS(TreeNode node, Vector2 parentPosition, float horizontalOffset)
        {
            if (node == null)
                return;

            Vector2 offset = new(horizontalOffset * _input.Offset.x, _input.Offset.y);
            Vector2 childPosition = parentPosition + offset;
            DrawLine(parentPosition, childPosition, _input.LineLength);
            DrawNumber(node, childPosition);
            DrawDFS(node.Left, childPosition, -_width.GetWidthForNode(node.Id));
            DrawDFS(node.Right, childPosition, _width.GetWidthForNode(node.Id));
        }

        private void DrawNumber(TreeNode node, Vector2 childPosition)
        {
            bool isLeaf = node.Id >= _nodesCount;
            _materialBridge.SetInt("_Value", isLeaf ? (int)node.Id - _nodesCount : (int)node.Id);
            _materialBridge.SetColor("_Color", isLeaf ? _input.LeafColor : _input.InternalNodeColor);

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