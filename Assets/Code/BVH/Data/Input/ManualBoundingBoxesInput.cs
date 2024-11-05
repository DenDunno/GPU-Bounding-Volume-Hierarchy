using System.Collections.Generic;
using Code.Data;
using UnityEngine;

namespace Code.Components.MortonCodeAssignment
{
    public class ManualBoundingBoxesInput : IBoundingBoxesInput
    {
        private readonly IReadOnlyList<MonoBehaviour> _input;

        public ManualBoundingBoxesInput(IReadOnlyList<MonoBehaviour> input)
        {
            _input = input;
        }

        public AABB[] Calculate()
        {
            AABB[] output = new AABB[_input.Count];

            for (int i = 0; i < _input.Count; ++i)
            {
                output[i] = ((IAABBProvider)_input[i]).CalculateBox();
            }

            return output;
        }
    }
}