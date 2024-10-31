using System.Collections.Generic;
using TerraformingTerrain2d;
using UnityEngine;

namespace Code.Components.MortonCodeAssignment.TestTree
{
    public class TreeVisualization
    {
        private readonly Dictionary<uint, int> _widthPerNode = new();
        private readonly Material _material;
        private readonly int _innerNodesCount;
        private readonly Mesh _mesh;
        private float _depthFactor;
        private float _lineLength;
        private int _height;

        public TreeVisualization(int innerNodesCount)
        {
            _innerNodesCount = innerNodesCount;
            _material = Resources.Load<Material>("Utilities/Digit");
            _mesh = MeshExtensions.BuildQuad(1);
        }

        public void Draw(TreeNode node, float depthFactor, float lineLength)
        {
            _depthFactor = depthFactor;
            _lineLength = lineLength;
            DrawDFS(node, Vector2.zero, 0, 1);
        }

        public void Initialize(TreeNode node)
        {
            _height = ComputeHeight(node, 0);
            CalculateWidthPerNodeDFS(node);
        }

        private int ComputeHeight(TreeNode node, int depth)
        {
            if (node == null)
                return depth;

            return Mathf.Max(
                ComputeHeight(node.Left, depth + 1), 
                ComputeHeight(node.Right, depth + 1));
        }

        private void DrawDFS(TreeNode node, Vector2 parentPosition, float offset, float depth)
        {
            if (node == null)
                return;

            Vector2 childPosition = parentPosition + new Vector2(offset * _depthFactor, -2);
            DrawLine(parentPosition, childPosition, _lineLength);
            DrawNumber(node, childPosition);
            DrawDFS(node.Left, childPosition, -_widthPerNode[node.Id], depth + 1);
            DrawDFS(node.Right, childPosition,  _widthPerNode[node.Id], depth + 1);
        }

        private int CalculateWidthPerNodeDFS(TreeNode node)
        {
            if (node == null) return 0;
            
            return _widthPerNode[node.Id] = 1 + 
                                            CalculateWidthPerNodeDFS(node.Left) + 
                                            CalculateWidthPerNodeDFS(node.Right);
        }

        private void DrawNumber(TreeNode node, Vector2 childPosition)
        {
            Material material = new(_material);
            bool isLeaf = node.Id >= _innerNodesCount;
            material.SetInt("_Value", isLeaf ? (int)node.Id - _innerNodesCount : (int)node.Id);
            material.SetColor("_Color", isLeaf ? new Color(0.77f, 0.64f, 0f) : new Color(0.11f, 0.44f, 0.07f));

            if (node.Id > _innerNodesCount + _innerNodesCount + 1) 
            {
                material.SetColor("_Color", Color.red);
            }
            
            Graphics.DrawMesh(_mesh, childPosition, Quaternion.identity, material, 0);
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