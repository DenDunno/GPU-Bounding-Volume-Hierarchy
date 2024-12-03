using System;
using UnityEngine;

namespace DefaultNamespace.Code.GeometryGeneration
{
    [Serializable]
    public class LineGenerationData
    {
        [field: SerializeField] public Vector3 Direction { get; private set; }
        [field: SerializeField] public float Distance { get; private set; }
    }
}