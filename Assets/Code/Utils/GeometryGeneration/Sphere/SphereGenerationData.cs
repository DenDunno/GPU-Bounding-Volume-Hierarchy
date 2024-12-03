using System;
using UnityEngine;

namespace DefaultNamespace.Code.GeometryGeneration
{
    [Serializable]
    public class SphereGenerationData
    {
        [field: SerializeField] public Vector3 Origin { get; private set; }
        [field: SerializeField] public float Radius { get; private set; }
    }
}