using System;
using UnityEngine;

namespace Code.Components.MortonCodeAssignment
{
    [Serializable]
    public class VisualizationData : VisualizationElementDataBase
    {
        [field: SerializeField] public BinaryTreeVisualizationData BinaryTree { get; private set; }
        [field: SerializeField] public BVHTreeVisualizationData BVHTree { get; private set; }
        [field: SerializeField] public bool ShowNearestNeighbours { get; private set; }
    }
}