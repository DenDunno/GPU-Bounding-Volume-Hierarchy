using UnityEngine;

namespace Code.Components.MortonCodeAssignment
{
    public class BVHShaders
    {
        public readonly ComputeShader PlocPlusPLusShader;
        public readonly ComputeShader HPLOCShader;
        public readonly ComputeShader PrefixSum;
        public readonly ComputeShader Sorting;
        public readonly ComputeShader Setup;

        public BVHShaders(ComputeShader prefixSum, ComputeShader hplocShader,
            ComputeShader plocPlusPLusShader, ComputeShader sorting, ComputeShader setup)
        {
            HPLOCShader = hplocShader;
            PrefixSum = prefixSum;
            Sorting = sorting;
            Setup = setup;
            PlocPlusPLusShader = plocPlusPLusShader;
        }

        public static BVHShaders Load()
        {
            return new BVHShaders(
                plocPlusPLusShader: Load("BVH/PLOC/PLOC++"),
                prefixSum: Load("PrefixSum/PrefixSum"),
                sorting: Load("RadixSort/RadixSort"),
                setup: Load("MortonCode/Setup"),
                hplocShader: Load("BVH/HPLOC"));
        }

        private static ComputeShader Load(string path)
        {
            return Resources.Load<ComputeShader>(path);
        }
    }
}