using UnityEngine;

namespace Code
{
    public class Test : MonoBehaviour
    {
        [SerializeField] private ComputeShader _prefixSumShader;
        [SerializeField] private ComputeShader _sortShader;
        [SerializeField] private int _seed = 0;
        [SerializeField] private int _size = 10;
        [SerializeField] private int[] _input;
        [SerializeField] private int[] _output;
        private GPURadixSort _sort;

        private void OnValidate()
        {
            _output = new int[_input.Length];
        }

        private void Start()
        {
            _input = new RandomCollectionGeneration(_seed, _size, 10).Create();
            _output = new int[_size];

            ComputeBuffer buffer = new(_input.Length, sizeof(int));
            buffer.SetData(_input);
            
            IGPUPrefixSum prefixSum = new GPUPrefixSumFactory(_prefixSumShader, buffer).Create();
            prefixSum.Dispatch();
            
            buffer.GetData(_output);
            ExpectedPrefixSum.CheckOutput(_input, _output);
        }
    }
}