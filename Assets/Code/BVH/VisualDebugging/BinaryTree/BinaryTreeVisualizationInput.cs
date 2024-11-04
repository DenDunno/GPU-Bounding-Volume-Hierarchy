using System;
using UnityEngine;

namespace Code.Components.MortonCodeAssignment
{
    [Serializable]
    public class BinaryTreeVisualizationInput : VisualizationElementDataBase
    {
        [field: SerializeField] public float HorizontalOffset { get; private set; }
        [field: SerializeField] public float VerticalOffset { get; private set; }
        [field: SerializeField] public float LineLength { get; private set; }
        [field: SerializeField] public Color InternalNodeColor { get; private set; }
        [field: SerializeField] public Color LeafColor { get; private set; }
        
        public Vector2 Offset => new(HorizontalOffset, VerticalOffset);
    }
}