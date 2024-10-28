using UnityEngine;

namespace Code.Components.MortonCodeAssignment
{
    public class BVHShaders
    {
        public readonly ComputeShader BVHConstruction;
        public readonly ComputeShader PrefixSum;
        public readonly ComputeShader Sorting;
        public readonly ComputeShader Setup;

        public BVHShaders(ComputeShader bvhConstruction, ComputeShader prefixSum,
            ComputeShader sorting, ComputeShader setup)
        {
            BVHConstruction = bvhConstruction;
            PrefixSum = prefixSum;
            Sorting = sorting;
            Setup = setup;
        }

        public static BVHShaders Load()
        {
            return new BVHShaders(
                prefixSum: Load("PrefixSum/PrefixSum"),
                bvhConstruction: Load("BVH/HPLOC"),
                sorting: Load("RadixSort/RadixSort"),
                setup: Load("MortonCode/Setup"));
        }

        private static ComputeShader Load(string path)
        {
            return Resources.Load<ComputeShader>(path);
        }
    }
}