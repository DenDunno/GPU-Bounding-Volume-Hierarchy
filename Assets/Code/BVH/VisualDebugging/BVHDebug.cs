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
        
        public void Initialize(BVHNode[] innerNodes, uint root)
        {
            BinaryTree tree = new(new TreeCalculator(innerNodes, root).Compute());
            _binaryTreeDebug.Initialize(innerNodes.Length, tree.Root);
            _bvhTreeDebug.Initialize(tree, innerNodes);
        }

        public void Draw()
        {
            _binaryTreeDebug.TryDraw();
            _bvhTreeDebug.TryDraw();
        }
    }
}