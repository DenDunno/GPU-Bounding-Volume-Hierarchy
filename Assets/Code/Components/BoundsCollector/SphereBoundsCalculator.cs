using Code.Data;
using UnityEngine;

namespace Code.Components.BoundsCollector
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

        public AABB Evaluate()
        {
            Vector3 offset = Vector3.one * _radius;
            
            return new AABB(_position - offset, _position + offset);
        }
    }
}