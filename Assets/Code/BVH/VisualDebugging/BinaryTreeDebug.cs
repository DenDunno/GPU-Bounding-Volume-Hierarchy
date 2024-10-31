using System;
using System.Collections.Generic;
using Code.Components.MortonCodeAssignment.TestTree;
using TerraformingTerrain2d;
using UnityEngine;

namespace Code.Components.MortonCodeAssignment
{
    [Serializable]
    public class BinaryTreeDebug : DebugElement
    {
        [SerializeField] private float _depthFactor;
        [SerializeField] private float _lineLength;
        private readonly Dictionary<uint, int> _widthPerNode = new();
        private int _innerNodesCount;
        private Material _material;
        private TreeNode _root;
        private Mesh _mesh;

        public void Initialize(int innerNodesCount, TreeNode node)
        {
            CalculateWidthPerNodeDFS(node);
            _material = Resources.Load<Material>("Utilities/Digit");
            _mesh = MeshExtensions.BuildQuad(1);
            _innerNodesCount = innerNodesCount;
            _root = node;
        }

        protected override void OnDraw()
        {
            DrawDFS(_root, Vector2.zero, 0);
        }

        private int CalculateWidthPerNodeDFS(TreeNode node)
        {
            if (node == null) return 0;
            
            return _widthPerNode[node.Id] = 1 + 
                                            CalculateWidthPerNodeDFS(node.Left) + 
                                            CalculateWidthPerNodeDFS(node.Right);
        }

        private void DrawDFS(TreeNode node, Vector2 parentPosition, float offset)
        {
            if (node == null)
                return;

            Vector2 childPosition = parentPosition + new Vector2(offset * _depthFactor, -2);
            DrawLine(parentPosition, childPosition, _lineLength);
            DrawNumber(node, childPosition);
            DrawDFS(node.Left, childPosition, -_widthPerNode[node.Id]);
            DrawDFS(node.Right, childPosition,  _widthPerNode[node.Id]);
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

            material.SetPass(0);
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