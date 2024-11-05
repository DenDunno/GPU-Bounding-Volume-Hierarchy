using System;
using UnityEngine;

namespace Code.Components.MortonCodeAssignment
{
    [Serializable]
    public class VisualizationData
    {
        [field: SerializeField] public BinaryTreeVisualizationData BinaryTree { get; private set; }
        [field: SerializeField] public BVHTreeVisualizationData BVHTree { get; private set; }
    }
}