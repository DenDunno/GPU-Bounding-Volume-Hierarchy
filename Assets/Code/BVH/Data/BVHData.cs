using System;
using UnityEngine;

namespace Code.Components.MortonCodeAssignment
{
    [Serializable]
    public class BVHData : IValidatedData
    {
        [field: SerializeField] public int BufferSize { get; private set; } = 1;
        
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