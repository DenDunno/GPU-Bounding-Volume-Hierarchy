using System.Collections.Generic;
using Code.Data;
using UnityEngine;

namespace Code.Components.MortonCodeAssignment
{
    public class ManualBoundingBoxesInput : IBoundingBoxesInput
    {
        private readonly IReadOnlyList<GameObject> _input;

        public ManualBoundingBoxesInput(IReadOnlyList<GameObject> input)
        {
            _input = input;
        }

        public AABB[] Calculate()
        {
            AABB[] output = new AABB[_input.Count];

            for (int i = 0; i < _input.Count; ++i)
            {
                IAABBProvider aabbProvider = _input[i].GetComponent<IAABBProvider>();
                output[i] = aabbProvider.CalculateBox();
            }

            return output;
        }
    }
}