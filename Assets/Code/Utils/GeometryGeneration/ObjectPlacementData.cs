using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DefaultNamespace.Code.GeometryGeneration
{
    [Serializable]
    public class ObjectPlacementData
    {
        [field: SerializeField, EnumToggleButtons]
        public Placement Placement { get; private set; }

        [field: SerializeField, Nested, ShowIf(nameof(Placement), Placement.Line)]
        public LineGenerationData LineGenerationData { get; private set; }

        [field: SerializeField, Nested, ShowIf(nameof(Placement), Placement.Cube)]
        public CubeGenerationData CubeGenerationData { get; private set; }

        [field: SerializeField, Nested, ShowIf(nameof(Placement), Placement.Sphere)]
        public SphereGenerationData SphereGenerationData { get; private set; }

        [field: SerializeField] public int Count { get; private set; } = 100;
        [field: SerializeField] public GameObject Prefab { get; private set; }

        public IPointGeneration CreateGenerationAlgorithm()
        {
            return Placement switch
            {
                Placement.Cube => new CubePointGeneration(CubeGenerationData),
                Placement.Sphere => new SpherePointGeneration(SphereGenerationData),
                Placement.Line => new LinePointGeneration(LineGenerationData),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}