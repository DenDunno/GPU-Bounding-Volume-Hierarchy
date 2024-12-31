using UnityEngine;

namespace Code.Components.MortonCodeAssignment
{
    public class BVHShaders
    {
        public readonly ComputeShader PlocPlusPLus;
        public readonly ComputeShader PrefixSum;
        public readonly ComputeShader Sorting;
        public readonly ComputeShader HPLOC;
        public readonly ComputeShader Setup;

        public BVHShaders(ComputeShader prefixSum, ComputeShader hploc,
            ComputeShader plocPlusPLus, ComputeShader sorting, ComputeShader setup)
        {
            PlocPlusPLus = plocPlusPLus;
            PrefixSum = prefixSum;
            Sorting = sorting;
            HPLOC = hploc;
            Setup = setup;
        }

        public static BVHShaders Load()
        {
            return new BVHShaders(
                prefixSum: Load("PrefixSum/PrefixSum"),
                plocPlusPLus: Load("BVH/PLOC/PLOC++"),
                sorting: Load("RadixSort/RadixSort"),
                setup: Load("MortonCode/Setup"),
                hploc: Load("BVH/HPLOC"));
        }

        private static ComputeShader Load(string path)
        {
            return Resources.Load<ComputeShader>(path);
        }
    }
}