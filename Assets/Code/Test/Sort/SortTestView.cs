using System;
using Code.Utils.Extensions;
using UnityEngine;

namespace Code.Test.Sort
{
    [Serializable]
    public class SortTestView : CollectionComparisonTest
    {
        [SerializeField] private ComputeShader _sortShader;
        [SerializeField] private ComputeShader _prefixSumShader;

        protected override string TestName => "Sorting";

        protected override CollectionComparisonResult<int> RunComparisonTest(int index, InputGenerationRules rules, int[] input)
        {
            int[] inputData = new RandomCollectionGeneration(rules.Seed, index, rules.MinValue, rules.MaxValue).Create();
            int[] output = new int[index];

            GPURadixSortInput sortInput = new(_sortShader, _prefixSumShader, inputData.Length);
            using GPURadixSort sort = new(sortInput);

            sort.SetData(inputData);
            sort.Execute(ref output, inputData.Length);

            Array.Sort(inputData);

            return output.IsSame(inputData);
        }
    }
}