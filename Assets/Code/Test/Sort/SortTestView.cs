using System;
using System.Collections.Generic;
using Code.Utils.Extensions;
using UnityEngine;

namespace Code.Test.Sort
{
    [Serializable]
    public class SortTestView : CollectionComparisonTest
    {
        [SerializeField] private ComputeShader _sortShader;
        [SerializeField] private ComputeShader _prefixSumShader;
        [SerializeField] private int _size;

        protected override string TestName => "Sorting";
        
        protected override CollectionComparisonResult<int> RunComparisonTest(int[] input)
        {
            GPURadixSortInput sortInput = new(_sortShader, _prefixSumShader, input.Length);
            using GPURadixSort sort = new(sortInput);

            sort.SetData(input);
            sort.Execute(input.Length);
            
            List<int> expectedOutput = new(input);
            expectedOutput.Sort();

            return expectedOutput.IsSame(Output);
        }
    }
}