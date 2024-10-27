using UnityEngine;

namespace Code.Components.MortonCodeAssignment
{
    public class ShadersPresenter
    {
        public ComputeShader PrefixSum { get; private set; }
        public ComputeShader Sorting { get; private set; }
        public ComputeShader Setup { get; private set; }

        public ShadersPresenter Load()
        {
            PrefixSum = Load("PrefixSum/PrefixSum");
            Sorting = Load("RadixSort/RadixSort");
            Setup = Load("MortonCode/Setup");
            return this;
        }
        
        private static ComputeShader Load(string path)
        {
            return Resources.Load<ComputeShader>(path);
        }
    }
}