using System;
using UnityEngine;

namespace DefaultNamespace.Code.GeometryGeneration.Plane
{
    [Serializable]
    public class PlaneGenerationData
    {
        [SerializeField] private Vector3 _normal = Vector3.up;
        [field: SerializeField] public Vector3 Point { get; private set; }
        [field: SerializeField] public float Radius { get; private set; } = 5;
        public Vector3 Normal => _normal.normalized;
    }
}