using System;
using UnityEngine;

namespace Code.Components.MortonCodeAssignment
{
    [Serializable]
    public class BVHTreeVisualizationData : VisualizationElementDataBase
    {
        [SerializeField] [Range(0, 100)] private int _visibleDepth;
        [SerializeField] [Range(0, 100)] private int _depth;
        
        [field: SerializeField] public bool ShowAll { get; private set; }
        [field: SerializeField] public float Alpha { get; private set; }
        [field: SerializeField] public Color Color { get; private set; }
        public int VisibleDepth => _visibleDepth;
        public int Depth => _depth;
    }
}