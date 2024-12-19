using System;
using DefaultNamespace.Code.GeometryGeneration.Plane;
using EditorWrapper;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DefaultNamespace.Code.GeometryGeneration
{
    [Serializable]
    public class ObjectPlacementData
    {
        [SerializeField, EnumToggleButtons] private Placement _placement;

        [SerializeField, Nested, ShowIf(nameof(_placement), Placement.Line)] 
        private LineGenerationData _lineGenerationData;

        [SerializeField, Nested, ShowIf(nameof(_placement), Placement.Cube)]
        private CubeGenerationData _cubeGenerationData;

        [SerializeField, Nested, ShowIf(nameof(_placement), Placement.Sphere)]
        private SphereGenerationData _sphereGenerationData;

        [SerializeField, Nested, ShowIf(nameof(_placement), Placement.Plane)]
        private PlaneGenerationData _planeGenerationData;

        [field: SerializeField] public int Count { get; private set; } = 100;
        [field: SerializeField] public GameObject Prefab { get; private set; }
        [field: SerializeField] public bool ShowGenerationBounds { get; private set; }

        public IPointGeneration CreateGenerationAlgorithm()
        {
            return _placement switch
            {
                Placement.Sphere => new SpherePointGeneration(_sphereGenerationData),
                Placement.Plane => new PlanePointGeneration(_planeGenerationData),
                Placement.Cube => new CubePointGeneration(_cubeGenerationData),
                Placement.Line => new LinePointGeneration(_lineGenerationData),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public IDrawable CreateGenerationVisualization()
        {
            return _placement switch
            {
                Placement.Sphere => new SphereVisualization(_sphereGenerationData),
                Placement.Plane => new PlaneDebug(_planeGenerationData),
                Placement.Cube => DummyDrawable.Instance,
                Placement.Line => DummyDrawable.Instance,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}