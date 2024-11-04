using System;
using UnityEngine;

namespace Code.Components.MortonCodeAssignment
{
    [Serializable]
    public class BVHData : IValidatedData
    {
        [field: SerializeField] public VisualizationData VisualizationData { get; private set; }
        [field: SerializeField] public int BufferSize { get; private set; } = 1;
        [field: SerializeField] public BVHContent Content { get; private set; } 

        void IValidatedData.OnValidate()
        {
            BufferSize = Mathf.Max(1, BufferSize);
        }

        bool IValidatedData.IsValid()
        {
            return true;
        }
    }
}