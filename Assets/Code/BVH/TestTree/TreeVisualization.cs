using System.Collections.Generic;
using TerraformingTerrain2d;
using UnityEngine;

namespace Code.Components.MortonCodeAssignment.TestTree
{
    public class TreeVisualization
    {
        private readonly Dictionary<uint, int> _widthPerNode = new();
        private readonly Material _material;
        private readonly int _leavesCount;
        private readonly Mesh _mesh;
        private int _height;

        public TreeVisualization(int leavesCount)
        {
            _leavesCount = leavesCount;
            _material = Resources.Load<Material>("Utilities/Digit");
            _mesh = MeshExtensions.BuildQuad(1);
        }

        public void Draw(TreeNode node, float depthFactor)
        {
            DrawDFS(node, Vector2.zero, depthFactor, 0, 1);
        }

        public void Initialize(TreeNode node)
        {
            _height = ComputeHeight(node, 1);
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

        private void DrawDFS(TreeNode node, Vector2 parentPosition, float depthFactor, float offset, float depth)
        {
            if (node == null)
                return;

            Vector2 childPosition = parentPosition + new Vector2(offset * depth / _height * depthFactor, -2);
            DrawNumber(node, parentPosition, childPosition);
            DrawDFS(node.Left, childPosition, depthFactor, -_widthPerNode[node.Id] / 2f, depth + 1);
            DrawDFS(node.Right, childPosition,depthFactor,  _widthPerNode[node.Id] / 2f, depth + 1);
        }

        private int CalculateWidthPerNodeDFS(TreeNode node)
        {
            if (node == null) return 0;
            
            return _widthPerNode[node.Id] = 1 + 
                                            CalculateWidthPerNodeDFS(node.Left) + 
                                            CalculateWidthPerNodeDFS(node.Right);
        }

        private void DrawNumber(TreeNode node, Vector2 parentPosition, Vector2 childPosition)
        {
            Material material = new(_material);
            bool isLeaf = node.Id >= _leavesCount;
            material.SetInt("_Value", isLeaf ? (int)node.Id - _leavesCount : (int)node.Id);
            material.SetColor("_Color", isLeaf ? new Color(0.77f, 0.64f, 0f) : new Color(0.11f, 0.44f, 0.07f));

            if (node.Id > _leavesCount + _leavesCount - 1) 
            {
                material.SetColor("_Color", Color.red);
            }
            
            Debug.DrawLine(parentPosition, childPosition);
            Graphics.DrawMesh(_mesh, childPosition, Quaternion.identity, material, 0);
        }
    }
}