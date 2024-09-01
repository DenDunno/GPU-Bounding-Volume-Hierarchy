using System;
using System.Collections.Generic;
using Code.Utils.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Code.Test.Sort
{
    [Serializable]
    public class SortTestView : CollectionComparisonTest
    {
        [SerializeField] private ComputeShader _sortShader;
        [SerializeField] private ComputeShader _prefixSumShader;
        
        protected override string TestName => "Sorting";
        
        [Button]
        public void CPUSort()
        {
            new CPURadixSortMock().Sort(Input, Output, 4);
        }
        
        protected override CollectionComparisonResult<int> RunComparisonTest(int[] input)
        {
            GPURadixSortInput sortInput = new(_sortShader, _prefixSumShader, input.Length);
            using GPURadixSort sort = new(sortInput);

            sort.SetData(input);
            sort.Execute(ref Output, input.Length);
            
            List<int> expectedOutput = new(input);
            expectedOutput.Sort();

            return expectedOutput.IsSame(Output);
        }
    }
}