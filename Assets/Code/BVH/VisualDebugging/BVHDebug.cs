using System;
using Code.Components.MortonCodeAssignment.TestTree;
using UnityEngine;

namespace Code.Components.MortonCodeAssignment
{
    [Serializable]
    public class BVHDebug
    {
        [SerializeField] private BinaryTreeDebug _binaryTreeDebug;
        [SerializeField] private BVHTreeDebug _bvhTreeDebug;
        [SerializeField] private bool _showLeafsBounds;
        
        public void Initialize(int innerNodesCount, ComputeBuffer nodes)
        {
            BVHNode[] innerNodes = new BVHNode[innerNodesCount];
            nodes.GetData(innerNodes);

            BinaryTree tree = new(new TreeCalculator(innerNodes).Compute());
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