using System;
using UnityEngine;

namespace Code.Components.MortonCodeAssignment
{
    [Serializable]
    public class BVHData : IValidatedData
    {
        [field: SerializeField] public VisualizationData Visualization { get; private set; }
        [field: SerializeField] public BoundingBoxesInput BoxesInput { get; private set; }
        [field: SerializeField] public BVHConstructionAlgorithmId Algorithm { get; private set; } 
            = BVHConstructionAlgorithmId.PLOCPlusPlus;

        void IValidatedData.OnValidate()
        {
            BoxesInput.OnValidate();
        }

        bool IValidatedData.IsValid()
        {
            return true;
        }
    }
}