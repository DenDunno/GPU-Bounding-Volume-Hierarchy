using UnityEngine;

namespace DefaultNamespace.Code.GeometryGeneration
{
    public class SpherePointGeneration : IPointGeneration
    {
        private readonly SphereGenerationData _data;

        public SpherePointGeneration(SphereGenerationData data)
        {
            _data = data;
        }

        public Vector3 Evaluate()
        {
            return _data.Origin + Random.insideUnitSphere * _data.Radius;
        }
    }
}