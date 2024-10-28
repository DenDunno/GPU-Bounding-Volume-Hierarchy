using UnityEngine;

namespace DefaultNamespace.Code.GeometryGeneration
{
    public class PointInSphereGeneration : IPointGeneration
    {
        private readonly Vector3 _origin;
        private readonly float _radius;

        public PointInSphereGeneration(Vector3 origin, float radius)
        {
            _origin = origin;
            _radius = radius;
        }

        public Vector3 Evaluate()
        {
            return _origin + Random.insideUnitSphere * _radius;
        }
    }
}