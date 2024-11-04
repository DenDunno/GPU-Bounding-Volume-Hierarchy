using System;
using UnityEngine;

namespace Code.Components.MortonCodeAssignment
{
    [Serializable]
    public class VisualizationData
    {
        [field: SerializeField] public BinaryTreeVisualizationInput BinaryTree { get; private set; }
    }
}