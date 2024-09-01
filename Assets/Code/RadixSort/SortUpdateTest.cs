using System.Collections.Generic;
using Code.Utils.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Code
{
    public class SortUpdateTest : MonoBehaviour
    {
        [SerializeField] private ComputeShader _chunkSortShader;
        [SerializeField] private ComputeShader _prefixSumShader;
        [SerializeField] private bool _update;
        [SerializeField] private int[] _input;
        [SerializeField] private int[] _output;
        private PrefixSumTest _test;
        private GPURadixSort _sort;
        
        [Button]
        private void Sort()
        {
            GPURadixSortInput input = new(_chunkSortShader, _prefixSumShader, _input.Length);
            using GPURadixSort sort = new(input);
            
            sort.SetData(_input);
            sort.Execute(ref _output, _input.Length);
            CheckIfExpected();
        }

        private void CheckIfExpected()
        {
            List<int> expectedArray = new(_input);
            expectedArray.Sort();
            CollectionComparisonResult<int> result = expectedArray.IsSame(_output);
            
            if (result.IsEqual == false)
            {
                Debug.LogError($"Sort test failed at index {result.Index}. " +
                               $"First = {result.FirstValue} Second = {result.SecondValue}");
            }
        }
    }
}