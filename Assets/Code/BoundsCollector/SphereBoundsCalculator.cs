using UnityEngine;

namespace Code.SpheresBoundsCollector
{
    public readonly ref struct SphereBoundsCalculator
    {
        private readonly Vector3 _position;
        private readonly float _radius;

        public SphereBoundsCalculator(Vector3 position, float radius)
        {
            _position = position;
            _radius = radius;
        }

        public BoundingBox Evaluate()
        {
            Vector3 offset = Vector3.one * _radius;
            
            return new BoundingBox(_position - offset, _position + offset);
        }
    }
}