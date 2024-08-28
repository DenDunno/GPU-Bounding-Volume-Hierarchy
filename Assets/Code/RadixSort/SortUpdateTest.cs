using Sirenix.OdinInspector;
using UnityEngine;

namespace Code
{
    public class SortUpdateTest : MonoBehaviour
    {
        [SerializeField] private ComputeShader _prefixSumShader;
        [SerializeField] private ComputeShader _sortShader;
        [SerializeField] private bool _update;
        [SerializeField] private int[] _input;
        [SerializeField] private int[] _output;
        private PrefixSumTest _test;
        private GPURadixSort _sort;
        
        [Button]
        private void Sort()
        {
            GPURadixSortInput input = new(_sortShader, _prefixSumShader, _input.Length);
            using GPURadixSort sort = new(input);
            
            sort.SetData(_input);
            sort.Execute(ref _output, _input.Length);
        }
    }
}