using Code.Utils.ShaderUtils.Buffer;
using UnityEngine;

namespace Code
{
    public class Test : MonoBehaviour
    {
        [SerializeField] private ComputeShader _prefixSumShader;
        [SerializeField] private ComputeShader _sortShader;
        [SerializeField] private int[] _input;
        [SerializeField] private int[] _output;
        private GPURadixSort _sort;

        private void OnValidate()
        {
            _output = new int[_input.Length];
        }

        private void Start()
        {
            GPURadixSortInput input = new(_sortShader, _prefixSumShader, _input.Length);
            _sort = new GPURadixSort(input);
            _sort.Initialize(new SetArrayOperation<int>(_input));
            _sort.Execute(_output);
        }

        private void Update()
        {
        }
    }
}