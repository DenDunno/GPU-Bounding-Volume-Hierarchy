using System;
using UnityEngine;

namespace DefaultNamespace.Code.GeometryGeneration
{
    [Serializable]
    public class CubeGenerationData
    {
        [field: SerializeField] public Vector3Int Dimensions { get; private set; } = Vector3Int.one;
        [field: SerializeField] public float Distance { get; private set; } = 10;
    }
}