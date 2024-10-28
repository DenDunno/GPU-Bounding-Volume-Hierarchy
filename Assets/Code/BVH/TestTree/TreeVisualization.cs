using System.Collections.Generic;
using TerraformingTerrain2d;
using UnityEngine;

namespace Code.Components.MortonCodeAssignment.TestTree
{
    public class TreeVisualization
    {
        private readonly float _depthFactor;
        private readonly float _depthFactor2;
        private readonly Material _material;
        private readonly Mesh _mesh;
        private readonly Dictionary<uint, int> _widthPerNode = new();

        public TreeVisualization(float depthFactor, float depthFactor2)
        {
            _depthFactor = depthFactor;
            _depthFactor2 = depthFactor2;
            _material = Resources.Load<Material>("Utilities/Digit");
            _mesh = MeshExtensions.BuildQuad(1);
        }

        public void Draw(TreeNode node)
        {
            CalculateWidthPerNodeDFS(node);
            DrawBFS(node);
        }

        private void DrawBFS(TreeNode node)
        {
            Queue<TreeNode> queue = new();
            queue.Enqueue(node);

            while (queue.Count != 0)
            {
                int layerCount = queue.Count;
                float distance = 0;
                
                while (layerCount-- > 0)
                {
                    TreeNode child = queue.Dequeue();
                    Dra
                    distance += _widthPerNode[child.Id];
                }
                queue.Enqueue(child.Left);
                queue.Enqueue(child.Right);
            }
        }

        private int CalculateWidthPerNodeDFS(TreeNode node)
        {
            if (node == null) return 0;
            
            return _widthPerNode[node.Id] = 1 + 
                                            CalculateWidthPerNodeDFS(node.Left) + 
                                            CalculateWidthPerNodeDFS(node.Right);
        }

        // public void Draw(TreeNode node, Vector2 position)
        // {
        //     if (node == null)
        //         return;
        //
        //     Debug.DrawLine(parentPosition, position, Color.red);
        //     DrawNumber(node, position);
        //     float xOffset = depth * _depthFactor + _depthFactor2;
        //     Draw(node.Left, position, position + new Vector2(-xOffset, -_offset), depth + 1, height);
        //     Draw(node.Right, position, position + new Vector2(xOffset, -_offset), depth + 1, height);
        // }

        private void DrawNumber(TreeNode node, Vector2 position)
        {
            Material material = new(_material);
            material.SetInt("_Value", (int)node.Id);
            Graphics.DrawMesh(_mesh, position, Quaternion.identity, material, 0);
        }
    }
}