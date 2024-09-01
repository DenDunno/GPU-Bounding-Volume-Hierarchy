using System;
using Code.Test;
using UnityEngine;

namespace Code
{
    [Serializable]
    public class PrefixSumTestView : CollectionComparisonTest
    {
        [SerializeField] private ComputeShader _prefixSumShader;

        protected override string TestName => "Prefix sum";

        protected override CollectionComparisonResult<int> RunComparisonTest(int index, InputGenerationRules rules, int[] input)
        {
            using PrefixSumTest test = new(index, rules.Seed, _prefixSumShader);
            return test.Run();
        }
    }
}