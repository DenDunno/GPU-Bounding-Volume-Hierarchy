using System;
using UnityEngine;

namespace Code.Components.MortonCodeAssignment
{
    [Serializable]
    public class BVHData : IValidatedData
    {
        [field: SerializeField] public VisualizationData Visualization { get; private set; }
        [field: SerializeField] public BoundingBoxesInput BoxesInput { get; private set; }
        [field: SerializeField] public BVHConstructionId Algorithm { get; private set; }
        [field: SerializeField] public SceneSize SceneSize { get; private set; }
        [field: SerializeField] public bool IsStupidSearch { get; private set; }
        [HideInInspector] [SerializeField] public BVHNode[] Nodes;
        [HideInInspector] [SerializeField] public int Root;

        void IValidatedData.OnValidate()
        {
            BoxesInput.OnValidate();
        }

        bool IValidatedData.IsValid()
        {
            return BoxesInput.IsValid();
        }
    }
}