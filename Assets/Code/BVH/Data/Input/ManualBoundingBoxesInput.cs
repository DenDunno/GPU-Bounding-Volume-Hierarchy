using System.Collections.Generic;
using Code.Data;

namespace Code.Components.MortonCodeAssignment
{
    public class ManualBoundingBoxesInput : IBoundingBoxesInput
    {
        private readonly IReadOnlyList<IAABBProvider> _aabbProviders;
        private readonly AABB[] _result;

        public ManualBoundingBoxesInput(IReadOnlyList<IAABBProvider> aabbProviders)
        {
            _result = new AABB[aabbProviders.Count];
            _aabbProviders = aabbProviders;
        }

        public int Count => _result.Length;

        public AABB[] Calculate()
        {
            for (int i = 0; i < _aabbProviders.Count; ++i)
            {
                _result[i] = _aabbProviders[i].CalculateBox();
            }

            return _result;
        }
    }
}