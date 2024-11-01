using System;
using Code.Components.MortonCodeAssignment.TestTree;
using Code.Utils.Extensions;
using UnityEngine;

namespace Code.Components.MortonCodeAssignment
{
    [Serializable]
    public class BVHDebug
    {
        [SerializeField] private BinaryTreeDebug _binaryTreeDebug;
        [SerializeField] private BVHTreeDebug _bvhTreeDebug;
        [SerializeField] private bool _showLeafsBounds;
        
        public void Initialize(int innerNodesCount, BVHBuffers buffers)
        {
            BVHNode[] innerNodes = new BVHNode[innerNodesCount];
            buffers.Nodes.GetData(innerNodes);
            uint rootIndex = buffers.Root.GetValue<uint>();

            BinaryTree tree = new(new TreeCalculator(innerNodes, rootIndex).Compute());
            _binaryTreeDebug.Initialize(innerNodesCount, tree.Root);
            _bvhTreeDebug.Initialize(tree, innerNodes);
        }

        public void Draw()
        {
            _binaryTreeDebug.TryDraw();
            _bvhTreeDebug.TryDraw();
        }
    }
}