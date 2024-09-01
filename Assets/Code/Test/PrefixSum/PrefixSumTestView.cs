using System;
using Code.Test;
using Code.Utils.Extensions;
using UnityEngine;

namespace Code
{
    [Serializable]
    public class PrefixSumTestView : CollectionComparisonTest
    {
        [SerializeField] private ComputeShader _prefixSumShader;

        protected override string TestName => "Prefix sum";

        protected override CollectionComparisonResult<int> RunComparisonTest(int[] input)
        {
            int[] outputPrefixSum = new int[input.Length];
            ComputeBuffer buffer = new(input.Length, sizeof(int));
            IGPUPrefixSum prefixSum = new GPUPrefixSumFactory(_prefixSumShader, buffer).Create();
            int[] expectedPrefixSum = new PrefixSumGeneration().Generate(input);
            
            buffer.SetData(input);
            prefixSum.Dispatch();
            buffer.GetData(outputPrefixSum);

            buffer.Dispose();
            prefixSum.Dispose();
            
            return expectedPrefixSum.IsSame(outputPrefixSum);
        }
    }
}