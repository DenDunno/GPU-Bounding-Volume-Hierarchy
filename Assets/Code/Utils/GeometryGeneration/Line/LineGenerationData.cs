using System;
using UnityEngine;

namespace DefaultNamespace.Code.GeometryGeneration
{
    [Serializable]
    public class LineGenerationData
    {
        [field: SerializeField] public Vector3 Direction { get; private set; } = Vector3.right;
        [field: SerializeField] public float RandomMaxOffset { get; private set; } = 10;
        [field: SerializeField] public float Distance { get; private set; } = 10;
    }
}