using UnityEngine;

namespace Code.Components.MortonCodeAssignment
{
    public class ShadersPresenter
    {
        public readonly ComputeShader MortonCodesSorting = Load("RadixSort/RadixSort");
        public readonly ComputeShader Setup = Load("MortonCode/Setup");

        private static ComputeShader Load(string path)
        {
            return Resources.Load<ComputeShader>(path);
        }
    }
}